using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField, Tooltip("���̵� ��&�ƿ��� ����� �г�")] GameObject fadePanel;
    [Space]
    [SerializeField, Tooltip("���̵� ��&�ƿ� �Ǵ� �� �ɸ��� �ð�")] float fadeOutTime;

    private Image fadeImg;  // �г��� �̹��� ������Ʈ
    private PlayerMove playerSc;

    void Start()
    {
        fadeImg = fadePanel.GetComponent<Image>();
        playerSc = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMove>();
    }

    public void FadeIn(Action _action = null)
    {
        StartCoroutine(CoFadeIn(_action));
    }

    public void FadeOut(Action _action = null)
    {
        StartCoroutine(CoFadeOut(_action));
    }

    IEnumerator CoFadeIn(Action _action)
    {
        if (playerSc != null) playerSc.IsLock = true;
        fadePanel.SetActive(true);  // �г� Ȱ��ȭ
        Color col = fadeImg.color;    // �г� �÷� ��������

        while(col.a < 1)
        {
            col.a += Time.deltaTime / fadeOutTime;

            if (col.a >= 1) col.a = 1;

            fadeImg.color = col;

            yield return null;
        }

        if(_action != null) _action();
    }

    IEnumerator CoFadeOut(Action _action)
    {
        Color col = fadeImg.color;    // �г� �÷� ��������

        while (col.a > 0)
        {
            col.a -= Time.deltaTime / fadeOutTime;

            if (col.a <= 0) col.a = 0;

            fadeImg.color = col;

            yield return null;
        }

        if (_action != null) _action();
        if (playerSc != null) playerSc.IsLock = false;

        fadePanel.SetActive(false);  // �г� ��Ȱ��ȭ
    }
}
