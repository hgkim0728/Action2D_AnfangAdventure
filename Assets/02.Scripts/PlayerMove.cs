using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("플레이어 이동 속도")] private float moveSpeed = 5.0f;
    [SerializeField, Tooltip("플레이어가 점프하는 힘")] private float jumpForce = 20.0f;
    [SerializeField, Tooltip("점프한 플레이어에게 적용될 중력")] private float gravity = 9.81f;

    [Space]
    [SerializeField, Tooltip("플레이어 공격 범위 리스트")] private List<GameObject> listAttackRange = new List<GameObject>();

    private Rigidbody2D rigid;  // 플레이어 Rigidbody2D 컴포넌트
    private CapsuleCollider2D capsuleCollider;  // 플레이어 캡슐콜라이더 컴포넌트
    private Animator anim;  // 플레이어 애니메이터 컴포넌트

    private GameObject curAttackRange;  // 플레이어의 현재 공격 범위
    private Vector2 moveDir;    // 플레이어 이동 방향
    private int jumpCount = 0;
    private float verticalVelocity;     // 플레이어가 수직으로 받는 힘
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
        curAttackRange = listAttackRange[0];
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
            anim.SetBool("IsGround", true);
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

    /// <summary>
    /// 캐릭터 좌우 이동에 따라 캐릭터 스프라이트 방향을 바꿔줌
    /// </summary>
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

        // 땅에 닿은 상태에서 스페이스 키를 누르면
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround == true || jumpCount == 1)
            {
                //rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                isJump = true;

                if (jumpCount != 1)
                {
                    anim.SetBool("Jump", true);
                }

                jumpCount++;
            }
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
            verticalVelocity = 0;
            verticalVelocity = jumpForce;   // jumpForce만큼 수직으로 힘을 더함
            isJump = false;     // 점프 상태 해제

            if(jumpCount == 2)
            {
                jumpCount = 0;
            }
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    /// <summary>
    /// 애니메이션 상태 변경
    /// 여기서만 변경되는 건 아님
    /// </summary>
    private void AnimationState()
    {
        anim.SetBool("Attack", false);  // 애니메이터의 Attack 값을 false로

        // 점프 상태가 아니고
        if (!isJump)
        {
            // 방향키를 누르는 중이라면
            if (moveDir.x != 0)
            {
                anim.SetBool("Run", true);  // 애니메이터의 Run을 true로
            }
            else// 방향키를 누르고 있지 않다면
            {
                anim.SetBool("Run", false); // 애니메이터의 Run을 false로
            }
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Anfang_Jump_Animation") == true
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.SetBool("Jump", false);
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Anfang_Jump_Animation") == true
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            curAttackRange.SetActive(false);
        }

        if(verticalVelocity < 0)
        {
            anim.SetBool("IsGround", false);
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("Attack", true);
            curAttackRange.SetActive(true);
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
