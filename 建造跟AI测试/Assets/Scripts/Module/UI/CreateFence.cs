using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFence : MonoBehaviour
{
    /// <summary>
    /// 跟随鼠标移动
    /// </summary>
    public Image followMoveSlot;
    public TileType type;

    void Start()
    {

    }

    /// <summary>
    /// 创建tile
    /// </summary>
    public void OnCreateTile()
    {
        followMoveSlot.gameObject.SetActive(true);
        followMoveSlot.transform.position = Utility.GetWorldToScreenPos();
        followMoveSlot.sprite =transform.GetChild(0).GetComponent<Image>().sprite;
        followMoveSlot.GetComponent<BuildController>().type = type;
        followMoveSlot.GetComponent<BuildController>().followMoveSlot = followMoveSlot;
    }
}
/// <summary>
/// tile类型
/// </summary>
public enum TileType
{
    /// <summary>
    /// 栅栏
    /// </summary>
    Fence,
}
