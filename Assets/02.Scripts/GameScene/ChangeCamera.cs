using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] Fade fadeSc;
    [SerializeField, Tooltip("����� ī�޶� Ȱ��ȭ �� ���̵� �ƿ��� ������ �������� �ð�")] float startFadeOutTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(true);
            Invoke("NextVirtualCamActive", startFadeOutTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(false);
        }
    }

    private void NextVirtualCamActive()
    {
        fadeSc.FadeOut();
    }
}
