using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Game_Enemy : MonoBehaviour
{
    CharacterController cc;
    Transform player;   //플레이어 좌표
    Vector3 originPos; //적의 초기좌표

    public float findDistance = 8f; //인식범위
    public float attackDistance = 2.5f; //공격범위
    public float moveDistance = 20f; //적 이동 반경
    public float moveSpeed = 3f; //이동속도
    public float attackPower = 3f; //적의 공격 데미지
    public float enemyHp = 50f; //적의 현재 체력
    public float enemyMaxHp = 50f; //적의 최대 체력
    float attackcurTime = 0f;  //공격시간 누적 변수
    float attackDelay = 2f;  //공격 딜레이 시간

    Animator anim;
    NavMeshAgent chom;


    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState enemyState;

    private void Start()
    {
        enemyState = EnemyState.Idle; //초기 상태 대기 상태
        attackcurTime = attackDelay; //조우시에는 바로 공격하도록
        player = GameObject.Find("Player").transform; //플레이어 좌표 받아오기
        cc = GetComponent<CharacterController>(); //캐릭터 컴포넌트 받아오기
        originPos = transform.position;

        chom = GetComponent<NavMeshAgent>();
        anim = transform.GetComponentInChildren<Animator>();  //자식으로부터 애니메이터 변수 받아오기
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }
    }

    void Idle() //대기 상태
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            enemyState = EnemyState.Move; //상대 전환
            anim.SetTrigger("IdleToMove"); //이동 애니메이션으로 전환
        }
    }

    void Move() //이동상태
    {
        if (Vector3.Distance(transform.position,originPos) > moveDistance) //이동 반경을 벗어난다면
        {
            enemyState = EnemyState.Return; //적 복귀로 상태변경
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance) //공격범위 밖이라면
        {
            chom.isStopped = true;
            chom.ResetPath();
            chom.stoppingDistance = attackDistance; // 내비로 접근 최소 거리 설정
            chom.destination = player.position; //내비 목적지 설정
            
        }
        else //공격범위 안이라면
        {
            enemyState = EnemyState.Attack; //적 공격 상태로 바꿈
            anim.SetTrigger("MoveToAttackdelay");//공격 대기 애니메이션
        }


    }

    void Attack() //공격
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)  //공격범위 안이라면
        {
            attackcurTime += Time.deltaTime; //변수에 시간누적
            if (attackcurTime > attackDelay) // 시간이 되었다면
            {
                Player_Health.instance.IncDegHp("Hungry", -attackPower); //데미지 처리문
                print("공격");
                attackcurTime = 0f; //시간 초기화
                anim.SetTrigger("StartAttack"); //공격 애니메이션
            }

        }
        else //공격범위 밖이라면
        {
            enemyState = EnemyState.Move; //플레이어 재추격
            anim.SetTrigger("AttackToMove"); //이동 애니메이션
            attackcurTime = 0f; //시간 초기화
        }

    }

    void Return()
    {
        if (Vector3.Distance(transform.position,originPos) > 0.3f) //초기좌표와 이동값이 오차가 있다면
        {
            Vector3 dir = (originPos - transform.position).normalized; //방향설정
            cc.Move(dir * moveSpeed * Time.deltaTime); //이동

            transform.forward = dir; //정면을 복귀방향을 향하도록 한다.
        }
        else
        {
            transform.position = originPos; 
            enemyHp = enemyMaxHp; //체력회복
            attackcurTime = attackDelay; //조우시에는 바로 공격하도록
            enemyState = EnemyState.Idle; //대기상태로 전환
            anim.SetTrigger("MoveToIdle"); //대기 애니메이션으로 전환
        }
    }
    
    public void HitEnemy(float hitPower) //적이 공격 받았을 때 체력 감소 함수
    {
        if(enemyState == EnemyState.Damaged || enemyState == EnemyState.Die || enemyState == EnemyState.Return)
        {
            return; //이미 피격되고있거나 사망또는 복귀상태일경우 함수를 종료한다.
        }

        enemyHp -= hitPower; //적 데미지처리

        if(enemyHp > 0)
        {
            enemyState = EnemyState.Damaged; // 상태 전환
            anim.SetTrigger("Damaged"); //애니메이션 실행
            Damaged();
        }
        else
        {
            enemyState = EnemyState.Die; //상태 전환
            anim.SetTrigger("Die"); //애니메이션 실행
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess()); //코루틴함수 시작
    }

    void Die()
    {
        StopAllCoroutines(); //진행중인 피격판정 모두 종료

        StartCoroutine(DieProcess());
    }

    
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.5f); //피격 모션 시간만큼 대기
        enemyState = EnemyState.Move; //이동상태로 전환
    }

    IEnumerator DieProcess()
    {
        Game_Score.instance.killCnt++; //점수용 킬카운트 추가
        cc.enabled = false; //cc를 비활성화
        yield return new WaitForSeconds(1f); // 1초 대기후 자기자신 제거
        Destroy(gameObject);
    }
}
