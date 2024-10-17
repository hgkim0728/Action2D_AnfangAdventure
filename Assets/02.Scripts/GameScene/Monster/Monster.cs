using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected enum MonsterState
    {
        Idle,
        Move,
        Trace,
        Die
    }

    [SerializeField, Tooltip("이 몬스터가 배치된 스테이지의 번호")] protected int stageNum;

    // 몬스터 이동
    [SerializeField, Tooltip("몬스터 이동속도")] protected float moveSpeed = 5.0f;
    protected int monsterDir = 1;

    // 전투
    [Space]
    [SerializeField, Tooltip("플레이어 위치")] protected Transform trsPlayer;
    [SerializeField, Tooltip("플레이어를 추격하는 범위")] protected float traceRange = 5.0f;

    [SerializeField, Tooltip("몬스터 공격 범위")] protected float monsterAttackRange = 1f;
    [SerializeField, Tooltip("공격 쿨타임")] protected float attackCoolTime = 1.0f;
    float coolTime;
    [SerializeField, Tooltip("몬스터 공격력")] protected int monsterAtk = 1;
    [SerializeField, Tooltip("공격을 한 상태인지 아닌지")] protected bool isAttack = false;

    [SerializeField, Tooltip("몬스터 체력")] protected int monsterHp = 2;
    [SerializeField, Tooltip("피격당했을 때 날아갈 거리")] protected float hitImpulse = 3.0f;
    [SerializeField, Tooltip("몬스터가 피격당한 상태인지 아닌지")] protected bool isHit = false;
    [SerializeField, Tooltip("몬스터가 죽었는지 아닌지")] protected bool isDie = false;

    // 상태 변경 시간
    [Space]
    [SerializeField, Tooltip("상태 변경 최소 시간")] protected float minStateChangeTime = 2.0f;
    [SerializeField, Tooltip("상태 변경 최대 시간")] protected float maxStateChangeTime = 4.0f;
    [SerializeField] protected float stateChangeTime;

    // 컴포넌트
    protected Rigidbody2D rigid;
    protected BoxCollider2D monsterCol;
    protected Animator anim;

    // 아이템 매니저
    ItemManager itemManager;

    [SerializeField, Tooltip("몬스터 현재 상태")] MonsterState monsterState = MonsterState.Idle;

    public int MonsterAtk
    {
        get { return monsterAtk; }
    }

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        stateChangeTime = 0.5f;
        coolTime = attackCoolTime;
    }

    protected void Start()
    {
        trsPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }

    protected void SetMonsterObj()
    {
        anim = transform.GetComponentInChildren<Animator>();
        monsterCol = transform.GetComponentInChildren<BoxCollider2D>();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(monsterState != MonsterState.Trace && collision.gameObject.tag == "Player")
        {
            monsterDir *= -1;
        }
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
        MonsterStateCheck();
        MonsterAction();
    }

    /// <summary>
    /// 몬스터 상태 체크
    /// </summary>
    /// <returns></returns>
    protected virtual void MonsterStateCheck()
    {
        // 몬스터가 죽었다면 return
        if (isDie) return;

        // 몬스터가 추격 상태가 아니고 피격당하지 않았다면
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
            // 몬스터가 플레이어에게 피격 당해밀려날 때 공중에 머무는 시간이 길 경우 레이캐스트를 추가할 필요가 있을 듯

            // 현재 재생중인 애니메이션이 피격 애니메이션이고 애니메이션의 재생이 끝났다면
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Slime_Hit_Animation") == true &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isHit = false;  // 피격 상태 해제
                anim.SetTrigger("Trace");   // 피격 애니메이션에서 대기 애니메이션으로 넘어가게 하기
                monsterState = MonsterState.Trace;  // 몬스터 상태를 추격 상태로
            }
        }
    }

    /// <summary>
    /// 몬스터 상태에 따라 행동하게 하는 함수
    /// </summary>
    protected virtual void MonsterAction()
    {
        switch(monsterState)
        {
            // 몬스터가 대기 상태일 경우
            // 딱히 하는 거 없음
            case MonsterState.Idle:
                break;

            // 몬스터가 이동 상태일 경우
            case MonsterState.Move:
                MonsterMove();  // 이동 함수 작동
                break;

            // 몬스터가 추격 상태일 경우
            case MonsterState.Trace:
                Trace();    // 추격 함수 작동
                break;
            
        }
    }

    /// <summary>
    /// 몬스터의 이동을 담당하는 함수
    /// </summary>
    protected virtual void MonsterMove()
    {
        // 몬스터의 속도와 방향대로 이동
        float x = moveSpeed * monsterDir;
        rigid.velocity = new Vector2(x, rigid.velocity.y);
        MonsterTurn();
        MoveCheck();
    }

    /// <summary>
    /// 몬스터의 이동 방향에 따라 스프라이트의 방향 변경
    /// </summary>
    protected virtual void MonsterTurn()
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

    /// <summary>
    /// 몬스터가 이동중일 때 몬스터의 앞에 벽이 있는지와
    /// 길이 있는지를 체크
    /// </summary>
    protected virtual void MoveCheck()
    {
        // 몬스터의 앞에 벽이 있는지를 체크
        RaycastHit2D wallHit = Physics2D.Raycast(monsterCol.bounds.center, Vector2.right * monsterDir,
            (monsterCol.bounds.size.x / 2 + 0.2f), LayerMask.GetMask("Ground", "Obstacle"));
        Debug.DrawRay(monsterCol.bounds.center, Vector2.right * monsterDir, Color.red);

        // 몬스터의 앞에 길이 있는지를 체크
        Vector2 v = monsterCol.bounds.center;
        v.x = monsterCol.bounds.center.x + (monsterCol.bounds.size.x / 2 + 0.1f) * monsterDir;
        RaycastHit2D groundHit = Physics2D.Raycast(v, Vector2.down, monsterCol.bounds.size.y / 2 + 0.1f,
            LayerMask.GetMask("Ground"));
        Debug.DrawRay(v, Vector2.down, Color.red);

        // 몬스터의 앞에 벽이 있거나 길이 없다면
        if(wallHit.transform != null || groundHit.transform == null)
        {
            // 몬스터 상태가 추격 상태일 경우
            if (monsterState == MonsterState.Trace)
            {
                rigid.velocity = Vector2.zero;  // 정지
                anim.SetBool("Move", false);    // 대기 애니메이션 재생
            }
            else if (monsterState == MonsterState.Move)// 몬스터 상태가 이동 상태일 경우
            {
                monsterDir *= -1;   // 이동 방향을 반대 방향으로 변경
            }
        }
    }

    /// <summary>
    /// 몬스터가 추격 상태일 경우의 행동
    /// </summary>
    protected virtual void Trace()
    {
        // 몬스터와 플레이어의 거리
        float distance = Vector2.Distance(transform.position, trsPlayer.position);

        // 추격 범위 안에 플레이어가 있을 경우
        if (distance < traceRange)
        {
            // 몬스터의 공격 범위 안에 플레이어가 없을 경우
            if (distance > monsterAttackRange)
            {
                // 공격 쿨타임 초기화
                coolTime = attackCoolTime;
                isAttack = false;

                // 플레이어가 어느 방향에 있는지를 체크
                if (transform.position.x - trsPlayer.position.x < 0)
                {
                    monsterDir = 1;
                }
                else
                {
                    monsterDir = -1;
                }

                anim.SetBool("Move", true); // 이동 애니메이션 재생
                MonsterMove();  // 플레이어가 있는 방향으로 이동
            }
            else if (isAttack == false)// 플레이어가 공격 범위 안에 있고 공격 쿨타임이 지난 상태라면
            {
                MonsterAttack();
            }

            // 재장전 상태라면
            if (isAttack == true)
            {
                // 공격 쿨타임이 지나지 않았다면
                if (coolTime > 0)
                {
                    coolTime -= Time.deltaTime;     // 시간 감소
                }
                else
                {
                    coolTime = attackCoolTime;  // 쿨타임 재설정
                    isAttack = false;   // 재장전 상태 해제
                }
            }
        }
        else// 플레이어가 추격 범위 밖에 있다면
        {
            coolTime = attackCoolTime;  // 공격 쿨타임 재설정
            isAttack = false;
            rigid.velocity = Vector2.zero;  // 추격 정지
            monsterState = MonsterState.Idle;   // 몬스터의 상태를 대기로
            anim.SetBool("Move", false);
        }
    }

    protected virtual void MonsterAttack()
    {
        isAttack = true;    // 몬스터의 상태를 재장전(더 좋은 표현을 찾기 전까지는 이렇게 부르기로) 상태로
        anim.SetTrigger("Attack");  // 공격 애니메이션 재생
        anim.SetBool("Move", false);
    }

    /// <summary>
    /// 몬스터 피격 함수
    /// </summary>
    /// <param name="_damage">몬스터가 받을 데미지</param>
    public virtual void MonsterHit(int _damage)
    {
        monsterHp -= _damage;   // 플레이어의 공격력만큼 몬스터 체력 감소
        isHit = true;   // 피격 상태
        rigid.velocity = Vector2.zero;  // 이동 정지
        anim.SetTrigger("Hit"); // 피격 애니메이션 작동
        monsterState = MonsterState.Idle;   // 몬스터 상태를 대기 상태로
        Invoke("Pushed", 0.15f);
    }

    public void Pushed()
    {
        if (transform.position.x - trsPlayer.position.x < 0)
        {
            rigid.AddForce(new Vector2(-hitImpulse, hitImpulse), ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector2.one * hitImpulse, ForceMode2D.Impulse);
        }

        if(isDie == false && monsterHp <= 0)
        {
            Invoke("MonsterDie", 0.2f);
        }
    }

    void MonsterDie()
    {
        isDie = true;
        anim.SetTrigger("Die");

        // 랜덤으로 드랍할 아이템을 정하게 하고 아이템 매니저한테 아이템 프리팹을 받아오게 할 것
        //itemManager.PickItem(transform.position);
        this.gameObject.SetActive(false);
    }
}
