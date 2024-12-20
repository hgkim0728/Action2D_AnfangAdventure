﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAction : Monster
{
    void Awake()
    {
        base.Awake();
        base.SetMonsterObj();
    }

    protected override void MonsterAttack()
    {
        base.MonsterAttack();
        rigid.velocity = Vector2.right * monsterDir * monsterAttackRange;
    }
}
