using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动参数")]
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed = 8f;
    /// <summary>
    /// 蹲伏速度除数
    /// </summary>
    public float crouchSpeedDivisor = 3f;
    [Header("跳跃参数")]
    /// <summary>
    /// 跳跃力
    /// </summary>
    public float jumpForce = 6.3f;
    /// <summary>
    /// 跳跃持续力
    /// </summary>
    public float jumpHoldForce = 1.9f;
    /// <summary>
    /// 跳跃持续时间
    /// </summary>
    public float jumpHoldDuration = 0.1f;
    /// <summary>
    /// 下蹲跳跃加成
    /// </summary>
    public float crouchJumpBoost = 2.5f;
    /// <summary>
    /// 悬挂跳跃力
    /// </summary>
    public float hangingJumpForce = 15f;

    [Header("状态")]
    /// <summary>
    /// 是否在地面
    /// </summary>
    public bool isOnGround;
    /// <summary>
    /// 是否下蹲
    /// </summary>
    public bool isCrouch;
    /// <summary>
    /// 是否跳跃
    /// </summary>
    public bool isJump;
    /// <summary>
    /// 头顶是否被挡住
    /// </summary>
    public bool isHeadBlocked;
    /// <summary>
    /// 是否悬挂状态
    /// </summary>
    public bool isHanging;

    [Header("环境设置")]
    public LayerMask groundLayer;
    /// <summary>
    /// 脚偏移
    /// </summary>
    public float footOffset = 0.4f;
    /// <summary>
    /// 头顶缝隙
    /// </summary>
    public float headClearance = 0.5f;
    /// <summary>
    /// 距离地面的距离
    /// </summary>
    public float groundDistance = 0.2f;
    /// <summary>
    /// 眼睛位置
    /// </summary>
    public float eyeHeight = 1.5f;
    /// <summary>
    /// 抓取距离
    /// </summary>
    public float grabDistance = 0.4f;
    /// <summary>
    /// 达到偏移
    /// </summary>
    public float reachOffset = 0.7f;

    private Rigidbody2D rb;
    private BoxCollider2D boxColl;
    /// <summary>
    /// 移动X轴
    /// </summary>
    private float xVelocity;
    /// <summary>
    /// 跳跃时间
    /// </summary>
    private float jumpTime;
    /// <summary>
    /// 站立时BoxCollider2D的大小跟偏移，下蹲时BoxCollider2D的大小跟偏移
    /// </summary>
    private Vector2 standSize, standOffset, crouchSize, crouchOffset;
    //按键设置
    /// <summary>
    /// 按下跳跃
    /// </summary>
    bool jumpPressed;
    /// <summary>
    /// 长按跳跃
    /// </summary>
    bool jumpHeld;
    /// <summary>
    /// 长按蹲下
    /// </summary>
    bool crouchHeld;
    /// <summary>
    /// 按下蹲下
    /// </summary>
    bool crouchPressed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
        standSize = boxColl.size;
        standOffset = boxColl.offset;
        crouchSize = new Vector2(boxColl.size.x, boxColl.size.y / 2);
        crouchOffset = new Vector2(boxColl.offset.x, boxColl.offset.y / 2);
    }
    void Update()
    {
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
        crouchPressed = Input.GetButtonDown("Crouch");

    }
    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        Jump();
    }
    /// <summary>
    /// 物理检查
    /// </summary>
    void PhysicsCheck()
    {
        //左右脚射线
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftCheck || rightCheck)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
        //头顶射线
        RaycastHit2D headCheck = Raycast(new Vector2(0f, boxColl.size.y), Vector2.up, headClearance, groundLayer);
        if (headCheck)
        {
            isHeadBlocked = true;
        }
        else
        {
            isHeadBlocked = false;
        }
        Vector2 direction = new Vector2(transform.localScale.x, 0f);
        //检测头顶是否有障碍物
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction.x, boxColl.size.y), direction, grabDistance, groundLayer);
        //检测墙
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction.x, eyeHeight), direction, grabDistance, groundLayer);
        //墙壁检测
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction.x, boxColl.size.y), Vector2.down, grabDistance, groundLayer);
        if (!isOnGround && rb.velocity.y < 0f && ledgeCheck && wallCheck && !blockedCheck)
        {
            //重新设置角色位置
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance - 0.05f) * direction.x;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            //
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    void Jump()
    {
        if (isHanging)
        {
            //向上
            if (jumpPressed)
            {
                //悬挂跳跃
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
            //落下
            if (crouchPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }
        if (jumpPressed && isOnGround && !isJump && !isHeadBlocked)
        {
            if (isCrouch & isOnGround)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;
            //计算时间
            jumpTime = Time.time + jumpHoldDuration;
            //设置给刚体添加力
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if (isJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
            {
                isJump = false;
            }
        }
    }
    /// <summary>
    /// 地面移动
    /// </summary>
    void GroundMovement()
    {
        if (isHanging)
        {
            return;
        }
        if (crouchHeld && !isCrouch && isOnGround)
        {
            Crouch();
        }
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
        {
            StandUp();
        }
        else if (!isOnGround && isCrouch)
        {
            StandUp();
        }

        if (isCrouch)
        {
            //计算下蹲移动速度
            xVelocity /= crouchSpeedDivisor;
        }
        //获取按键移动
        xVelocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        Flip();
    }
    /// <summary>
    /// 翻转方向
    /// </summary>
    void Flip()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        if (xVelocity > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }
    /// <summary>
    /// 蹲下
    /// </summary>
    void Crouch()
    {
        isCrouch = true;
        boxColl.size = crouchSize;
        boxColl.offset = crouchOffset;
    }
    /// <summary>
    /// 站起来
    /// </summary>
    void StandUp()
    {
        isCrouch = false;
        boxColl.size = standSize;
        boxColl.offset = standOffset;
    }
    /// <summary>
    /// 射线检测
    /// </summary>
    /// <param name="offset">偏移量</param>
    /// <param name="rayDirection">方向</param>
    /// <param name="length">长度</param>
    /// <param name="layer">检测的层级</param>
    /// <returns></returns>
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(pos + offset, rayDirection, length, layer);
        Color color = hit2D ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit2D;
    }
}
