using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 10f;
    //墙上滑行速度
    public float wallSlideSpeed = 3f;


    public float wallJumpLerp = 10;

    //是否跳跃
    private bool isJump = false;
    //抓住墙
    public bool grabWall = false;
    //墙上跳跃
    public bool wallJumped = false;

    private Rigidbody2D rb;
    CollisionController collisionController;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionController = GetComponent<CollisionController>();
        // animator = GetComponent<Animator>();
    }
    void Update()
    {

        if (collisionController.isWall && !collisionController.isGround && rb.velocity.y < 0)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                Vector2 speed = new Vector2(0, wallSlideSpeed);
                rb.velocity = -speed;
            }
        }
        //抓墙动作
        if (Input.GetKeyDown(KeyCode.LeftShift) && collisionController.isWall)
        {
            rb.gravityScale = 0;
            grabWall = true;
        }
        //松开抓墙动作
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            rb.gravityScale = 5;
            grabWall = false;
        }
        //跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
            if (collisionController.isGround)
            {
                Jump(Vector2.up);
            }
            else if (collisionController.isWall)
            {
                WallJump();
            }
        }
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        Walk(dir);
        if (grabWall)
        {
            WallSlide(dir);
        }
    }
    /// <summary>
    /// 行走
    /// </summary>
    /// <param name="dir"></param>
    private void Walk(Vector2 dir)
    {
        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }
    /// <summary>
    /// 爬墙
    /// </summary>
    /// <param name="dir"></param>
    private void WallSlide(Vector2 dir)
    {
        if (!collisionController.isWall)
        {
            return;
        }
        //抓墙时上下行走
        rb.velocity = new Vector2(rb.velocity.x, dir.y * speed);
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    /// <param name="dir"></param>
    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }
    /// <summary>
    /// 墙上跳跃
    /// </summary>
    private void WallJump()
    {

        Vector2 wallDir = collisionController.isRightWall ? Vector2.left : Vector2.right;
        Jump((Vector2.up / 1f + wallDir / 1f));
        wallJumped = true;
    }
    IEnumerator RestoringGravity()
    {
        rb.gravityScale = 5;
        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = 0;
    }
}