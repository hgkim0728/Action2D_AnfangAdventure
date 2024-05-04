using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    ItemImg itemImg;
    Item item;  // 슬롯 안에 있는 아이템의 정보
    int slotIdx = 0;    // 리스트에 들어간 순서
    public int SlotIdx
    {
        get { return slotIdx; }
        set { slotIdx = value; }
    }
    int itemCount = 0;  // 슬롯 안에 있는 아이템의 수
    public int ItemCount
    {
        set { itemCount = value; }
    }

    private void Start()
    {
        itemImg = GetComponentInChildren<ItemImg>();
    }

    public void InsertItem(Item _item)
    {
        item = _item;
        itemCount++;
        itemImg.InsertItemImg(item.ItemSprite);
    }
}
