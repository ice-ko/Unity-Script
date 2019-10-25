using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanel : UIBase
{
    private Text txt;
    private CanvasGroup canvasGroup;
    /// <summary>
    /// 显示隐藏时间
    /// </summary>
    [Range(0, 3)]
    public float showTime = 1f;
    private float timer = 0f;
    void Start()
    {
        txt = transform.Find("bg/Text").GetComponent<Text>();
        canvasGroup = transform.Find("bg").GetComponent<CanvasGroup>();

        Bind(UIEvent.Prompt_Msg);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Prompt_Msg:
                PromptMsg msg = message as PromptMsg;
                PromptMessage(msg); break;
        }
    }
    /// <summary>
    /// 提示消息
    /// </summary>
    /// <param name="msg"></param>
    void PromptMessage(PromptMsg msg)
    {
        txt.text = msg.Text;
       // txt.color = msg.color;
        canvasGroup.alpha = 0;
        timer = 0;
        StartCoroutine(PromptAnim());
    }
    /// <summary>
    /// 显示动画
    /// </summary>
    /// <returns></returns>
    IEnumerator PromptAnim()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (timer < showTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
