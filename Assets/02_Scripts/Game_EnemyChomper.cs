using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Game_EnemyChomper : MonoBehaviour
{
    Transform player; //�÷��̾���ǥ
    private NavMeshAgent pathFinder; //��� ��� AI ������Ʈ

    public ParticleSystem hitEffect; //�ǰ� �� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; //����� ����� ����� �ҽ�
    public AudioClip hitSound; //�ǰݽ� ����� ����� �ҽ�
    public AudioClip attackSound; //�ǰݽ� ����� ����� �ҽ�

    private Animator enemyAnimator; //�ִϸ����� ������Ʈ
    private AudioSource enemyAudioPlayer; //����� �ҽ� ������Ʈ
    private Renderer enemyRenderer; //������ ������Ʈ

    public float damage = 2f; //���ݷ�
    public float curHealth = 20f; //����ü��
    public float maxHealth = 20f; //�ִ�ü��
    public float attackDistance = 4.0f; //���ݻ�Ÿ�
    public float timeBetAttack = 1f; //���� ����
    private float lastAttackTime; //������ ���� ����

    private bool isdead;
    Animator anim;
    NavMeshAgent chom;

    private void Awake()
    {
        //���� ������Ʈ���� ����� ������Ʈ ��������
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
        player = GameObject.Find("Player").transform; //�÷��̾� ��ǥ �޾ƿ���

        chom = GetComponent<NavMeshAgent>();
        anim = transform.GetComponentInChildren<Animator>();  //�ڽ����κ��� �ִϸ����� ���� �޾ƿ���
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

    void Idle() //��� ����
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)  //���ݹ��� ���̶��
        {
            anim.SetTrigger("IdleToAttack");
            chomperState = ChomperState.Attack;
        }
        if (!Player_Health.instance.isDead)
        {
            chomperState = ChomperState.Move;
        }
    }

    void Move() //�̵�����
    {
        //���� ��Ÿ����� ������ �������� ��ȯ 
        if(Vector3.Distance(transform.position, player.position) > attackDistance) //���ݹ��� ���̶��
        {
            //�÷��̾ �i�´�.
            anim.SetTrigger("IdleToMove");
            chom.isStopped = true;
            chom.ResetPath(); //���� ��θ� �����.
            chom.stoppingDistance = attackDistance; // ����� ���� �ּ� �Ÿ� ����
            chom.destination = player.position; //���� ������ ����
        }
        else //���ݹ��� ���̶��
        {
            //�÷��̾ �����Ѵ�.
            chomperState = ChomperState.Attack;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    void Attack() //����
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)  //���ݹ��� ���̶��
        {
            lastAttackTime += Time.deltaTime; //������ �ð�����
            if (lastAttackTime > timeBetAttack) // �ð��� �Ǿ��ٸ�
            {
                transform.LookAt(player); //�÷��̾ �ٶ󺸰�
                anim.SetTrigger("MoveToAttack"); //���� �ִϸ��̼�
                enemyAudioPlayer.PlayOneShot(attackSound); //���� �Ҹ� ���
                Player_Health.instance.IncDegHp("Hungry", -damage); //������ ó����
                print("����");
                lastAttackTime = 0f; //�ð� �ʱ�ȭ
                anim.SetTrigger("AttackToIdle");
                chomperState = ChomperState.Idle;
            }

        }
        else //���ݹ��� ���̶��
        {
            //�÷��̾� ���߰�
            anim.SetTrigger("AttackToIdle"); 
            chomperState = ChomperState.Idle;
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

    public void HitEnemy(float hitPower) //���� ���� �޾��� �� ü�� ���� �Լ�
    {
        if (chomperState == ChomperState.Damaged || chomperState == ChomperState.Die)
        {
            return; //�̹� �ǰݵǰ��ְų� ��� �����ϰ�� �Լ��� �����Ѵ�.
        }

        enemyAudioPlayer.PlayOneShot(hitSound); //�ǰ� �Ҹ� ���
        curHealth -= hitPower; //�� ������ó��

        if (curHealth > 0)
        {
            chomperState = ChomperState.Damaged; // ���� ��ȯ
            anim.SetTrigger("Damaged"); //�ִϸ��̼� ����
            Damaged();
        }
        else
        {
            chomperState = ChomperState.Die; //���� ��ȯ
            anim.SetTrigger("Die"); //�ִϸ��̼� ����
            Die();
        }
    }

    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.5f); //�ǰ� ��� �ð���ŭ ���
        chomperState = ChomperState.Move; //�̵����·� ��ȯ
    }

    IEnumerator DieProcess()
    {
        isdead = true;
        enemyAudioPlayer.PlayOneShot(deathSound); //��� �Ҹ� ���
        Game_Score.instance.killCnt++; //������ ųī��Ʈ �߰�
        yield return new WaitForSeconds(1f); // 1�� ����� �ڱ��ڽ� ����
        Destroy(gameObject);
    }
}
