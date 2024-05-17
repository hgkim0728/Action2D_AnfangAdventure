using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<Item> listItemSo = new List<Item>();
    [SerializeField] private List<GameObject> listItemPrefabs = new List<GameObject>();
    [SerializeField] private int fillPrefabsCount = 10;
    private int kind;

    void Start()
    {
        kind = listItemSo.Count;
        FillPrefab();
    }

    void Update()
    {
        
    }

    private void FillPrefab()
    {
        for(int i = 0; i < fillPrefabsCount; i++)
        {
            Random.Range(0, kind);
            GameObject go = GameObject.Instantiate(itemPrefab);
            listItemPrefabs.Add(go);
            // 스크립터블 오브젝트를 넣어줘야 함
        }
    }
}
