using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // 플레이어 이동 속도
    [SerializeField] private float jumpForce = 20.0f;   // 플레이어가 점프하는 힘

    private Rigidbody2D rigid;  // 플레이어 Rigidbody2D 컴포넌트
    private CapsuleCollider2D capsuleCollider;  // 플레이어 캡슐콜라이더 컴포넌트
    private Animator anim;  // 플레이어 애니메이터 컴포넌트

    private Vector2 moveDir;    // 플레이어 이동 방향
    private float verticalVelocity;     // 플레이어가 수직으로 받는 힘
    [SerializeField] private float gravity = 9.81f;  // 중력
    private bool isGround = false;  // 플레이어가 땅에 닿았는지 여부
    private bool isJump = false;    // 플레이어가 점프중인지 아닌지

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
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
        AnimationState();
    }

    /// <summary>
    /// 플레이어가 땅에 닿았는지를 체크
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
    /// 플레이어 좌우이동
    /// </summary>
    private void Move()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
        Turn();

        //float direction = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        //Vector2 pos = transform.position;
        //pos.x += direction;
        //transform.position = pos;
    }

    private void Turn()
    {
        if(moveDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(moveDir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    /// <summary>
    /// 플레이어 점프
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
        // 플레이어가 땅에 닿지 않은 상태, 공중에 있다면
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

        // 플레이어가 점프 상태라면
        if(isJump)
        {
            verticalVelocity = jumpForce;   // jumpForce만큼 수직으로 힘을 더함
            isJump = false;     // 점프 상태 해제
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void AnimationState()
    {
        if (!isJump)
        {
            if (moveDir.x != 0)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }
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
