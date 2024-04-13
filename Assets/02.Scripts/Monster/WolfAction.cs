using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAction : Monster
{
    void Awake()
    {
        base.Awake();
        anim = transform.GetComponentInChildren<Animator>();
        monsterCol = transform.GetComponentInChildren<BoxCollider2D>();
    }
}
