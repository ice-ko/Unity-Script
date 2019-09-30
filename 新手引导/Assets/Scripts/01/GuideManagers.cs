using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 新手引导管理
/// </summary>
public class GuideManagers : MonoBehaviour
{
    /// <summary>
    /// 引导步骤数组（如：第一步-》第二步。。。。）
    /// </summary>
    public List<GuideUIList> guideList = new List<GuideUIList>();
    /// <summary>
    /// 当前数组索引
    /// </summary>
    private int currentIndex = 0;
    /// <summary>
    /// 是否完成所有的新手引导
    /// </summary>
    private bool isFinish = false;
    /// <summary>
    /// 遮罩对象
    /// </summary>
    private GameObject maskPrefabs;
    /// <summary>
    /// 
    /// </summary>
    public void Next()
    {
        if (isFinish || currentIndex > guideList.Count)
        {
            return;
        }
        //注销上一步按钮点击事件
        if (currentIndex != 0 && guideList[currentIndex - 1].go.GetComponent<EventTriggerListener>() != null)
        {
            EventTriggerListener.GetListener(guideList[currentIndex - 1].go).onClick -= null;
        }

        if (maskPrefabs == null)
        {
            maskPrefabs = Instantiate(Resources.Load<GameObject>("RectGuidance_Panel"), this.transform);
        }
        //初始化遮罩
        maskPrefabs.GetComponent<RectGuidance>().Init(guideList[currentIndex].go.GetComponent<Image>()); ;

        currentIndex++;
        //给当前按钮添加点击事件
        if (currentIndex < guideList.Count)
        {
            EventTriggerListener.GetListener(guideList[currentIndex - 1].go).onClick += (go) =>
            {
                Next();
            };
        }
        //最后一个按钮点击事件处理
        else if (currentIndex == guideList.Count)
        {
            EventTriggerListener.GetListener(guideList[currentIndex - 1].go).onClick += (go) =>
            {
                maskPrefabs.gameObject.SetActive(false);
                //注销最后一个按钮的点击事件
                EventTriggerListener.GetListener(guideList[currentIndex - 1].go).onClick -= null;
            };
            isFinish = true;
        }
    }
}
/// <summary>
/// 引导ui数组
/// </summary>
[Serializable]
public class GuideUIList
{
    /// <summary>
    /// 引导步骤对象
    /// </summary>
    public GameObject go;

    public GuideUIList(GameObject go)
    {
        this.go = go;
    }
}

