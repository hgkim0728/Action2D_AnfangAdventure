using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    [SerializeField, Tooltip("���� �� �߰��Ǵ� ���ݷ�")] protected int equipAtk;
    public int EquipAtk { get { return equipAtk; } }
}
