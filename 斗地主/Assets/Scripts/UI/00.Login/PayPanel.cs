using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayPanel : UIBase
{
    private Button btnStart;
    private Button btnRegist;
    void Start()
    {
        btnStart = transform.Find("btn_Start").GetComponent<Button>();
        btnRegist = transform.Find("btn_Regist").GetComponent<Button>();
        //注册事件
        btnStart.onClick.AddListener(OnStartClick);
        btnRegist.onClick.AddListener(OnRegistClick);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();

        btnStart.onClick.RemoveAllListeners();
        btnRegist.onClick.RemoveAllListeners();
    }
    void OnStartClick()
    {
        Dispatch(AreaCode.UI,UIEvent.Start_Code,true);
    }
    void OnRegistClick()
    {
        Dispatch(AreaCode.UI, UIEvent.Regist_Code, true);
    }
}
