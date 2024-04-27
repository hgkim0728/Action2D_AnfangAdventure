using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Data", menuName = "Scriptable Object/Equipment Data", order = int.MaxValue)]
public class Equipment : Item
{
    private PlayerMove.PlayerEquip equipKind;
}
