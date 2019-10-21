using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Transform rayDown, rayLeft, rayRight;

    public LayerMask platformLayer, obstacleLayer;
    /// <summary>
    /// 是否向左移动，否则向右移动
    /// </summary>
    private bool isMoveLeft = false;
    /// <summary>
    /// 是否正在跳跃
    /// </summary>
    private bool isJumping = false;
    /// <summary>
    /// 下一个平台的位置
    /// </summary>
    private Vector3 nextPlatformLeft, nextPlatformRight;
    /// <summary>
    /// 最后一个平台
    /// </summary>
    private GameObject lastHitGo;
    private bool isMove = false;

    private ManagerVars vars;
    private Rigidbody2D rig2D;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        rig2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Debug.DrawRay(rayDown.position, Vector2.down * 1, Color.red);
        Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
        Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);
        if (GameManager.Instance.IsGameStart == false || GameManager.Instance.IsGameOver || GameManager.Instance.IsPause || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //if (Input.GetMouseButtonDown(0) && isJumping == false)
        if (Input.GetMouseButtonDown(0) && isJumping == false && nextPlatformLeft != Vector3.zero)
        {
            if (isMove == false)
            {
                EventCenter.Broadcast(EventType.PlayerMove);
                isMove = true;
            }

            EventCenter.Broadcast(EventType.DecidePath);

            isJumping = true;
            Vector3 mousePos = Input.mousePosition;
            //是否点击左边屏幕
            if (mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }
            //是否点击右边屏幕
            else if (mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;
            }
            Jump();
        }

        //游戏结束 逻辑
        if (rig2D.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;

            StartCoroutine(CoroutineHelper.DealyBroadcast(EventType.ShowGameOverPanel));
        }
        if (isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
            GameObject go = SimplePool.Spawn(vars.deathEffect, transform.position);
            GameManager.Instance.IsGameOver = true;
            //回收对象
            StartCoroutine(CoroutineHelper.RecoverPoolCoroutine(go));
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(CoroutineHelper.DealyBroadcast(EventType.ShowGameOverPanel));
            Destroy(gameObject, 1f);

           
        }
        if (transform.position.y - Camera.main.transform.position.y < -6 && GameManager.Instance.IsGameOver == false)
        {
            GameManager.Instance.IsGameOver = true;
            print("游戏结束");
            StartCoroutine(CoroutineHelper.DealyBroadcast(EventType.ShowGameOverPanel));
        }
    }

    /// <summary>
    /// 是否射线检测平台
    /// </summary>
    /// <returns></returns>
    private bool IsRayPlatform()
    {
        //向下发射射线
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null && hit.collider.tag == "Platform")
        {
            //判断是否同一个平台 如果不是则添加分数
            if (lastHitGo != hit.collider.gameObject)
            {
                if (lastHitGo == null)
                {
                    lastHitGo = hit.collider.gameObject;
                    return true;
                }
                EventCenter.Broadcast(EventType.AddScore);
                lastHitGo = hit.collider.gameObject;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 是否检测到障碍物
    /// </summary>
    /// <returns></returns>
    private bool IsRayObstacle()
    {
        //发射射线
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);
        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstace")
            {
                return true;
            }
        }

        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstace")
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        if (isJumping)
        {
            if (isMoveLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.DOMoveX(nextPlatformLeft.x, 0.2f);
                transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
            }
            else
            {
                transform.DOMoveX(nextPlatformRight.x, 0.2f);
                transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
                transform.localScale = Vector3.one;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatform = collision.gameObject.transform.position;
            //计算左边平台的位置
            nextPlatformLeft = new Vector3(currentPlatform.x - vars.nextXPos, currentPlatform.y + vars.nextYPos, 0);
            //计算右边平台的位置
            nextPlatformRight = new Vector3(currentPlatform.x + vars.nextXPos, currentPlatform.y + vars.nextYPos, 0);
        }
        if (collision.transform.tag == "Pickup")
        {
            EventCenter.Broadcast(EventType.AddDiamond);
            SimplePool.Despawn(collision.gameObject);
        }
    }
}
