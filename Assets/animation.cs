using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField]Collider2D standingCollider;
    [SerializeField]Transform groundCheckCollider;

    [SerializeField]LayerMask groundLayer;

    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float jumpPower = 500;
    float horizontalValue;
    
    bool isGrounded;
    bool facingRight = true;
    bool jump;
    bool crouchPressed;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
          //animator의 yVelocity(y속도)를 설정합니다.
        animator.SetFloat("yVelocity", rb.velocity.y);

        //수평 값을 저장(horizontal = 수평)
        horizontalValue = Input.GetAxisRaw("Horizontal");


        //점프 버튼을 누르면 점프가 활성화됩니다.
        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
        //점프 버튼을 누르지 않을시 비활성화
        else if(Input.GetButtonUp("Jump"))
            {
            jump = false;
            animator.SetBool("Jump", false);
            }


    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jump);
    }

    void GroundCheck()
    {
        isGrounded = false;
        //GroundCheckObject가 다른 개체와 충돌하는지 확인하십시오.
        //"Ground" 레이어에 있는 2D 충돌체
        //그렇다면(isGrounded 참) else(isGrounded 거짓)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(colliders.Length > 0)
        isGrounded = true;

        //땅에 붙어있는한 "Jump" animator은 비활성화
        animator.SetBool("Jump", !isGrounded);
    }

    void Move(float dir, bool jumpFlag)
    {   
        
            //플레이어가 스페이스바를 누른 경우
             if(jumpFlag)
            {
            
            jumpFlag = false;
            //점프력 추가
            rb.AddForce(new Vector2(0f,jumpPower));
            }
        

       

      
        
      
        //dir 및 속도를 사용하여 x 값 설정
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        //실행 수정자와 곱셈을 실행하는 경우
      
        //속도에 대한 Vec2 생성
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        //플레이어의 속도 설정
        rb.velocity = targetVelocity;

        //현재 스케일 값 저장
        Vector3 currentScale = transform.localScale;
        //오른쪽을 보고 왼쪽을 클릭한 경우(왼쪽으로 뒤집기)
        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        //왼쪽을 보고 오른쪽을 클릭한 경우(오른쪽으로 뒤집기)
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }

        // (0 idle, 4 walk, 8 running)
        //x 값에 따라 float xVelocity를 설정합니다.
        //RigidBody2D 속도
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        
    }
}
