using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트
    BoxCollider2D boxCollider;
    Item itemSO;    // 아이템 스크립터블 오브젝트
    public Item ItemSO
    {
        get { return itemSO; }
        set { itemSO = value; }
    }
    int itemIdx;    // 아이템 매니저에서 관리하기 위한 번호
    public int ItemIdx
    {
        get { return itemIdx; }
        set { itemIdx = value; }
    }
    bool usePrefab = false; // 현재 아이템 정보를 담고 게임 내에서 사용중인 프리팹인지 여부

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            boxCollider.isTrigger = true;
        }
    }

    /// <summary>
    /// 프리팹 안에 아이템 정보를 넣어주는 함수
    /// </summary>
    /// <param name="_itemSO">프리팹 안에 들어갈 아이템 정보</param>
    public void FillItemSO(Item _itemSO)
    {
        itemSO = _itemSO;
        usePrefab = true;   // 현재 아이템으로서 사용중이라는 것을 알려준다
        spriteRenderer.sprite = itemSO.ItemSprite;  // 스프라이트 변경
    }

    /// <summary>
    /// 프리팹 안을 비우는 함수
    /// </summary>
    public void CleanItemSO()
    {
        itemSO = null;
        usePrefab = false;
        spriteRenderer.sprite = null;
    }
}
