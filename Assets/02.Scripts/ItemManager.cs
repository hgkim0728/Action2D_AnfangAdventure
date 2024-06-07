using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    class PlayerOwnItem
    {
        private int count = 1;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private Item itemSc;
        public Item ItemSc
        { get { return itemSc; } }

        public PlayerOwnItem(Item _itemSc)
        {
            itemSc = _itemSc;
        }
    }

    [SerializeField, Tooltip("아이템 프리팹")] private GameObject itemPrefab;
    [SerializeField, Tooltip("인벤토리 슬롯 프리팹")] private GameObject slotPrefab;

    [SerializeField, Tooltip("인벤토리 슬롯이 들어갈 위치")] private Transform inventoryContent;

    [SerializeField] private List<Item> listItemSo = new List<Item>();
    [SerializeField] private List<GameObject> listItemPrefabs = new List<GameObject>();
    [SerializeField, Tooltip("인벤토리 슬롯 리스트")] private List<GameObject> listInventorySlot = new List<GameObject>();
    [SerializeField, Tooltip("한 번에 생성할 아이템 프리팹의 수")] private int fillPrefabsCount = 10;
    private int itemType;
    [SerializeField, Tooltip("인벤토리 슬롯 개수")] private int inventorySlotCount = 20;

    void Start()
    {
        itemType = listItemSo.Count;
        FillPrefab();
        CreateInventorySlot();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 아이템 프리팹 오브젝트 생성
    /// </summary>
    private void FillPrefab()
    {
        for(int i = 0; i < fillPrefabsCount; i++)
        {
            Random.Range(0, itemType);
            GameObject go = GameObject.Instantiate(itemPrefab);
            listItemPrefabs.Add(go);
            ItemPrefab sc = go.GetComponent<ItemPrefab>();
            sc.ItemIdx = i;


        }
    }

    /// <summary>
    /// 인벤토리 슬롯 생성
    /// </summary>
    private void CreateInventorySlot()
    {
        for(int i = 0; i < inventorySlotCount; i++)
        {
            GameObject go = GameObject.Instantiate(slotPrefab, inventoryContent);
            listInventorySlot.Add(go);
            go.GetComponent<Slot>().SlotIdx = i;
        }
    }

    //public void PickUpItem(Item _itemSo)
    //{
    //    bool ownItem = false;
    //    string pickUpItemName = _itemSo.ItemName;

    //    foreach(PlayerOwnItem p in listOwnItem)
    //    {
    //        if(pickUpItemName == p.ItemSc.ItemName)
    //        {
    //            ownItem = true;
    //            p.Count++;
    //        }
    //    }

    //    if(ownItem == false)
    //    {
    //        PlayerOwnItem newItem = new PlayerOwnItem(_itemSo);
    //        listOwnItem.Add(newItem);
    //    }
    //}
}
