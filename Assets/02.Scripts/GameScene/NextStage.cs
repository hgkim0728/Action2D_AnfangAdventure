using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    [SerializeField, Tooltip("출구가 될 게임오브젝트")] private Transform exit;
    [SerializeField, Tooltip("페이드 인&아웃 스크립트")] Fade fadeSc;

    Transform playerTrs;

    Action fadeInNextAction;

    void Start()
    {
        fadeInNextAction += FadeInNextFunc;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerTrs = collision.transform;
            fadeSc.FadeIn(fadeInNextAction);
        }
    }

    private void FadeInNextFunc()
    {
        playerTrs.transform.position = exit.transform.position;
    }
}
