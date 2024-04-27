using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField, Tooltip("������ �̸�")] internal string itemName;
    public string ItemName
    {
        get { return name; }
    }

    [SerializeField, Tooltip("������ ��������Ʈ")] internal Sprite itemSprite;
    public Sprite ItemSprite
    {
        get { return itemSprite; }
    }
}
