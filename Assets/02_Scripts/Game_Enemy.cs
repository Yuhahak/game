using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Game_Enemy : MonoBehaviour
{
    CharacterController cc;
    Transform player;   //�÷��̾� ��ǥ
    Vector3 originPos; //���� �ʱ���ǥ

    public float findDistance = 8f; //�νĹ���
    public float attackDistance = 2.5f; //���ݹ���
    public float moveDistance = 20f; //�� �̵� �ݰ�
    public float moveSpeed = 3f; //�̵��ӵ�
    public float attackPower = 3f; //���� ���� ������
    public float enemyHp = 50f; //���� ���� ü��
    public float enemyMaxHp = 50f; //���� �ִ� ü��
    float attackcurTime = 0f;  //���ݽð� ���� ����
    float attackDelay = 2f;  //���� ������ �ð�

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
        enemyState = EnemyState.Idle; //�ʱ� ���� ��� ����
        attackcurTime = attackDelay; //����ÿ��� �ٷ� �����ϵ���
        player = GameObject.Find("Player").transform; //�÷��̾� ��ǥ �޾ƿ���
        cc = GetComponent<CharacterController>(); //ĳ���� ������Ʈ �޾ƿ���
        originPos = transform.position;

        chom = GetComponent<NavMeshAgent>();
        anim = transform.GetComponentInChildren<Animator>();  //�ڽ����κ��� �ִϸ����� ���� �޾ƿ���
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

    void Idle() //��� ����
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            enemyState = EnemyState.Move; //��� ��ȯ
            anim.SetTrigger("IdleToMove"); //�̵� �ִϸ��̼����� ��ȯ
        }
    }

    void Move() //�̵�����
    {
        if (Vector3.Distance(transform.position,originPos) > moveDistance) //�̵� �ݰ��� ����ٸ�
        {
            enemyState = EnemyState.Return; //�� ���ͷ� ���º���
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance) //���ݹ��� ���̶��
        {
            chom.isStopped = true;
            chom.ResetPath();
            chom.stoppingDistance = attackDistance; // ����� ���� �ּ� �Ÿ� ����
            chom.destination = player.position; //���� ������ ����
            
        }
        else //���ݹ��� ���̶��
        {
            enemyState = EnemyState.Attack; //�� ���� ���·� �ٲ�
            anim.SetTrigger("MoveToAttackdelay");//���� ��� �ִϸ��̼�
        }


    }

    void Attack() //����
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)  //���ݹ��� ���̶��
        {
            attackcurTime += Time.deltaTime; //������ �ð�����
            if (attackcurTime > attackDelay) // �ð��� �Ǿ��ٸ�
            {
                Player_Health.instance.IncDegHp("Hungry", -attackPower); //������ ó����
                print("����");
                attackcurTime = 0f; //�ð� �ʱ�ȭ
                anim.SetTrigger("StartAttack"); //���� �ִϸ��̼�
            }

        }
        else //���ݹ��� ���̶��
        {
            enemyState = EnemyState.Move; //�÷��̾� ���߰�
            anim.SetTrigger("AttackToMove"); //�̵� �ִϸ��̼�
            attackcurTime = 0f; //�ð� �ʱ�ȭ
        }

    }

    void Return()
    {
        if (Vector3.Distance(transform.position,originPos) > 0.3f) //�ʱ���ǥ�� �̵����� ������ �ִٸ�
        {
            Vector3 dir = (originPos - transform.position).normalized; //���⼳��
            cc.Move(dir * moveSpeed * Time.deltaTime); //�̵�

            transform.forward = dir; //������ ���͹����� ���ϵ��� �Ѵ�.
        }
        else
        {
            transform.position = originPos; 
            enemyHp = enemyMaxHp; //ü��ȸ��
            attackcurTime = attackDelay; //����ÿ��� �ٷ� �����ϵ���
            enemyState = EnemyState.Idle; //�����·� ��ȯ
            anim.SetTrigger("MoveToIdle"); //��� �ִϸ��̼����� ��ȯ
        }
    }
    
    public void HitEnemy(float hitPower) //���� ���� �޾��� �� ü�� ���� �Լ�
    {
        if(enemyState == EnemyState.Damaged || enemyState == EnemyState.Die || enemyState == EnemyState.Return)
        {
            return; //�̹� �ǰݵǰ��ְų� ����Ǵ� ���ͻ����ϰ�� �Լ��� �����Ѵ�.
        }

        enemyHp -= hitPower; //�� ������ó��

        if(enemyHp > 0)
        {
            enemyState = EnemyState.Damaged; // ���� ��ȯ
            anim.SetTrigger("Damaged"); //�ִϸ��̼� ����
            Damaged();
        }
        else
        {
            enemyState = EnemyState.Die; //���� ��ȯ
            anim.SetTrigger("Die"); //�ִϸ��̼� ����
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess()); //�ڷ�ƾ�Լ� ����
    }

    void Die()
    {
        StopAllCoroutines(); //�������� �ǰ����� ��� ����

        StartCoroutine(DieProcess());
    }

    
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.5f); //�ǰ� ��� �ð���ŭ ���
        enemyState = EnemyState.Move; //�̵����·� ��ȯ
    }

    IEnumerator DieProcess()
    {
        Game_Score.instance.killCnt++; //������ ųī��Ʈ �߰�
        cc.enabled = false; //cc�� ��Ȱ��ȭ
        yield return new WaitForSeconds(1f); // 1�� ����� �ڱ��ڽ� ����
        Destroy(gameObject);
    }
}
