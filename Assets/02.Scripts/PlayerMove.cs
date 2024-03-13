using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpForce = 20.0f;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float direction = Input.GetAxisRaw("Horizontal") * playerSpeed * Time.deltaTime;
        Vector2 pos = transform.position;
        pos.x += direction;
        transform.position = pos;
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }
    }
}
