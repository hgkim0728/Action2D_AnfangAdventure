using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemImg : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    private Image itemImg;  // 슬롯 안에 있는 아이템의 스프라이트
    private CanvasGroup canvasGroup;    // 캔버스 그룹 컴포넌트
    private RectTransform rect;     // 렉트트랜스폼 컴포넌트
    private TMP_Text itemCountTxt;  // 소지한 아이템 개수를 표시할 텍스트
    [SerializeField]private Canvas canvas;  // 캔버스
    private Transform preParent;    // 원래 아이템이 있던 슬롯을 저장할 변수
    public Transform PreParent { get { return preParent; } }

    private void Awake()
    {
        itemImg = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        itemCountTxt = GetComponentInChildren<TMP_Text>();
        canvas = FindObjectOfType<Canvas>();
    }

    /// <summary>
    /// 슬롯 이미지 비우는 함수
    /// </summary>
    public void ClearImg()
    {
        itemImg.sprite = null;
        itemImg.color = Color.clear;
        itemCountTxt.text = null;
        itemInfo = null;
    }

    public void InsertItem(Item _item)
    {
        itemInfo = _item;
        itemImg.sprite = itemInfo.ItemSprite;
        itemImg.color = Color.white;

        SetItemCount();
    }

    public void SetItemCount()
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemInfo == null) return;

        preParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 curPos = Camera.main.ScreenToWorldPoint(eventData.position);
        curPos.z = 0;
        rect.transform.position = curPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == canvas.transform)
        {
            transform.SetParent(preParent);
            rect.localPosition = Vector2.zero;
        }

        canvasGroup.blocksRaycasts = true;
    }
}
