using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //投掷进度条
    public GameObject progressBarCanvas;

    /// <summary>
    /// 投掷进度条显示/隐藏
    /// </summary>
    /// <param name="progressRate"></param>
    public void TrajectoryProgressBar(float progressRate)
    {
        if (progressRate > 0)
        {
            progressBarCanvas.SetActive(true);
        }
        else
        {
            progressBarCanvas.SetActive(false);
        }
        progressBarCanvas.transform.GetComponentInChildren<Slider>().value = progressRate;

    }


}
