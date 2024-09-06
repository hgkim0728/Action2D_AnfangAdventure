using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    [SerializeField] private Transform exit;
    [SerializeField] private PolygonCollider2D polyCol;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CinemachineConfiner2D cine = 
                GameObject.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
            cine.m_BoundingShape2D = polyCol;
            collision.transform.position = exit.transform.position;
        }
    }
}
