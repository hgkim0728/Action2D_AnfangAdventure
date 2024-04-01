using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    enum MonsterState
    {
        Idle,
        Move,
        Trace,
        Attack,
        Die
    }

    // 몬스터 이동
    [SerializeField, Tooltip("몬스터 이동속도")] private float moveSpeed = 5.0f;
    private int monsterDir = 1;

    // 전투
    [Space]
    [SerializeField, Tooltip("플레이어 위치")] private Transform trsPlayer;
    [SerializeField, Tooltip("몬스터 공격 범위")] private float monsterAttackRange = 1f;
    [SerializeField, Tooltip("몬스터 체력")] private int monsterHp = 2;
    [SerializeField, Tooltip("몬스터 공격력")] private int monsterAtk = 1;
    [SerializeField, Tooltip("공격 쿨타임")] private float attackCoolTime = 1.0f;
    [SerializeField, Tooltip("피격당했을 때 날아갈 거리")] private float hitImpulse = 3.0f;
    [SerializeField, Tooltip("몬스터가 죽었는지 아닌지")] private bool isDie = false;

    // 상태 변경 시간
    [Space]
    [SerializeField, Tooltip("상태 변경 최소 시간")] private float minStateChangeTime = 2.0f;
    [SerializeField, Tooltip("상태 변경 최대 시간")] private float maxStateChangeTime = 4.0f;
    [SerializeField] private float stateChangeTime;

    // 컴포넌트
    private Rigidbody2D rigid;
    private BoxCollider2D monsterCol;
    private Animator anim;

    [SerializeField] private MonsterState monsterState = MonsterState.Idle;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        stateChangeTime = 0.5f;
    }

    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        monsterCol = transform.GetComponentInChildren<BoxCollider2D>();
        trsPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MonsterStateCheck());
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(monsterState != MonsterState.Die && collision.tag == "PlayerAttack")
    //    {
    //        monsterState = MonsterState.Hit;
    //        float dir = transform.position.x - collision.transform.position.x;

    //        if(dir > 0)
    //        {
    //            rigid.AddForce(Vector2.right * hitImpulse, ForceMode2D.Force);
    //        }
    //        else if(dir < 0)
    //        {
    //            rigid.AddForce(Vector2.left * hitImpulse, ForceMode2D.Impulse);
    //        }
    //    }
    //}

    void Update()
    {
        MonsterAction();
    }

    IEnumerator MonsterStateCheck()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.1f);

            if (monsterState != MonsterState.Trace && monsterState != MonsterState.Attack)
            {
                if(stateChangeTime <= 0)
                {
                    stateChangeTime = Random.Range(minStateChangeTime, maxStateChangeTime);
                    int nextState = Random.Range(0, 2);

                    if(nextState == 0)
                    {
                        monsterState = MonsterState.Idle;
                        anim.SetBool("Move", false);
                    }
                    else
                    {
                        do
                        {
                            monsterDir = Random.Range(-1, 2);
                        } while (monsterDir == 0);

                        monsterState = MonsterState.Move;
                        anim.SetBool("Move", true);
                    }
                }
                else
                {
                    stateChangeTime -= Time.deltaTime;
                }
            }
        }
    }

    private void MonsterAction()
    {
        switch(monsterState)
        {
            case MonsterState.Idle:
                break;

            case MonsterState.Move:
                MonsterMove();
                break;

            case MonsterState.Trace:
                break;
            
        }
    }

    private void MonsterMove()
    {
        float x = moveSpeed * monsterDir;
        rigid.velocity = new Vector2(x, rigid.velocity.y);
        MonsterTurn();
        MoveCheck();
    }

    private void MonsterTurn()
    {
        if(monsterDir == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void MoveCheck()
    {
        RaycastHit2D wallHit = Physics2D.Raycast(monsterCol.bounds.center, Vector2.right * monsterDir,
            (monsterCol.bounds.size.x / 2 + 0.2f), LayerMask.GetMask("Ground"));
        Debug.DrawRay(monsterCol.bounds.center, Vector2.right * monsterDir, Color.red);

        Vector2 v = monsterCol.bounds.center;
        v.x = monsterCol.bounds.center.x + (monsterCol.bounds.size.x / 2 + 0.1f) * monsterDir;
        RaycastHit2D groundHit = Physics2D.Raycast(v, Vector2.down, monsterCol.bounds.size.y / 2 - 0.1f,
            LayerMask.GetMask("Ground"));
        Debug.DrawRay(v, Vector2.down, Color.red);

        if(wallHit.transform != null || groundHit.transform != null)
        {
            if (monsterState == MonsterState.Trace)
            {
                monsterState = MonsterState.Idle;
            }
            else if (monsterState == MonsterState.Move)
            {
                monsterDir *= -1;
            }
        }
    }

    public void MonsterHit(float _damage)
    {
        Debug.Log("Ouch");
        anim.SetTrigger("Hit");

    }
}
