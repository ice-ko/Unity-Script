using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 矩形遮罩镂空
/// </summary>
public class RectGuidance : MonoBehaviour
{
    public static RectGuidance instance;
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
    /// 最终的偏移x
    /// </summary>
    private float targetOffsetX = 0;
    /// <summary>
    /// 最终的偏移y
    /// </summary>
    private float targetOffsetY = 0;
    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material material;
    /// <summary>
    /// 当前的偏移x
    /// </summary>
    private float currentOffsetX = 0f;
    /// <summary>
    /// 当前的偏移y
    /// </summary>
    private float currentOffsetY = 0f;
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
        //计算高亮显示区域在画布中的范围
        targetOffsetX = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[3])) / 2f;
        targetOffsetY = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[1])) / 2f;
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
            //计算偏移初始值
            for (int i = 0; i < corners.Length; i++)
            {
                if (i % 2 == 0)
                {
                    currentOffsetX = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corners[i]), center), currentOffsetX);
                }
                else
                {
                    currentOffsetY = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corners[i]), center), currentOffsetY);
                }
            }
        }
        //设置遮罩材质中当前偏移的变量
        material.SetFloat("_SliderX", currentOffsetX);
        material.SetFloat("_SliderY", currentOffsetY);
    }
    /// <summary>
    /// 收缩速度
    /// </summary>
    private float shrinkVelocityX = 0f;
    private float shrinkVelocityY = 0f;
    private void Update()
    {
        //从当前偏移量到目标偏移量差值显示收缩动画
        float valueX = Mathf.SmoothDamp(currentOffsetX, targetOffsetX, ref shrinkVelocityX, shrinkTime);
        float valueY = Mathf.SmoothDamp(currentOffsetY, targetOffsetY, ref shrinkVelocityY, shrinkTime);
        if (!Mathf.Approximately(valueX, currentOffsetX))
        {
            currentOffsetX = valueX;
            material.SetFloat("_SliderX", currentOffsetX);
        }
        if (!Mathf.Approximately(valueY, currentOffsetY))
        {
            currentOffsetY = valueY;
            material.SetFloat("_SliderY", currentOffsetY);
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
