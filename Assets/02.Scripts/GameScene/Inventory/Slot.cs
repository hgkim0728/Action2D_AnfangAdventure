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
    public ItemImg Img
    {
        get { return itemImg; }
    }
    public bool fill = false;   // 슬롯에 아이템이 들어있는지 여부

    // 다른 오브젝트를 슬롯 위로 드래그하고 마우스의 버튼을 놨을 때
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            // 드래그된 아이템 이미지 오브젝트
            ItemImg ev = eventData.pointerDrag.transform.GetComponent<ItemImg>();

            if (ev == null) return;

            Item i = ev.ItemInfo;   // 드래그된 아이템 이미지 오브젝트 안에 들어있던 아이템 정보

            if (i == null) return;

            // 이 슬롯이 비어있다면
            if (fill == false)
            {
                //itemImg.InsertItemInfo(i);
                //ev.ClearImg();  // 드래그된 이미지 오브젝트를 비우는 함수 실행
                ev.PreParent.GetComponent<Slot>().fill = false;
                fill = true;
            }

            ev.PreParent.GetComponent<Slot>().itemImg = this.itemImg;
            // 이 슬롯 오브젝트를 드래그된 아이템 이미지 오브젝트의 부모 오브젝트로 하고 위치를 조정
            eventData.pointerDrag.transform.SetParent(this.transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;

            // 이 슬롯 오브젝트에 있던 아이템 이미지 오브젝트를 드래그된 이미지 오브젝트가 있던 슬롯으로
            this.itemImg.gameObject.transform.SetParent(ev.PreParent);
            this.itemImg.gameObject.transform.localPosition = Vector3.zero;

            this.itemImg = ev;
        }
    }
}
