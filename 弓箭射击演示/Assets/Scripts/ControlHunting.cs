using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHunting : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject arrowPrefab;//箭预制体
    public int arrows = 5;//箭的数量
    public float forceSize=800;//力的大小
    //
    GameObject bowString;//弓的子对象
    GameObject arrow;//实例化的箭
    //线渲染器
    List<Vector3> bowStringPosition;//弓弦的位置
    LineRenderer bowStringLinerenderer;//使用线渲染器来做弓弦
    Vector3 stringPullout; //线渲染器中间部分的位置
    Vector3 stringRestPosition = new Vector3(-0.44f, -0.06f, 2f);
    //
    float arrowStartX = 0.8f;//位置
    float length;//箭拉出的长度
    bool issArrowShot;//是否射击
    void Start()
    {
        bowString = transform.GetChild(0).gameObject;
        //添加线渲染器并设置宽带、顶点数
        bowStringLinerenderer = bowString.AddComponent<LineRenderer>();
        bowStringLinerenderer.startWidth = 0.05f;
        bowStringLinerenderer.positionCount = 3;
        //不启用
        bowStringLinerenderer.useWorldSpace = false;
        //设置层级
        bowStringLinerenderer.sortingOrder = 5;
        //设置材质
        bowStringLinerenderer.material = Resources.Load("Materials/bowStringMaterial") as Material;
        //顶点位置
        bowStringPosition = new List<Vector3>();
        bowStringPosition.Add(new Vector3(-0.44f, 1.43f, 2f));
        bowStringPosition.Add(new Vector3(-0.44f, -0.06f, 2f));
        bowStringPosition.Add(new Vector3(-0.43f, -1.32f, 2f));
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
        //设置中心位置
        stringPullout = stringRestPosition;
        //创建箭
        CreateArrow(true);
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PrepareArrow();
        }
        if (Input.GetMouseButtonUp(0))
        {
            ShootArrow();
        }
        DrawBowString();
    }
    /// <summary>
    /// 绘制弓弦
    /// </summary>
    public void DrawBowString()
    {
        bowStringLinerenderer = bowString.GetComponent<LineRenderer>();
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, stringPullout);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
    }
    /// <summary>
    /// 创建箭 
    /// </summary>
    /// <param name="hitTarget"></param>
    public void CreateArrow(bool hitTarget)
    {
        // 玩家还有箭吗？
        if (arrows > 0)
        {
            //现在实例化一个新箭头
            this.transform.localRotation = Quaternion.identity;
            arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            arrow.name = "arrow";
            arrow.transform.localScale = this.transform.localScale;
            arrow.transform.localPosition = this.transform.position + new Vector3(arrowStartX, 0, 0);
            arrow.transform.localRotation = this.transform.localRotation;
            arrow.transform.parent = this.transform;
            issArrowShot = false;
            //减去一个箭头
            arrows--;
        }
        else
        {
            //没有箭，
            //所以游戏结束了
            //gameState = GameStates.over;
            //gameOverCanvas.enabled = true;
            //endscoreText.text = "You shot all the arrows and scored " + score + " points.";
        }
    }
    /// <summary>
    /// 准备射击
    /// </summary>
    public void PrepareArrow()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider != null && issArrowShot == false)
        {
            // 确定屏幕上的位置
            var posX = pos.x;
            var posY = pos.y;
            // 计算位置
            Vector2 mousePos = new Vector2(transform.position.x - posX, transform.position.y - posY);
            //计算倾斜角
            float angleZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angleZ);
            // 确定箭拉出的长度
            length = mousePos.magnitude / 3f;
            length = Mathf.Clamp(length, 0, 1);
            // 设置弓弦线渲染器
            stringPullout = new Vector3(-(0.44f + length), -0.06f, 2f);
            //设置箭尾位置
            Vector3 arrowPosition = arrow.transform.localPosition;
            arrowPosition.x = (arrowStartX - length);
            arrow.transform.localPosition = arrowPosition;
        }
    }
    /// <summary>
    /// 释放箭
    /// 获得弓箭旋转并加速箭头
    /// </summary>
    public void ShootArrow()
    {
        if (arrow.GetComponent<Rigidbody2D>() == null)
        {
            issArrowShot = true;
            arrow.AddComponent<Rigidbody2D>();
            //设置父对象
            arrow.transform.parent = gameManager.transform;
            //
            Vector3 force = Quaternion.Euler(
                new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z)) * new Vector3(forceSize * length, 0,0);
            //给刚体添加力
            arrow.GetComponent<Rigidbody2D>().AddForce(force);
        }
        //if (arrow.GetComponent<Rigidbody>() == null)
        //{
        //    issArrowShot = true;
        //    arrow.AddComponent<Rigidbody>();
        //    arrow.transform.parent = gameManager.transform;
        //    arrow.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)) * new Vector3(25f * length, 0, 0), ForceMode.VelocityChange);
        //}
        //还原弓弦
        stringPullout = stringRestPosition;
    }
}
