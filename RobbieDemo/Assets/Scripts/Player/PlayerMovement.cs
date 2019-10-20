using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed = 8f;
    /// <summary>
    /// 蹲伏速度除数
    /// </summary>
    public float crouchSpeedDivisor = 3f;
    /// <summary>
    /// 是否下蹲
    /// </summary>
    public bool isCrouch;

    private Rigidbody2D rb;
    private BoxCollider2D boxColl;

    private float xVelocity;
    /// <summary>
    /// 站立时BoxCollider2D的大小跟偏移，下蹲时BoxCollider2D的大小跟偏移
    /// </summary>
    private Vector2 standSize, standOffset, crouchSize, crouchOffset;
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

    }
    private void FixedUpdate()
    {
        GroundMovement();
    }
    /// <summary>
    /// 地面移动
    /// </summary>
    void GroundMovement()
    {
        if (Input.GetButton("Crouch"))
        {
            Crouch();
        }
        else if (!Input.GetButton("Crouch") && isCrouch)
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
}
