using System.Collections;
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
    private int maxHp = 10;
    [SerializeField, Tooltip("플레이어 캐릭터 체력")] private int playerHp = 10;
    public int PlayerHp
    {
        get { return playerHp; }
        set { playerHp = value; }
    }

    // 게임
    [Space]
    [SerializeField, Tooltip("튜토리얼을 종료했는지")] bool tutorial = false;

    [Space]
    [Header("UI")]
    [Space]
    [SerializeField, Tooltip("게임오버 패널")] GameObject gameoverPanel;
    [SerializeField, Tooltip("인벤토리 패널")] GameObject inventoryPanel;
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

        hpSlider.maxValue = maxHp;
        hpSlider.value = playerHp;
    }

    void Start()
    {
        
    }

    void Update()
    {
        SetHPBar();
        InventorySet();
    }

    private string HPTextContent()
    {
        return $"{playerHp}/{maxHp}";
    }

    private void SetHPBar()
    {
        hpSlider.value = playerHp;
        hpText.text = HPTextContent();
    }

    private void InventorySet()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryPanel.activeSelf == false)
            {
                inventoryPanel.SetActive(true);
                hpAbiltyText.text = HPTextContent();
                atkText.text = player.PlayerAtk.ToString();
                Time.timeScale = 0f;
            }
            else
            {
                inventoryPanel.SetActive(false);
                Time.timeScale = 1f;
            }
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
}
