using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    int hitCount = 0;   // 플레이어 공격 작동 테스트용. 끝나면 꼭 지울 것

    enum MonsterState
    {
        Idle,
        Move,
        Trace,
        Die
    }

    // 몬스터 이동
    [SerializeField, Tooltip("몬스터 이동속도")] private float moveSpeed = 5.0f;
    private int monsterDir = 1;

    // 전투
    [Space]
    [SerializeField, Tooltip("플레이어 위치")] private Transform trsPlayer;
    [SerializeField, Tooltip("플레이어를 추격하는 범위")] private float traceRange = 5.0f;

    [SerializeField, Tooltip("몬스터 공격 범위")] private float monsterAttackRange = 1f;
    [SerializeField, Tooltip("공격 쿨타임")] private float attackCoolTime = 1.0f;
    private float coolTime;
    [SerializeField, Tooltip("몬스터 공격력")] private int monsterAtk = 1;
    [SerializeField, Tooltip("공격을 한 상태인지 아닌지")] private bool isAttack = false;

    [SerializeField, Tooltip("몬스터 체력")] private int monsterHp = 2;
    [SerializeField, Tooltip("피격당했을 때 날아갈 거리")] private float hitImpulse = 3.0f;
    [SerializeField, Tooltip("몬스터가 피격당한 상태인지 아닌지")] private bool isHit = false;
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
        stateChangeTime = 0.1f;
        coolTime = attackCoolTime;
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

    /// <summary>
    /// 몬스터 상태 체크
    /// </summary>
    /// <returns></returns>
    IEnumerator MonsterStateCheck()
    {
        // 몬스터가 살아있다면
        while (!isDie)
        {
            yield return new WaitForSeconds(0.1f);

            // 몬스터가 추적 상태가 아니고 피격 상태도 아니라면
            if (monsterState != MonsterState.Trace && isHit == false)
            {
                // 상태 변경 시간이 되었다면
                if(stateChangeTime <= 0)
                {
                    // 랜덤으로 상태 변경 시간을 재설정
                    stateChangeTime = Random.Range(minStateChangeTime, maxStateChangeTime);
                    int nextState = Random.Range(0, 2); // 다음 상태를 랜덤으로 고르고

                    // 0이라면 대기 상태
                    if(nextState == 0)
                    {
                        monsterState = MonsterState.Idle;   // 몬스터 상태를 대기 상태로
                        rigid.velocity = Vector2.zero;  // 이동하지 않도록 정지
                        anim.SetBool("Move", false);    // 애니메이션도 대기 상태로
                    }
                    else// 1이라면 이동 상태
                    {
                        do
                        {
                            monsterDir = Random.Range(-1, 2);   // 이동 방향 랜덤 지정
                        } while (monsterDir == 0);  // -1 아니면 1만 되도록

                        monsterState = MonsterState.Move;   // 몬스터 상태를 이동으로
                        anim.SetBool("Move", true);     // 애니메이션도 이동 상태로
                    }
                }
                else// 상태 변경 시간이 되지 않았다면
                {
                    stateChangeTime -= Time.deltaTime;  // 상태 변경 시간 감소
                }
            }
            else if(isHit == true)  // 피격 상태라면
            {
                // 현재 재생중인 애니메이션이 피격 애니메이션이고 애니메이션의 재생이 끝났다면
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("Slime_Hit_Animation") == true &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    isHit = false;
                    anim.SetTrigger("Trace");
                    monsterState = MonsterState.Trace;
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
                Trace();
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
        RaycastHit2D groundHit = Physics2D.Raycast(v, Vector2.down, monsterCol.bounds.size.y / 2 + 0.1f,
            LayerMask.GetMask("Ground"));
        Debug.DrawRay(v, Vector2.down, Color.red);

        if(wallHit.transform != null || groundHit.transform == null)
        {
            if (monsterState == MonsterState.Trace)
            {
                rigid.velocity = Vector2.zero;
                anim.SetBool("Move", false);
            }
            else if (monsterState == MonsterState.Move)
            {
                monsterDir *= -1;
            }
        }
    }

    private void Trace()
    {
        float distance = Vector2.Distance(transform.position, trsPlayer.position);

        if (distance < traceRange)
        {
            if (distance > monsterAttackRange)
            {
                coolTime = attackCoolTime;
                isAttack = false;

                if (transform.position.x - trsPlayer.position.x < 0)
                {
                    monsterDir = 1;
                }
                else
                {
                    monsterDir = -1;
                }

                anim.SetBool("Move", true);
                MonsterMove();
            }
            else if (isAttack == false)
            {
                isAttack = true;
                anim.SetTrigger("Attack");
            }

            if (isAttack == true)
            {
                if (coolTime > 0)
                {
                    coolTime -= Time.deltaTime;
                }
                else
                {
                    coolTime = attackCoolTime;
                    isAttack = false;
                }
            }
        }
        else
        {
            coolTime = attackCoolTime;
            isAttack = false;
            rigid.velocity = Vector2.zero;
            monsterState = MonsterState.Idle;
        }
    }

    public void MonsterHit(int _damage)
    {
        hitCount++;
        Debug.Log("Ouch " + hitCount);
        monsterHp -= _damage;
        isHit = true;
        rigid.velocity = Vector2.zero;
        anim.SetTrigger("Hit");
        monsterState = MonsterState.Idle;
    }
}
