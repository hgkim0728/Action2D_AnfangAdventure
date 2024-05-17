using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트
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
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
