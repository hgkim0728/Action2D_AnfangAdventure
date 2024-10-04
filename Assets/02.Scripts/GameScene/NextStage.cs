using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    [SerializeField, Tooltip("�ⱸ�� �� ���ӿ�����Ʈ")] private Transform exit;
    [SerializeField, Tooltip("���̵� ��&�ƿ� ��ũ��Ʈ")] Fade fadeSc;

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
