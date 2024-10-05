using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] Fade fadeSc;
    [SerializeField, Tooltip("버츄얼 카메라 활성화 후 페이드 아웃을 시작할 때까지의 시간")] float startFadeOutTime;

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
