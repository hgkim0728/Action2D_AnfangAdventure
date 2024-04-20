using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region 변수
    
    // 플레이어
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

    // UI
    [Space]
    [SerializeField, Tooltip("게임오버 패널")] GameObject gameoverPanel;
    [SerializeField, Tooltip("플레이어 체력바")] Slider hpSlider;

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
    }

    private void SetHPBar()
    {
        hpSlider.value = playerHp;
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
