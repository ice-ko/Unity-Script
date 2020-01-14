using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpPanel : UIBase
{
    private Image[] imgCard;
    void Start()
    {
        imgCard = new Image[3];
        imgCard[0] = transform.Find("img_1").GetComponent<Image>();
        imgCard[1] = transform.Find("img_2").GetComponent<Image>();
        imgCard[2] = transform.Find("img_3").GetComponent<Image>();
        Bind(UIEvent.Set_Cards);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Set_Cards:
                SetCards(message as List<CardDto>);
                break;
        }
    }
    void Update()
    {

    }
    /// <summary>
    /// 设置底牌
    /// </summary>
    /// <param name="cards"></param>
    void SetCards(List<CardDto> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            imgCard[i].sprite = Resources.Load<Sprite>("Poker/"+cards[i].Name);
        }
    }
}
