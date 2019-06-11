using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public float smoothing = 1;//渐变平滑速度

    private float tatgetAlpha = 0;
    //
    private Text toolTipText;
    private Text contentText;
    private CanvasGroup canvasGroup;
    void Start()
    {
        toolTipText = GetComponent<Text>();
        contentText = transform.Find("ContentText").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (canvasGroup.alpha != tatgetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, tatgetAlpha, smoothing * Time.deltaTime);
            //是否到底目标值
            if (Mathf.Abs(canvasGroup.alpha - tatgetAlpha) < 0.01f)
            {
                canvasGroup.alpha = tatgetAlpha;
            }
        }
    }
    /// <summary>
    /// 显示提示框
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text)
    {
        toolTipText.text = text;
        contentText.text = text;
        tatgetAlpha = 1;
    }
    /// <summary>
    /// 隐藏提示框
    /// </summary>
    public void Hide()
    {
        tatgetAlpha = 0;
    }
}
