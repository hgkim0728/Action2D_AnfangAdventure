using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트
    private BoxCollider2D boxCollider;  // 박스컬라이더2D 컴포넌트
    private Transform playerTrs;    // 플레이어의 위치를 담을 변수
    public ItemManager itemManager;     // 아이템 매니저 스크립트
    private Item itemSO;    // 아이템 스크립터블 오브젝트
    public Item ItemSO
    {
        get { return itemSO; }
        set { itemSO = value; }
    }
    [SerializeField, Tooltip("플레이어가 주운 아이템이 플레이어를 향해 이동하는 속도")] float moveSpeed;
    private int itemPrefabIdx;    // 아이템 매니저에서 관리하기 위한 번호
    public int ItemPrefabIdx
    {
        get { return itemPrefabIdx; }
        set { itemPrefabIdx = value; }
    }
    private bool usePrefab = false; // 현재 아이템 정보를 담고 게임 내에서 사용중인 프리팹인지 여부
    public bool UsePrefab
    {
        get { return usePrefab; }
        set { usePrefab = value; }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// 프리팹 안에 아이템 정보를 넣어주는 함수
    /// </summary>
    /// <param name="_itemSO">프리팹 안에 들어갈 아이템 정보</param>
    public void InsertItemInfo(Item _itemSO)
    {
        itemSO = _itemSO;
        usePrefab = true;   // 현재 아이템으로서 사용중이라는 것을 알려준다
        spriteRenderer.sprite = itemSO.ItemSprite;  // 스프라이트 변경
    }

    /// <summary>
    /// 프리팹 안을 비우는 함수
    /// </summary>
    public void ClearItemInfo()
    {
        itemSO = null;
        usePrefab = false;
        //pickedUp = false;
        spriteRenderer.sprite = null;
        boxCollider.isTrigger = false;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 플레이어가 처치한 몬스터가 아이템을 드랍할 때 호출
    /// </summary>
    /// <param name="_point">아이템을 드랍할 위치</param>
    public void DropItem(Vector2 _point)
    {
        gameObject.SetActive(true);
        transform.position = _point;
        boxCollider.isTrigger = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 플레이어가 아이템을 주웠을 때
    /// </summary>
    public void PickedItem(Transform _playerTrs)
    {
        boxCollider.isTrigger = true;   // 아이템이 플레이어한테 밀려나가는 거 방지
        playerTrs = _playerTrs;    // 플레이어 위치 가져오기
        // 플레이어를 향해 이동
        transform.position = Vector2.MoveTowards(transform.position, playerTrs.position, moveSpeed);
        itemManager.FillSlot(itemSO);
        ClearItemInfo();
    }
}
