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
    private int monsterDir;

    // 전투
    [Space]
    [SerializeField, Tooltip("플레이어 위치")] private Transform trsPlayer;
    [SerializeField, Tooltip("몬스터 공격 범위")] private float monsterAttackRange = 1f;
    [SerializeField, Tooltip("몬스터 체력")] private int monsterHp = 2;
    [SerializeField, Tooltip("몬스터 공격력")] private int monsterAtk = 1;
    [SerializeField, Tooltip("공격 쿨타임")] private float attackCoolTime = 1.0f;
    [SerializeField, Tooltip("피격당했을 때 날아갈 거리")] private float hitImpulse = 3.0f;

    // 상태 변경 시간
    [Space]
    [SerializeField, Tooltip("상태 변경 최소 시간")] private float minStateChangeTime = 2.0f;
    [SerializeField, Tooltip("상태 변경 최대 시간")] private float maxStateChangeTime = 4.0f;
    private float stateChangeTime;

    // 컴포넌트
    private Rigidbody2D rigid;
    private BoxCollider2D monsterCol;
    private Animator anim;

    private MonsterState monsterState = MonsterState.Idle;

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
    }

    private void MonsterStateCheck()
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

        if(stateChangeTime <= 0 && monsterState != MonsterState.Trace)
        {
            stateChangeTime = Random.Range(minStateChangeTime, maxStateChangeTime);
        }
        else
        {
            stateChangeTime -= Time.deltaTime;
        }
    }

    private void MonsterMove()
    {
        rigid.velocity = moveSpeed * Vector2.right * monsterDir;
        MonsterTurn();
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

    public void MonsterHit(float _damage)
    {
        Debug.Log("Ouch");
        anim.SetTrigger("Hit");

    }
}
