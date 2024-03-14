using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // �÷��̾� �̵� �ӵ�
    [SerializeField] private float jumpForce = 20.0f;   // �÷��̾ �����ϴ� ��

    private Rigidbody2D rigid;  // �÷��̾� Rigidbody2D ������Ʈ
    private CapsuleCollider2D capsuleCollider;  // �÷��̾� ĸ���ݶ��̴� ������Ʈ

    private Vector2 moveDir;    // �÷��̾� �̵� ����
    private float verticalVelocity;     // �÷��̾ �������� �޴� ��
    private float gravity = 9.81f;  // �߷�
    private bool isGround = false;  // �÷��̾ ���� ��Ҵ��� ����
    private bool isJump = false;    // �÷��̾ ���������� �ƴ���

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
        CheckGround();
        Move();
        Jump();
        CheckGravity();
    }

    /// <summary>
    /// �÷��̾ ���� ��Ҵ����� üũ
    /// </summary>
    private void CheckGround()
    {
        isGround = false;

        if(verticalVelocity > 0)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, 
            capsuleCollider.bounds.size.y / 2 + 0.1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.size.y / 2 + 0.1f), Color.red);

        if(hit.transform != null)
        {
            isGround = true;
        }
    }

    /// <summary>
    /// �÷��̾� �¿��̵�
    /// </summary>
    private void Move()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
        //float direction = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        //Vector2 pos = transform.position;
        //pos.x += direction;
        //transform.position = pos;
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    private void Jump()
    {
        //if(Physics2D.Raycast(transform.position, Vector2.down, capsuleCollider.size.y / 2 + 0.3f, LayerMask.GetMask("Ground")))
        //{
        //    isGround = true;
        //}

        if(Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            //rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            isJump = true;
        }
    }

    private void CheckGravity()
    {
        // �÷��̾ ���� ���� ���� ����, ���߿� �ִٸ�
        if(!isGround)
        {
            verticalVelocity -= gravity * Time.deltaTime;

            if(verticalVelocity < -10f)
            {
                verticalVelocity = -10f;
            }
        }
        else
        {
            verticalVelocity = 0;
        }

        // �÷��̾ ���� ���¶��
        if(isJump)
        {
            verticalVelocity = jumpForce;   // jumpForce��ŭ �������� ���� ����
            isJump = false;     // ���� ���� ����
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    // ȭ�� ���� ���߱� �ڵ�
    private void S()
    {
        float targetRatio = 9f / 16f;
        float ratio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = ratio / targetRatio;
        float fixedWidth = (float)Screen.width / scaleHeight;
        Screen.SetResolution((int)fixedWidth, Screen.height, true);
    }
}
