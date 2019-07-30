using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //移动速度
    public float speed;
    //跳跃力
    public float jumpForce = 550f;
    //脚的位置
    public Transform feetPos;
    //检测半径
    public float checkRadius;
    //检测层
    public LayerMask lyerMask;
    Rigidbody2D rig2D;
    Animator anim;
    //最后水平方向的朝向
    float last_delatX = 0f;
    //是否在地面
    bool isGround = false;
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("moveX", 1);
    }
    void Update()
    {

        //是否在地
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            // rig2D.velocity = Vector2.up * jumpForce;
            rig2D.AddForce(Vector2.up * jumpForce);
        }
    }
    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, lyerMask);

        //水平放心
        var h = Input.GetAxisRaw("Horizontal");
        //垂直方向
        // var v = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(h * speed, rig2D.velocity.y);

        if (Mathf.Abs(h) > Mathf.Epsilon)
        {
            last_delatX = h;
            anim.SetBool("IsRun", true);
        }
        else
        {
            anim.SetBool("IsRun", false);
        }
        anim.SetFloat("moveX", last_delatX);
        rig2D.velocity = move;
    }


}
