using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌控制类
/// </summary>
public class CardCtrl : MonoBehaviour
{
    public CardDto cardDto;
    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool isSelect;
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// 是否自身的卡牌
    /// </summary>
    private bool isMine;
    void Start()
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="cardDto">卡牌信息</param>
    /// <param name="index">索引</param>
    /// <param name="isMine">是否自己的卡牌</param>
    public void Init(CardDto cardDto, int index, bool isMine)
    {
        this.cardDto = cardDto;
        this.isMine = isMine;
        if (isSelect)
        {
            isSelect = false;
            transform.localPosition -= new Vector3(0, 0.3f, 0);
        }
        string resPath = string.Empty;
        if (isMine)
        {
            resPath = "Poker/" + cardDto.Name;
        }
        else
        {
            resPath = "Poker/CardBack";
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = index++;
        spriteRenderer.sprite = Resources.Load<Sprite>(resPath);
    }

    void Update()
    {
    }
    private void OnMouseDown()
    {
        if (isMine == false)
        {
            return;
        }
        isSelect = !isSelect;
        if (isSelect)
        {
            transform.localPosition += new Vector3(0, 0.3f, 0);
        }
        else
        {
            transform.localPosition -= new Vector3(0, 0.3f, 0);
        }
    }
    /// <summary>
    /// 选择状态
    /// </summary>
    public void SelectState()
    {
        if (isSelect == false)
        {
            isSelect = true;
            transform.localPosition += new Vector3(0, 0.3f, 0);
        }

    }
}
