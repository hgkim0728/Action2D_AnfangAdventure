using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트
    BoxCollider2D boxCollider;  // 박스컬라이더2D 컴포넌트
    Transform playerTrs;    // 플레이어의 위치를 담을 변수
    Item itemSO;    // 아이템 스크립터블 오브젝트
    public Item ItemSO
    {
        get { return itemSO; }
        set { itemSO = value; }
    }
    [SerializeField] float moveSpeed;
    int itemIdx;    // 아이템 매니저에서 관리하기 위한 번호
    public int ItemIdx
    {
        get { return itemIdx; }
        set { itemIdx = value; }
    }
    bool usePrefab = false; // 현재 아이템 정보를 담고 게임 내에서 사용중인 프리팹인지 여부
    public bool UsePrefab
    {
        get { return usePrefab; }
        set { usePrefab = value; }
    }
    bool pickedUp = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트의 태그가 플레이어라면
        if(collision.transform.CompareTag("Player"))
        {
            boxCollider.isTrigger = true;   // 아이템이 플레이어한테 밀려나가는 거 방지
            playerTrs = collision.transform;    // 플레이어 위치 가져오기
            // 플레이어를 향해 이동
            transform.position = Vector2.MoveTowards(transform.position, playerTrs.position, moveSpeed);
            pickedUp = true;    // 주워진 상태로 변경
        }
    }

    private void Update()
    {
        if (pickedUp == true)
        {
            if (transform.position != playerTrs.position)
            {
                
            }
            else
            {
                // 아이템 획득 관련 처리를 먼저 한 다음에
                CleanItemSO();
            }
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
        pickedUp = false;
        spriteRenderer.sprite = null;
        boxCollider.isTrigger = false;
    }

    public void DropItem(Vector2 _point)
    {
        gameObject.SetActive(true);
        transform.position = _point;
        boxCollider.isTrigger = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2, ForceMode2D.Impulse);
    }
}
