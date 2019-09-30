using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideManagers : MonoBehaviour
{
    public List<GuideUIList> guideList = new List<GuideUIList>();

    private int nowIndex = 0;
    private bool isFinish = false;//是否完成所有的新手引导
    private GameObject maskPrefabs;
    public void Next()
    {
        if (isFinish || nowIndex > guideList.Count)
        {
            return;
        }

        if (nowIndex != 0 && guideList[nowIndex - 1].go.GetComponent<EventTriggerListener>() != null)
        {
            EventTriggerListener.GetListener(guideList[nowIndex - 1].go).onClick -= null;
        }

        if (maskPrefabs == null)
        {
            maskPrefabs = Instantiate(Resources.Load<GameObject>("RectGuidance_Panel"), this.transform);
        }
        var rect = maskPrefabs.GetComponent<RectGuidance>();
        rect.Init(guideList[nowIndex].go.GetComponent<Image>());

        nowIndex++;

        if (nowIndex < guideList.Count)
        {
            EventTriggerListener.GetListener(guideList[nowIndex - 1].go).onClick += (go) =>
            {
                Next();
            };
        }
        else if (nowIndex == guideList.Count)
        {
            EventTriggerListener.GetListener(guideList[nowIndex - 1].go).onClick += (go) =>
            {
                maskPrefabs.gameObject.SetActive(false);
                EventTriggerListener.GetListener(guideList[nowIndex - 1].go).onClick -= null;
            };
            isFinish = true;
        }


    }
}
[Serializable]
public class GuideUIList
{
    public GameObject go;

    public GuideUIList(GameObject go)
    {
        this.go = go;
    }
}

