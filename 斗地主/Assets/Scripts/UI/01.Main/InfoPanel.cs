using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBase
{
    private Text txt_Name;
    private Text txt_Lv;
    private Text txt_Beem;
    private Text txt_Exp;

    private Slider slider;

    void Start()
    {
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_Lv = transform.Find("txt_Lv").GetComponent<Text>();
        txt_Beem = transform.Find("txt_Beem").GetComponent<Text>();
        txt_Exp = transform.Find("Slider/Text").GetComponent<Text>();
        slider = transform.Find("Slider").GetComponent<Slider>();

        Bind(UIEvent.Refresh_Info_Panel);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Refresh_Info_Panel:
                UserCharacterDto userCharacter = message as UserCharacterDto;
                RefreshView(userCharacter);
                break;
        }
    }
    /// <summary>
    /// 刷新视图
    /// </summary>
    void RefreshView(UserCharacterDto info)
    {
        txt_Name.text = info.Name;
        txt_Lv.text = "Lv." + info.Lv;
        //等级和经验的公式：maxExp=lv*100
        txt_Exp.text = info.Exp + " / " + info.Lv * 100;
        txt_Beem.text = "x " + info.Been;

        slider.value = info.Exp / info.Lv * 100;

    }
}
