using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField, Tooltip("페이드 인&아웃에 사용할 패널")] GameObject fadePanel;
    [Space]
    [SerializeField, Tooltip("페이드 인&아웃 되는 데 걸리는 시간")] float fadeOutTime;

    private Image fadeImg;  // 패널의 이미지 컴포넌트
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
        fadePanel.SetActive(true);  // 패널 활성화
        Color col = fadeImg.color;    // 패널 컬러 가져오기

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
        Color col = fadeImg.color;    // 패널 컬러 가져오기

        while (col.a > 0)
        {
            col.a -= Time.deltaTime / fadeOutTime;

            if (col.a <= 0) col.a = 0;

            fadeImg.color = col;

            yield return null;
        }

        if (_action != null) _action();
        if (playerSc != null) playerSc.IsLock = false;

        fadePanel.SetActive(false);  // 패널 비활성화
    }
}
