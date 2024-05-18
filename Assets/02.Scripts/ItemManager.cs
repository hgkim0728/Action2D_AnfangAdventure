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

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<Item> listItemSo = new List<Item>();
    [SerializeField] private List<GameObject> listItemPrefabs = new List<GameObject>();
    [SerializeField] private List<PlayerOwnItem> listOwnItem = new List<PlayerOwnItem>();
    [SerializeField] private int fillPrefabsCount = 10;
    private int kind;

    void Start()
    {
        kind = listItemSo.Count;
        FillPrefab();
    }

    void Update()
    {
        
    }

    private void FillPrefab()
    {
        for(int i = 0; i < fillPrefabsCount; i++)
        {
            Random.Range(0, kind);
            GameObject go = GameObject.Instantiate(itemPrefab);
            listItemPrefabs.Add(go);
            ItemPrefab sc = go.GetComponent<ItemPrefab>();
            sc.ItemIdx = i;


        }
    }

    public void PickUpItem(Item _itemSo)
    {
        bool ownItem = false;
        string pickUpItemName = _itemSo.ItemName;

        foreach(PlayerOwnItem p in listOwnItem)
        {
            if(pickUpItemName == p.ItemSc.ItemName)
            {
                ownItem = true;
                p.Count++;
            }
        }

        if(ownItem == false)
        {
            PlayerOwnItem newItem = new PlayerOwnItem(_itemSo);
            listOwnItem.Add(newItem);
        }
    }
}
