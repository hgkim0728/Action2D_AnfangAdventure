using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Sword, Bow, Recovery, Enforce}

public class Item : ScriptableObject
{
    [SerializeField, Tooltip("아이템 타입")] protected ItemType type;
    public ItemType Type { get { return type; } }
    [SerializeField, Tooltip("아이템 이름")] protected string itemName;
    public string ItemName { get { return name; } }
    [SerializeField, Tooltip("아이템 스프라이트")] protected Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }
    public int inventoryIdx = -1;
    public int itemCount = 0;
}
