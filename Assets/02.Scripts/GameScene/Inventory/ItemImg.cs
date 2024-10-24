using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemImg : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField, Tooltip("실험용, 정상 작동 확인하면 컴포넌트에서는 안 보이게")]Item itemInfo;  // 슬롯 안에 있는 아이템의 정보
    public Item ItemInfo
    {
        get { return itemInfo; }
    }
    int slotIdx = 0;    // 리스트에 들어간 순서
    public int SlotIdx
    {
        get { return slotIdx; }
        set { slotIdx = value; }
    }
    [SerializeField] private Image itemImg;  // 슬롯 안에 있는 아이템의 스프라이트
    [SerializeField] private CanvasGroup canvasGroup;    // 캔버스 그룹 컴포넌트
    [SerializeField] private RectTransform rect;     // 렉트트랜스폼 컴포넌트
    [SerializeField] private TMP_Text itemCountTxt;  // 소지한 아이템 개수를 표시할 텍스트
    [SerializeField]private Canvas canvas;  // 캔버스
    private Transform preParent;    // 원래 아이템이 있던 슬롯을 저장할 변수
    public Transform PreParent { get { return preParent; } }

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    /// <summary>
    /// 슬롯 이미지 비우는 함수
    /// </summary>
    private void ClearImg()
    {
        if (transform.parent == canvas.transform)
        {
            preParent.GetComponent<Slot>().fill = false;
        }
        else
        {
            transform.parent.GetComponent<Slot>().fill = false;
        }

        itemImg.sprite = null;
        itemImg.color = Color.clear;
        itemCountTxt.text = null;
        itemInfo = null;
    }

    /// <summary>
    /// 플레이어가 획득한 아이템이 인벤토리 슬롯에 표시되도록 아이템 정보를 집어넣는 기능
    /// </summary>
    /// <param name="_item">슬롯에 들어갈 아이템</param>
    public void InsertItemInfo(Item _item)
    {
        itemInfo = _item;
        itemImg.sprite = itemInfo.ItemSprite;
        itemImg.color = Color.white;

        SetItemCountTxt();
    }

    /// <summary>
    /// 소지한 아이템의 개수를 표시하는 텍스트의 내용을 수정하는 기능
    /// </summary>
    public void SetItemCountTxt()
    {
        int count = itemInfo.ItemCount;

        if(count == 0)
        {
            ClearImg();
        }
        else if(count > 1)
        {
            itemCountTxt.text = count.ToString();
        }
        else if(count == 1)
        {
            itemCountTxt.text = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(itemInfo == null) return;

        ItemType itemType = itemInfo.UseItem();
        
        if(itemType == ItemType.Recovery || itemType == ItemType.Enforce)
        {
            if (itemInfo.ItemCount <= 0)
            {
                ClearImg();
            }
        }

        SetItemCountTxt();

        switch(itemType)
        {
            case ItemType.Recovery:
                RecoveryPlayer();
                break;
        }
    }

    private void RecoveryPlayer()
    {
        GameManager.instance.PlayerHp++;
    }

    // 인벤토리의 아이템 이미지 드래그를 시작할 때
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 이 게임오브젝트를 자식으로 가지고 있는
        // 슬롯이 비어있을 경우에는 return
        if (itemInfo == null) return;

        preParent = this.transform.parent;   // 원래 부모 오브젝트인 슬롯을 저장
        transform.SetParent(canvas.transform);  // 부모 오브젝트를 캔버스로
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();   // 드래그중인 이미지가 맨앞으로 나오도록
    }

    // 인벤토리의 아이템 이미지를 드래그중일 때
    public void OnDrag(PointerEventData eventData)
    {
        if (itemInfo == null) return;

        // 드래그중인 마우스를 따라서 이미지가 움직이도록
        Vector3 curPos = Camera.main.ScreenToWorldPoint(eventData.position);
        curPos.z = 0;
        rect.transform.position = curPos;
    }

    // 인벤토리의 아이템 이미지 드래그를 끝냈을 때
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝났을 때 이미지의 부모 오브젝트가 캔버스라면
        if(transform.parent == canvas.transform)
        {
            transform.SetParent(preParent); // 이미지의 부모 오브젝트를 원래 부모였던 슬롯으로
            rect.localPosition = Vector2.zero;  // 드래그 전 위치로 돌아가도록
        }

        canvasGroup.blocksRaycasts = true;
    }
}
