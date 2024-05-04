using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemImg : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Image itemImg;  // 슬롯 안에 있는 아이템의 스프라이트
    private TMP_Text itemCountTxt;

    private void Awake()
    {
        itemImg = GetComponent<Image>();
    }

    void Start()
    {
        itemCountTxt = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void InsertItemImg(Sprite _sprite)
    {
        itemImg.sprite = _sprite;
        itemImg.color = Color.white;
    }

    public void EmptyImg()
    {
        itemImg.sprite = null;
        itemImg.color = Color.clear;
        itemCountTxt.text = null;
    }
}
