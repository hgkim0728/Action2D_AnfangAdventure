using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAction : Monster
{
    //void Awake()
    //{
    //    base.Awake();
    //}

    void Start()
    {
        base.Start();
        anim = transform.GetComponentInChildren<Animator>();
        monsterCol = transform.GetComponentInChildren<BoxCollider2D>();
    }

    //void Update()
    //{

    //}

    internal override void MonsterAttack()
    {
        isAttack = true;    // 몬스터의 상태를 재장전(더 좋은 표현을 찾기 전까지는 이렇게 부르기로) 상태로
        anim.SetTrigger("Attack");  // 공격 애니메이션 재생
        anim.SetBool("Move", false);
    }
}
