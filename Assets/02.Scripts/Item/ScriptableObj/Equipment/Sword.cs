using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipSword", menuName = "ScriptableObject/Equipment/Sword", order = int.MaxValue)]
public class Sword : Equipment
{
    [SerializeField, Tooltip("공격 범위")] float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
    }
}
