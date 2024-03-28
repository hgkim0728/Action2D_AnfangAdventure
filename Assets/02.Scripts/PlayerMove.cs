using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 플레이어 장비
    enum PlayerEquip
    {
        Sword,
        Wand,
        Bow
    }

    // 플레이어 이동
    [SerializeField, Tooltip("플레이어 이동 속도")] private float moveSpeed = 5.0f;
    [SerializeField, Tooltip("플레이어가 점프하는 힘")] private float jumpForce = 20.0f;
    [SerializeField, Tooltip("점프한 플레이어에게 적용될 중력")] private float gravity = 9.81f;
    private Vector2 moveDir;    // 플레이어 이동 방향
    private int jumpCount = 0;  // 점프 횟수, 무한 점프 방지용
    private float verticalVelocity;     // 플레이어가 수직으로 받는 힘
    private bool isGround = false;  // 플레이어가 땅에 닿았는지 여부
    private bool isJump = false;    // 플레이어가 점프중인지 아닌지

    // 전투
    [Space]
    [SerializeField, Tooltip("플레이어 공격 범위 리스트")] private List<float> listAttackRange = new List<float>();
    [SerializeField, Tooltip("플레이어 캐릭터 체력")] private int playerHp = 10;
    [SerializeField, Tooltip("플레이어 캐릭터 공격력")] private int playerAtk = 1;
    [SerializeField] private PlayerEquip playerEquip;    // 현재 플레이어가 장착한 장비
    private float curAttackRange;  // 플레이어의 현재 공격 범위

    // 컴포넌트
    private Rigidbody2D rigid;  // 플레이어 Rigidbody2D 컴포넌트
    private CapsuleCollider2D capsuleCollider;  // 플레이어 캡슐콜라이더 컴포넌트
    private Animator playerAnim;  // 플레이어 애니메이터 컴포넌트
    private Animator weaponAnim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnim = GetComponent<Animator>();
    }

    void Start()
    {
        weaponAnim = transform.GetComponentInChildren<Animator>();  // 임시
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
        isGround = false;   // 기본적으로 false로

        if(verticalVelocity > 0)    // 점프해서 위로 올라가는 중이면 리턴
        {
            return;
        }

        // 박스캐스트로 플레이어 캐릭터가 땅에 닿은 상태인지 아닌지를 판단
        // 플레이어의 콜라이더의 중심에서 플레이어 콜라이더의 절반 높이에 0.1을 더한 값만큼 아래로 레이캐스트
        // 레이어가 Ground일 때만 hit에 담김
        //RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down,
        //    capsuleCollider.bounds.size.y / 2 + 0.1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 
            0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        //Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.size.y / 2 + 0.1f), Color.red);

        // 플레이어가 땅에 닿은 상태라면
        if (hit.transform != null)
        {
            isGround = true;    // isGround를 true로
            jumpCount = 0;
            playerAnim.SetBool("IsGround", true); // 애니메이터의 IsGround도 true로
            weaponAnim.SetBool("IsGround", true);
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
        // 플레이어가 오른쪽으로 이동중이라면
        if(moveDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);    // 캐릭터 스프라이트 방향을 오른쪽으로
        }
        else if(moveDir.x < 0)// 플레이어가 왼쪽으로 이동중이라면
        {
            transform.localScale = new Vector3(-1, 1, 1);   // 캐릭터 스프라이트 방향을 왼쪽으로
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

        // Z 키를 누르면
        if(Input.GetKeyDown(KeyCode.Z))
        {
            // 땅에 닿은 상태 또는 한 번 점프한 상태라면
            if (isGround == true || jumpCount == 1)
            {
                //rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                isJump = true;  // 점프 상태로 변경

                // 첫 번째 점프라면 점프 애니메이션 재생
                if (jumpCount != 1)
                {
                    playerAnim.SetBool("Jump", true);
                    playerAnim.SetBool("IsGround", false);
                    weaponAnim.SetBool("IsGround", false);
                }

                jumpCount++;    // jumpCount 증가
            }
        }
    }

    private void CheckGravity()
    {
        // 플레이어가 땅에 닿지 않은 상태, 공중에 있다면
        if(!isGround)
        {
            // 시간이 지날수록 아래로 향하는 힘 증가
            verticalVelocity -= gravity * Time.deltaTime;

            // 떨어지는 속도 제한
            if(verticalVelocity < -10f)
            {
                verticalVelocity = -10f;
            }
        }
        else
        {
            verticalVelocity = 0;   // 공중이 아니라면 아래로 향하는 힘이 필요없음
        }

        // 플레이어가 점프 상태라면
        if(isJump)
        {
            verticalVelocity = 0;
            verticalVelocity = jumpForce;   // jumpForce만큼 수직으로 힘을 더함
            isJump = false;     // 점프 상태 해제

            // 이번이 두 번째 점프라면 jumpCount를 0으로
            if(jumpCount == 2)
            {
                jumpCount = 0;
            }
        }

        // 위에서 조건에 따라 적용한 verticalVelocity 값을 리지드바디에 적용
        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    /// <summary>
    /// 애니메이션 상태 변경
    /// 여기서만 변경되는 건 아님
    /// </summary>
    private void AnimationState()
    {
        // 애니메이터의 공격 애니메이션을 false로
        //playerAnim.SetBool("Slash", false);
        //playerAnim.SetBool("Shot", false);

        // 점프 상태가 아니고
        if (!isJump)
        {
            // 방향키를 누르는 중이라면
            if (moveDir.x != 0)
            {
                playerAnim.SetBool("Run", true);  // 애니메이터의 Run을 true로
            }
            else// 방향키를 누르고 있지 않다면
            {
                playerAnim.SetBool("Run", false); // 애니메이터의 Run을 false로
            }
        }

        // 현재 재생되는 애니메이션이 점프 애니메이션이고 애니메이션 재생이 끝났다면
        if(playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Anfang_Jump_Animation") == true
            && playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            playerAnim.SetBool("Jump", false);    // 애니메이터의 Jump를 false로
        }

        // 플레이어가 낙하중이라면
        if(verticalVelocity < 0)
        {
            playerAnim.SetBool("IsGround", false);    // 애니메이터의 IsGround를 false로
            weaponAnim.SetBool("IsGround", false);
        }

        // 공격 키를 누르면
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Attack();
        }
    }

    /// <summary>
    /// 플레이어가 현재 장착한 장비에 따라 공격을 실행
    /// </summary>
    private void Attack()
    {
        switch(playerEquip)
        {
            // 현재 장착한 장비가 검이라면
            case PlayerEquip.Sword:
                SwordAttack();
                break;

            // 현재 장착한 장비가 활이라면
            case PlayerEquip.Bow:
                BowAttack();
                break;
        }
    }

    private void SwordAttack()
    {
        // 작동 확인용 나중에 고쳐야 함
        RaycastHit2D hit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size,
            90f, Vector2.right * transform.localScale.x, curAttackRange * transform.localScale.x,
            LayerMask.GetMask("Monster"));
        //playerAnim.SetBool("Slash", true);   // 애니메이터의 Slash를 true로
        playerAnim.SetTrigger("Slash");

        // 피격당한 몬스터에게서 피격 함수 호출
        if (hit.transform != null)
        {
            hit.transform.gameObject.GetComponent<Monster>().MonsterHit(playerAtk);
        }
    }

    private void BowAttack()
    {
        playerAnim.SetBool("Shot", true);
        // 발사체를 발사하는 코드를 추가해야 함
    }

    /// <summary>
    /// 공격 범위 비활성화
    /// 공격 애니메이션 끝에 이벤트
    /// </summary>
    //public void AttackRangeOff()
    //{
    //    curAttackRange.SetActive(false);    // 공격 범위 비활성화
    //}

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
