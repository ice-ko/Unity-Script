using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;//移动速度
    public float jumpForce = 16f;//跳跃力
    public float slideSpeed = 0.2f;//滑动速度
    public float movementForceInAir = 10f;//空战的移动速度
    public bool isGround;//是否在地面
    public int amountOfJumps = 2;//跳跃次数

    //
    public Transform groundCheck;//地面检查位置
    public float groundCheckRadius;//地面检查半径
    public LayerMask whatIsGround;//检查的层
    public Transform wallCheck;//墙检查位置
    public float wallCheckDistance;//检查距离
    //
    private Rigidbody2D rig2d;
    private Animator animator;
    private float move;//移动方向
    private int amountOfJumpsLeft;//已跳跃数
    private bool isTouchingWall;//是否触碰到墙

    private int facingDirection = 1;//当前朝向方向
    private bool isFacingRight = true;
    private bool isWallSliding;//是否在墙上滑行
    void Start()
    {
        rig2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        amountOfJumps = amountOfJumpsLeft;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        CheckIfWallSliding();
    }
    private void FixedUpdate()
    {
        //检查是否在地面
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        //检查墙
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        //移动
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rig2d.velocity = new Vector2(h * moveSpeed, rig2d.velocity.y);
        if (h != 0)
        {
            move = h;
            Flip();
            animator.SetBool("IsMove", true);
        }
        else
        {
            animator.SetBool("IsMove", false);
        }
        if (rig2d.velocity.y < 0 && isTouchingWall)
        {
            isWallSliding = isTouchingWall;
            rig2d.velocity = new Vector2(moveSpeed*movementForceInAir,rig2d.velocity.y);
        }
        //
        CheckIfCanJump();
        animator.SetBool("IsGround", isGround);
        animator.SetBool("IsClimbingWall", isWallSliding);
        animator.SetFloat("yVelocity", rig2d.velocity.y);
    }
    /// <summary>
    /// 翻转朝向
    /// </summary>
    private void Flip()
    {
        //
        if (!isWallSliding)
        {
            facingDirection *= -1;
            transform.localScale = new Vector3(move, transform.localScale.y);
        }
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    public void Jump()
    {
        if (amountOfJumpsLeft > 0 )
        {
            rig2d.velocity = new Vector2(rig2d.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }
    /// <summary>
    /// 检查是否可以跳跃
    /// </summary>
    private void CheckIfCanJump()
    {
        if (isGround && rig2d.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGround && rig2d.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void OnDrawGizmos()
    {
        //绘制圆
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        //绘制线
        var pos = wallCheck.position;
        pos.x = pos.x + wallCheckDistance;
        Gizmos.DrawLine(wallCheck.position, pos);
    }
}
