using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPlayerCtrl : CharacterBase
{

    /// <summary>
    /// 卡牌父对象
    /// </summary>
    private Transform cardParent;
    void Start()
    {
        cardParent = transform.Find("CardPoint");
        Bind(CharacterEvent.Init_LeftCard, CharacterEvent.Add_LeftCard);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.Init_LeftCard:
                StartCoroutine(InitCardList());
                break;
            case CharacterEvent.Add_LeftCard:
                AddTableCard();
                break;
        }
    }
    void Update()
    {

    }
    /// <summary>
    /// 添加底牌
    /// </summary>
    /// <param name="cardList"></param>
    private void AddTableCard()
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/OtherCard");
        for (int i = 0; i < 3; i++)
        {
            CreateGo(cardPrefab, i);
        }
    }
    /// <summary>
    /// 协程延时一秒
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitCardList()
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/OtherCard");
        for (int i = 0; i < 17; i++)
        {
            CreateGo(cardPrefab, i);
            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    /// <param name="cardPrefab"></param>
    /// <param name="index"></param>
    private void CreateGo(GameObject cardPrefab, int index)
    {
        GameObject cardGo = Instantiate(cardPrefab, cardParent);
        cardGo.transform.localPosition = new Vector2((0.15f * index), 0);
        cardGo.GetComponent<SpriteRenderer>().sortingOrder = index;
    }
}