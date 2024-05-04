using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    [SerializeField, Tooltip("장착 시 추가되는 공격력")] protected int equipAtk;
    public int EquipAtk { get { return equipAtk; } }
}
