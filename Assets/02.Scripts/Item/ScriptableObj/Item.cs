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
    [SerializeField, Tooltip("아이템 인덱스")] protected int itemIdx = 0;
    public int ItemIdx { get { return itemIdx; } }
    [SerializeField, Tooltip("현재 플레이어가 소지한 개수")] protected int itemCount = 0;
    public int ItemCount {  get {  return itemCount; } }

    public void GainItem()
    {
        itemCount++;
    }

    public void UseItem()
    {
        itemCount--;
    }
}
