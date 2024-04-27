using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField, Tooltip("아이템 이름")] internal string itemName;
    public string ItemName
    {
        get { return name; }
    }

    [SerializeField, Tooltip("아이템 스프라이트")] internal Sprite itemSprite;
    public Sprite ItemSprite
    {
        get { return itemSprite; }
    }
}
