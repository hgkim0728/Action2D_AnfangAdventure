using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Serializable]
    public struct DropItems
    {
        public Item item;
        public float weight;
    }

    //class PlayerOwnItem
    //{
    //    private int count = 1;
    //    public int Count
    //    {
    //        get { return count; }
    //        set { count = value; }
    //    }
    //    private Item itemSc;
    //    public Item ItemSc
    //    { get { return itemSc; } }

    //    public PlayerOwnItem(Item _itemSc)
    //    {
    //        itemSc = _itemSc;
    //    }
    //}

    [SerializeField, Tooltip("아이템 프리팹")] private GameObject itemPrefab;
    [SerializeField, Tooltip("인벤토리 슬롯 프리팹")] private GameObject slotPrefab;

    [SerializeField, Tooltip("인벤토리 슬롯이 들어갈 위치")] private Transform inventoryContent;

    //[SerializeField] private List<Item> listItemSo = new List<Item>();
    //[SerializeField, Tooltip("인벤토리 스크립터블 오브젝트")] private InventorySO inventorySO;
    public List<DropItems> listDropItems = new List<DropItems>();   // 아이템 드롭 확률 리스트
    [SerializeField, Tooltip("아이템 프리팹 리스트")] private List<GameObject> listItemPrefabs = new List<GameObject>();
    [SerializeField, Tooltip("인벤토리 슬롯 리스트")] private List<GameObject> listInventorySlot = new List<GameObject>();
    [SerializeField, Tooltip("한 번에 생성할 아이템 프리팹의 수")] private int fillPrefabsCount = 10;
    //private int itemTypeCount;
    [SerializeField, Tooltip("인벤토리 슬롯 개수")] private int inventorySlotCount = 20;

    void Start()
    {
        //itemTypeCount = inventorySO.Items.Length;
        CreateItemPrefab();
        CreateInventorySlot();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 아이템 프리팹 오브젝트 생성
    /// </summary>
    private void CreateItemPrefab()
    {
        // 정해둔 개수만큼 생성
        for(int i = 0; i < fillPrefabsCount; i++)
        {
            //Random.Range(0, itemTypeCount);
            GameObject go = GameObject.Instantiate(itemPrefab); // 아이템 프리팹 게임오브젝트 생성
            listItemPrefabs.Add(go);    // 리스트에 추가
            ItemPrefab sc = go.GetComponent<ItemPrefab>();
            sc.ItemPrefabIdx = i;   // 몇번째로 생성된 아이템 프리팹인지 정보 저장


        }
    }

    /// <summary>
    /// 인벤토리 슬롯 생성
    /// </summary>
    private void CreateInventorySlot()
    {
        // 미리 설정해둔 횟수만큼 슬롯 게임오브젝트 생성 반복
        for(int i = 0; i < inventorySlotCount; i++)
        {
            // 슬롯 게임오브젝트를 만들어 인벤토리 콘텐츠의 자식오브젝트로 둠
            GameObject go = GameObject.Instantiate(slotPrefab, inventoryContent);
            listInventorySlot.Add(go);  // 슬롯 리스트에 추가
            go.GetComponent<Slot>().SlotIdx = i;    // 슬롯 스크립트 내의 슬롯 인덱스에 몇 번째 슬롯인지 표시
        }
    }

    /// <summary>
    /// 몬스터가 드랍할 아이템 랜덤 지정
    /// </summary>
    /// <param name="_point">몬스터 위치 정보</param>
    public void PickItem(Vector2 _point)
    {
        //if (listDropItems.Count == 0) return null;

        float sum = 0;
        foreach (DropItems dropItem in listDropItems)
        {
            sum += dropItem.weight;
        }

        float randomValue = UnityEngine.Random.value * sum;

        foreach (DropItems dropItem in listDropItems)
        {
            randomValue -= dropItem.weight;

            if (randomValue <= 0)
            {
                if (dropItem.item != null)
                {
                    PickItemPrefab(dropItem.item, _point);
                }
                break;
                //return dropItem.item;
            }
        }

        //return listDropItems[listDropItems.Count - 1].item;
    }

    public void PickItemPrefab(Item _item, Vector2 _point)
    {
        int itemPreIdx = 0; // 리스트에 있는 아이템 프리팹 체크용

        foreach(GameObject go in listItemPrefabs)
        {
            ItemPrefab itemPrefab = go.GetComponent<ItemPrefab>();   // 몬스터에게 전달할 아이템 프리팹을 넣을 변수
            
            if (itemPrefab.UsePrefab == false)
            {
                itemPrefab.InsertItemInfo(_item);
                itemPrefab.DropItem(_point);
                break;
                //return itemPrefab;
            }

            itemPreIdx++;
        }

        CreateItemPrefab();
        ItemPrefab itemPre = listItemPrefabs[itemPreIdx].GetComponent<ItemPrefab>();
        itemPre.InsertItemInfo(_item);
        itemPre.DropItem(_point);
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
