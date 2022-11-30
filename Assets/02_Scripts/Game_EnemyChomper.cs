using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Game_EnemyChomper : MonoBehaviour
{
    Transform player; //플레이어좌표
    private NavMeshAgent pathFinder; //경로 계산 AI 에이전트

    public ParticleSystem hitEffect; //피격 시 재생할 파티클 효과
    public AudioClip deathSound; //사망시 재생할 오디오 소스
    public AudioClip hitSound; //피격시 재생할 오디오 소스
    public AudioClip attackSound; //피격시 재생할 오디오 소스

    private Animator enemyAnimator; //애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; //오디오 소스 컴포넌트
    private Renderer enemyRenderer; //랜더러 컴포넌트

    public float damage = 2f; //공격력
    public float curHealth = 20f; //현재체력
    public float maxHealth = 20f; //최대체력
    public float attackDistance = 4.0f; //공격사거리
    public float timeBetAttack = 1f; //공격 간격
    private float lastAttackTime; //마지막 공격 시점

    private bool isdead;
    Animator anim;
    NavMeshAgent chom;

    private void Awake()
    {
        //게임 오브젝트에서 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();

        enemyRenderer = GetComponentInChildren<Renderer>();
        isdead = false;
    }

    enum ChomperState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    ChomperState chomperState;

    private void Start()
    {
        player = GameObject.Find("Player").transform; //플레이어 좌표 받아오기

        chom = GetComponent<NavMeshAgent>();
        anim = transform.GetComponentInChildren<Animator>();  //자식으로부터 애니메이터 변수 받아오기
    }

    private void Update()
    {
        if (!isdead)
        {
            switch (chomperState)
            {
                case ChomperState.Idle:
                    Idle();
                    break;
                case ChomperState.Move:
                    Move();
                    break;
                case ChomperState.Attack:
                    Attack();
                    break;
                case ChomperState.Damaged:
                    //Damaged();
                    break;
                case ChomperState.Die:
                    //Die();
                    break;
            }
        }
    }

    void Idle() //대기 상태
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)  //공격범위 안이라면
        {
            anim.SetTrigger("IdleToAttack");
            chomperState = ChomperState.Attack;
        }
        if (!Player_Health.instance.isDead)
        {
            chomperState = ChomperState.Move;
        }
    }

    void Move() //이동상태
    {
        //공격 사거리내로 들어오면 공격으로 전환 
        if(Vector3.Distance(transform.position, player.position) > attackDistance) //공격범위 밖이라면
        {
            //플레이어를 쫒는다.
            anim.SetTrigger("IdleToMove");
            chom.isStopped = true;
            chom.ResetPath(); //기존 경로를 지운다.
            chom.stoppingDistance = attackDistance; // 내비로 접근 최소 거리 설정
            chom.destination = player.position; //내비 목적지 설정
        }
        else //공격범위 안이라면
        {
            //플레이어를 공격한다.
            chomperState = ChomperState.Attack;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    void Attack() //공격
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)  //공격범위 안이라면
        {
            lastAttackTime += Time.deltaTime; //변수에 시간누적
            if (lastAttackTime > timeBetAttack) // 시간이 되었다면
            {
                transform.LookAt(player); //플레이어를 바라보고
                anim.SetTrigger("MoveToAttack"); //공격 애니메이션
                enemyAudioPlayer.PlayOneShot(attackSound); //공격 소리 재생
                Player_Health.instance.IncDegHp("Hungry", -damage); //데미지 처리문
                print("공격");
                lastAttackTime = 0f; //시간 초기화
                anim.SetTrigger("AttackToIdle");
                chomperState = ChomperState.Idle;
            }

        }
        else //공격범위 밖이라면
        {
            //플레이어 재추격
            anim.SetTrigger("AttackToIdle"); 
            chomperState = ChomperState.Idle;
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

    public void HitEnemy(float hitPower) //적이 공격 받았을 때 체력 감소 함수
    {
        if (chomperState == ChomperState.Damaged || chomperState == ChomperState.Die)
        {
            return; //이미 피격되고있거나 사망 상태일경우 함수를 종료한다.
        }

        enemyAudioPlayer.PlayOneShot(hitSound); //피격 소리 재생
        curHealth -= hitPower; //적 데미지처리

        if (curHealth > 0)
        {
            chomperState = ChomperState.Damaged; // 상태 전환
            anim.SetTrigger("Damaged"); //애니메이션 실행
            Damaged();
        }
        else
        {
            chomperState = ChomperState.Die; //상태 전환
            anim.SetTrigger("Die"); //애니메이션 실행
            Die();
        }
    }

    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.5f); //피격 모션 시간만큼 대기
        chomperState = ChomperState.Move; //이동상태로 전환
    }

    IEnumerator DieProcess()
    {
        isdead = true;
        enemyAudioPlayer.PlayOneShot(deathSound); //사망 소리 재생
        Game_Score.instance.killCnt++; //점수용 킬카운트 추가
        yield return new WaitForSeconds(1f); // 1초 대기후 자기자신 제거
        Destroy(gameObject);
    }
}
