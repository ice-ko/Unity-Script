using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayPanel : MonoBehaviour
{
    public StartPanel startPanel;
    public RegistPanel registPanel;

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
    public  void OnDestroy()
    {
       
        btnStart.onClick.RemoveAllListeners();
        btnRegist.onClick.RemoveAllListeners();
    }
    void OnStartClick()
    {
        startPanel.gameObject.SetActive(true);
    }
    void OnRegistClick()
    {
        registPanel.gameObject.SetActive(true);
    }
}
