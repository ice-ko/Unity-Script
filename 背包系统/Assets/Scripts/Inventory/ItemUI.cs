using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 物品ui信息
/// </summary>
public class ItemUI : MonoBehaviour
{
    public Item item;
    public int amount;//数量

    private Text amountText;
    private Image image;
    private void Awake()
    {
        amountText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
    }
    /// <summary>
    /// 设置itme信息
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_amount"></param>
    public void SetItem(Item _item, int _amount = 1)
    {
        item = _item;
        amount = _amount;
        if (item.Capacity > 1)
        {
            amountText.text = _amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
        image.sprite = Resources.Load<Sprite>(item.Sprite);
    }
    /// <summary>
    /// 增加数量
    /// </summary>
    /// <param name="_amount">数量</param>
    public void AddAmount(int _amount = 1)
    {
        amount += _amount;
        if (item.Capacity > 1)
        {
            amountText.text = _amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }
}
