using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    //射线检测
    public Transform groundCheck;//地面检查位置
    public float groundCheckRadius;//地面检查半径
    public LayerMask whatIsGround;//检查的层
    //私有变量
    private float finalOrientation;//最后的方向
    private bool isJump = false;
    private bool isGround;//是否在地面
    //组件
    private Rigidbody2D rig2D;
    private Animator animator;
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rig2D.velocity = new Vector2(rig2D.velocity.x, jumpForce);
            isJump = true;
        }
    }
    private void FixedUpdate()
    {
        //地面检测
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        //
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (isGround)
        {
            if (h != 0)
            {
                finalOrientation = h;
                Flip();
                animator.SetBool("IsRun", true);
            }
            else
            {
                animator.SetBool("IsRun", false);
            }
            rig2D.velocity = new Vector2(h * speed, rig2D.velocity.y);
        }
        animator.SetBool("IsJump", isGround);
        animator.SetFloat("yBlend", rig2D.velocity.y);
    }
    void Flip()
    {
        transform.localScale = new Vector3(finalOrientation, transform.localScale.y);
    }
    private void OnDrawGizmos()
    {
        //绘制圆
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        //绘制线
        //var pos = wallCheck.position;
        //pos.x = pos.x + wallCheckDistance;
        //Gizmos.DrawLine(wallCheck.position, pos);
    }
}
