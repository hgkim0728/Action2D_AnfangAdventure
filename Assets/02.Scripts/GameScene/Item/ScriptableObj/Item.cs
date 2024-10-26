using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Sword, Bow, Recovery, Enforce}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    // 아이템 기능 관련 수치
    [Serializable]
    public struct Stat
    {
        public string name;
        public int value;
    }

    [Header("아이템 정보")]
    [SerializeField, Tooltip("아이템 타입")] private ItemType type;
    public ItemType Type { get { return type; } }
    [SerializeField, Tooltip("아이템 이름")] private string itemName;
    public string ItemName { get { return name; } }
    [Multiline]
    [SerializeField, Tooltip("아이템 설명")] private string itemDescription;
    public string ItemDescription { get { return itemDescription; } }
    [SerializeField, Tooltip("아이템 스프라이트")] private Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }
    [SerializeField, Tooltip("아이템 인덱스")] private int itemIdx = 0;
    public int ItemIdx { get { return itemIdx; } }
    public List<Stat> Stats = new List<Stat>();

    [Space]
    [SerializeField, Tooltip("현재 플레이어가 소지한 개수")] private int itemCount = 0;

    public int ItemCount
    {  
        get {  return itemCount; }
        set {  itemCount = value; }
    }

    public void GetItem()
    {
        itemCount++;
    }

    public void UseItem()
    {
        if (type == ItemType.Recovery || type == ItemType.Enforce)
        {
            itemCount--;

            if (itemCount < 0)
            {
                itemCount = 0;
            }
        }
    }
}
