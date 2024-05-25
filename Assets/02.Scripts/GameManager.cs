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

        // 플레이어 강화 때 그냥 함수로 만들어버리는 것도?
        hpSlider.maxValue = maxHp;  // 체력바의 최대치를 플레이어 캐릭터의 최대 체력으로
        hpSlider.value = playerHp;  // 체력바의 값을 현재 플레이어의 체력으로
    }

    void Start()
    {
        
    }

    void Update()
    {
        SetHPBar();
        InventoryKey();
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
                // 능력치 창에 현재 플레이어의 체력과 공격력 표시
                hpAbiltyText.text = HPTextContent();
                atkText.text = player.PlayerAtk.ToString();
                Time.timeScale = 0f;    // 게임 내 시간 정지
            }
            else // 인벤토리가 열려있는 경우
            {
                inventoryPanel.SetActive(false);    // 인벤토리 창 비활성화
                Time.timeScale = 1f;    // 게임 내 시간 재생
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
