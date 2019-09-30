using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 圆形遮罩镂空
/// </summary>
public class CircleGuidance : MonoBehaviour
{
    public static CircleGuidance instance;
    /// <summary>
    /// 高亮显示目标
    /// </summary>
    public Image target;
    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] corners = new Vector3[4];
    /// <summary>
    /// 镂空区域中心
    /// </summary>
    private Vector4 center;
    /// <summary>
    /// 镂空区域半径
    /// </summary>
    private float radius;
    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material material;
    /// <summary>
    /// 当前高亮区域半径
    /// </summary>
    private float currentRadius;
    /// <summary>
    /// 高亮区域缩放的动画时间
    /// </summary>
    private float shrinkTime = 0.5f;
    /// <summary>
    /// 事件渗透组件
    /// </summary>
    private GuidanceEventPenetrate eventPenetrate;
    private void Awake()
    {
        instance = this;
    }
    public void Init(Image target)
    {
        this.target = target;
        eventPenetrate = GetComponent<GuidanceEventPenetrate>();
        if (eventPenetrate != null)
        {
            eventPenetrate.SetTargetImage(target);
        }
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //获取高亮区域的四个顶点的世界坐标
        target.rectTransform.GetWorldCorners(corners);
        //计算最终高亮显示区域的半径
        radius = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[2])) / 2;
        //计算高亮显示区域的中心
        float x = corners[0].x + ((corners[3].x - corners[0].x) / 2);
        float y = corners[0].y + ((corners[1].y - corners[0].y) / 2);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);
        //设置遮罩材质中的中心变量
        Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", centerMat);
        //计算当前高亮显示区域的半径
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if (canRectTransform != null)
        {
            //获取画布区域的四个顶点
            canRectTransform.GetWorldCorners(corners);
            //将画布顶点距离高亮区域中心最近的距离昨晚当前高亮区域半径的初始值
            foreach (var corner in corners)
            {
                currentRadius = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corner), corner), currentRadius);
            }
        }
        material.SetFloat("_Slider", currentRadius);
    }
    /// <summary>
    /// 收缩速度
    /// </summary>
    private float shrinkVelocity = 0f;
    private void Update()
    {
        //从当前半径到目标半径差值显示收缩动画
        float value = Mathf.SmoothDamp(currentRadius, radius, ref shrinkVelocity, shrinkTime);
        if (!Mathf.Approximately(value, currentRadius))
        {
            currentRadius = value;
            material.SetFloat("_Slider", currentRadius);
        }
    }

    /// <summary>
    /// 世界坐标转换为画布坐标
    /// </summary>
    /// <param name="canvas">画布</param>
    /// <param name="world">世界坐标</param>
    /// <returns></returns>
    private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out position);
        return position;
    }
}
