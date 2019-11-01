using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : UIBase
{
    private InputField inputName;
    private Button btnSubmit;
    PromptMsg promptMsg;
    void Start()
    {
        promptMsg = new PromptMsg();

        inputName = transform.Find("input_Nickname").GetComponent<InputField>();
        btnSubmit = transform.Find("btn_Submit").GetComponent<Button>();

        btnSubmit.onClick.AddListener(OnSubmitClick);

        Bind(UIEvent.Create_Panel);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Create_Panel:
                SetPanelActive((bool)message);
                break;
        }
    }
    public override void OnDestroy()
    {
        btnSubmit.onClick.RemoveListener(OnSubmitClick);
    }
    private void OnSubmitClick()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            promptMsg.Text = "请输入您的名称";
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        //发送请求
        SocketMsg msg = new SocketMsg
        {
            OpCode = MsgType.User,
            SubCode = UserCode.CreateCharacterRequest,
            value = inputName.text
        };
        Dispatch(AreaCode.NET,0, msg);
    }

    void Update()
    {

    }
}
