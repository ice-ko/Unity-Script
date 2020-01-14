using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : CharacterBase
{
    /// <summary>
    /// 卡牌列表
    /// </summary>
    private List<CardCtrl> cardCtrlsList;
    /// <summary>
    /// 卡牌父对象
    /// </summary>
    private Transform cardParent;
    void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlsList = new List<CardCtrl>();

        Bind(CharacterEvent.Init_MyCard,CharacterEvent.Add_MyCard);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.Init_MyCard:
                StartCoroutine(InitCardList(message as List<CardDto>));
                break;
            case CharacterEvent.Add_MyCard:
                AddTableCard(message as GrabDto);
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
    private void AddTableCard(GrabDto dto)
    {
        List<CardDto> tableCards = dto.TableCardList;
        List<CardDto> playerCards = dto.PlayerCardList;
        //复用之前创建的卡牌
        int index = 0;
        foreach (var item in cardCtrlsList)
        {
            item.gameObject.SetActive(true);
            item.Init(playerCards[index], index, true);
            index++;
        }
        //新增三张卡牌
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = index; i < playerCards.Count; i++)
        {
            CreateGo(cardPrefab, playerCards[i], i);
        }
    }
    /// <summary>
    /// 协程延时一秒
    /// </summary>
    /// <param name="cardList"></param>
    /// <returns></returns>
    private IEnumerator InitCardList(List<CardDto> cardList)
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = 0; i < cardList.Count; i++)
        {
            CreateGo(cardPrefab, cardList[i], i);
            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    private void CreateGo(GameObject cardPrefab, CardDto card, int index)
    {
        GameObject cardGo = Instantiate(cardPrefab, cardParent);
        cardGo.name = card.Name;
        cardGo.transform.localPosition = new Vector2((0.25f * index), 0);
        CardCtrl cardCtrl = cardGo.GetComponent<CardCtrl>();
        cardCtrl.Init(card, index, true);
        //缓存本地
        cardCtrlsList.Add(cardCtrl);
    }
}
