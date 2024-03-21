using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Hit,
        Die
    }

    [SerializeField, Tooltip("공격 쿨타임")] private float attackCoolTime = 1.0f;
    [SerializeField, Tooltip("피격당했을 때 날아갈 거리")] private float hitImpulse = 3.0f;

    private Rigidbody2D rigid;
    private Animator anim;

    private MonsterState monsterState = MonsterState.Idle;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(monsterState != MonsterState.Die && collision.tag == "PlayerAttack")
        {
            monsterState = MonsterState.Hit;
            float dir = transform.position.x - collision.transform.position.x;

            if(dir > 0)
            {
                rigid.AddForce(Vector2.right * hitImpulse, ForceMode2D.Impulse);
            }
            else if(dir < 0)
            {
                rigid.AddForce(Vector2.left * hitImpulse, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("슬라임 아프다");
    }

    void Update()
    {
        MonsterStateControll();
    }

    private void MonsterStateControll()
    {
        switch(monsterState)
        {
            case MonsterState.Idle:
                break;

            case MonsterState.Move:
                MonsterMove();
                break;

            case MonsterState.Hit:
                anim.SetBool("Hit", true);
                Invoke("OffHitAnimation", 0.1f);
                break;
        }
    }

    private void MonsterMove()
    {

    }

    private void OffHitAnimation()
    {
        anim.SetBool("Hit", false);
        monsterState = MonsterState.Attack;
    }
}
