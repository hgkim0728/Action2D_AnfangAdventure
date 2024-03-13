using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpForce = 20.0f;

    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleCollider;

    [SerializeField]private bool isGround = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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
        if(Physics2D.Raycast(transform.position, Vector2.down, capsuleCollider.size.y / 2 + 0.3f, LayerMask.GetMask("Ground")))
        {
            isGround = true;
        }
        Debug.DrawRay(transform.position, new Vector3(0, -capsuleCollider.size.y / 2 - 0.1f), Color.red);

        if(Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }
    }

    // 화면 비율 맞추기 코드
    private void S()
    {
        float targetRatio = 9f / 16f;
        float ratio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = ratio / targetRatio;
        float fixedWidth = (float)Screen.width / scaleHeight;
        Screen.SetResolution((int)fixedWidth, Screen.height, true);
    }
}
