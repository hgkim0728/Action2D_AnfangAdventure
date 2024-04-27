using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField, Tooltip("아이템 이름")] internal string itemName;
    public string ItemName
    {
        get { return name; }
    }
}
