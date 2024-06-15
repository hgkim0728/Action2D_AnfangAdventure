using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    private int slotIdx;    // 슬롯 인덱스
    public int SlotIdx
    {
        get { return slotIdx; }
        set { slotIdx = value; }
    }
    [SerializeField, Tooltip("자식오브젝트로 가지고 있는 아이템 이미지 표시용 게임오브젝트")] private ItemImg itemImg;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            ItemImg ev = eventData.pointerDrag.transform.GetComponent<ItemImg>();
            Item i = ev.ItemInfo;

            if (itemImg.ItemInfo != null)
            {
                ev.InsertItemInfo(itemImg.ItemInfo);
                itemImg.InsertItemInfo(i);
            }
            else
            {
                ev.ClearImg();
                itemImg.InsertItemInfo(i);
            }

            eventData.pointerDrag.transform.SetParent(this.transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;

            itemImg.gameObject.transform.SetParent(ev.PreParent);
            itemImg.gameObject.transform.localPosition = Vector3.zero;

            itemImg = ev;
        }
    }
}
