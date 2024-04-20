using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region ����
    
    // �÷��̾�
    [SerializeField, Tooltip("�÷��̾� ĳ���� ������Ʈ")] PlayerMove player;
    private int maxHp = 10;
    [SerializeField, Tooltip("�÷��̾� ĳ���� ü��")] private int playerHp = 10;
    public int PlayerHp
    {
        get { return playerHp; }
        set { playerHp = value; }
    }

    // ����
    [Space]
    [SerializeField, Tooltip("Ʃ�丮���� �����ߴ���")] bool tutorial = false;

    // UI
    [Space]
    [SerializeField, Tooltip("�÷��̾� ü�¹�")] Slider hpSlider;

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
        Time.timeScale = 0.0f;
    }
}
