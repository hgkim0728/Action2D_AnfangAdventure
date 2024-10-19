using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    enum MoveType { Horizontal, Vertical }

    [SerializeField] private MoveType moveType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveScale;
    float moveTime;
    int moveDir = 1;

    void Awake()
    {
        moveTime = moveScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    void Update()
    {
        if(moveType == MoveType.Horizontal)
        {
            HorizontalMove();
        }
        else
        {
            VerticalMove();
        }
    }

    private void HorizontalMove()
    {
        transform.Translate(Vector2.right * moveDir * moveSpeed * Time.deltaTime);

        MoveTimeCount();
    }

    private void VerticalMove()
    {
        transform.Translate(Vector2.up * moveDir * moveSpeed * Time.deltaTime);

        MoveTimeCount();
    }

    private void MoveTimeCount()
    {
        moveTime -= Time.deltaTime;

        if(moveTime <= 0)
        {
            moveDir *= -1;
            moveTime = moveScale;
        }
    }
}
