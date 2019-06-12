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
    private float targetScale = 1f;
    private Vector3 animationScale = new Vector3(1.4f, 1.4f, 1.4f);
    private float smoothing = 4;
    private void Awake()
    {
        amountText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
    }
    private void Update()
    {
        if (transform.localScale.x != targetScale)
        {
            float scale = Mathf.Lerp(transform.localScale.x, targetScale, smoothing * Time.deltaTime);
            transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Abs(transform.localScale.x - targetScale) < 0.02f)
            {
                transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            }
        }
    }
    /// <summary>
    /// 设置itme信息
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_amount"></param>
    public void SetItem(Item _item, int _amount = 1)
    {
        transform.localScale = animationScale;
        item = _item;
        amount = _amount;
        if (item.Capacity > 1)
        {
            amountText.text = amount.ToString();
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
        transform.localScale = animationScale;
        amount += _amount;
        if (item.Capacity > 1)
        {
            amountText.text = amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }
    /// <summary>
    /// 刷新物品显示文本
    /// </summary>
    public void RefreshAmount()
    {
        amountText.text = amount.ToString();
    }
}
