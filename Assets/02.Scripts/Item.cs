using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField, Tooltip("������ �̸�")] internal string itemName;
    public string ItemName
    {
        get { return name; }
    }
}
