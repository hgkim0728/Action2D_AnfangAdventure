using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    enum ItemKind { Sword, Bow, Recovery, Enforce}

    [SerializeField, Tooltip("아이템 이름")] protected string itemName;
    public string ItemName
    {
        get { return name; }
    }

    [SerializeField, Tooltip("아이템 스프라이트")] protected Sprite itemSprite;
    public Sprite ItemSprite
    {
        get { return itemSprite; }
    }
}
