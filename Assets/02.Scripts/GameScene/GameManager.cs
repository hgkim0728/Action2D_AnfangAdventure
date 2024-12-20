﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region 변수

    [Header("플레이어")]
    [SerializeField, Tooltip("플레이어 캐릭터 오브젝트")] PlayerMove player;
    private int maxHp;
    public int MaxHp {  get { return maxHp; } }
    [SerializeField, Tooltip("플레이어 캐릭터 체력")] private int playerHp = 10;
    public int PlayerHp
    {
        get { return playerHp; }
        set { playerHp = value; }
    }

    // 게임
    [Space]
    [SerializeField, Tooltip("아이템 매니저 스크립트")] private ItemManager itemManager;
    private bool gameClear = false;
    public bool GameClear
    {
        set
        { 
            gameClear = value;

            if (value == true)
            {
                gameClearPanel.SetActive(true);
            }
        }
    }
    private string saveKey = "SavePlayerData";

    [Space]
    [Header("UI")]
    [SerializeField, Tooltip("게임오버 패널")] GameObject gameoverPanel;
    [SerializeField, Tooltip("인벤토리 패널")] GameObject inventoryPanel;
    [SerializeField, Tooltip("게임클리어 패널")] GameObject gameClearPanel;
    [SerializeField, Tooltip("플레이어 체력바")] Slider hpSlider;
    [SerializeField, Tooltip("플레이어 체력바 텍스트")] TMP_Text hpText;
    [SerializeField, Tooltip("능력치 창의 플레이어 체력 텍스트")] TMP_Text hpAbiltyText;
    [SerializeField, Tooltip("플레이어 공격력 텍스트")] TMP_Text atkText;

    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        maxHp = playerHp;
        hpSlider.maxValue = maxHp;  // 체력바의 최대치를 플레이어 캐릭터의 최대 체력으로
        hpSlider.value = playerHp;  // 체력바의 값을 현재 플레이어의 체력으로
    }

    void Start()
    {
        if(PlayerPrefs.HasKey(saveKey) == false)
        {
            itemManager.ClearItemData();
        }
    }

    void Update()
    {
        SetHPBar();
        InventoryKey();
        PressEnter();
    }

    /// <summary>
    /// 체력바 안에 들어갈 텍스트의 내용을 반환함
    /// </summary>
    /// <returns></returns>
    private string HPTextContent()
    {
        return $"{playerHp}/{maxHp}";
    }

    /// <summary>
    /// 현재 플레이어의 체력에 따라 체력바의 값을 조정
    /// </summary>
    private void SetHPBar()
    {
        hpSlider.value = playerHp;
        hpText.text = HPTextContent();

        if(inventoryPanel.activeSelf == true)
        {
            // 능력치 창에 현재 플레이어의 체력과 공격력 표시
            hpAbiltyText.text = HPTextContent();
            atkText.text = player.PlayerAtk.ToString();
        }
    }

    /// <summary>
    /// 인벤토리를 열고 닫는 함수
    /// </summary>
    private void InventoryKey()
    {
        // I키를 누르면
        if(Input.GetKeyDown(KeyCode.I))
        {
            // 인벤토리가 닫혀있는 경우
            if(inventoryPanel.activeSelf == false)
            {
                inventoryPanel.SetActive(true); // 인벤토리 창 활성화
                Time.timeScale = 0f;    // 게임 내 시간 정지
            }
            else // 인벤토리가 열려있는 경우
            {
                inventoryPanel.SetActive(false);    // 인벤토리 창 비활성화
                Time.timeScale = 1f;    // 게임 내 시간 재생
            }
        }
    }

    private void PressEnter()
    {
        if(gameClear == true && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GameOver()
    {
        gameoverPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickExitBtn()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
