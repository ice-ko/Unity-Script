using UnityEngine;
using System.Collections;

public class AutomaticVerticalSize : MonoBehaviour
{
    //子物体高度
    public float childHeight = 35f;

    void Start()
    {
        AdjustSize();
    }
    /// <summary>
    /// 调整大小
    /// </summary>
    public void AdjustSize()
    {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
        size.y = this.transform.childCount * childHeight;
        this.GetComponent<RectTransform>().sizeDelta = size;
    }
}
