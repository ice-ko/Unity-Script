using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    private Transform panel;
    private Button btnSetting;
    private Button btnClose;
    private Button btnQuit;
    private Slider sldVolume;
    private Toggle togAudio;
    void Start()
    {
        panel = transform.Find("panel").GetComponent<Transform>();
        btnSetting = transform.Find("btn_Setting").GetComponent<Button>();
        btnClose = transform.Find("panel/btn_Close").GetComponent<Button>();
        btnQuit = transform.Find("panel/btn_Quit").GetComponent<Button>();
        sldVolume = transform.Find("panel/sld_Volume").GetComponent<Slider>();
        togAudio = transform.Find("panel/tog_Audio").GetComponent<Toggle>();

        btnSetting.onClick.AddListener(OnSettingClick);
        btnClose.onClick.AddListener(OnCloseClick);
        btnQuit.onClick.AddListener(OnQuitClick);
        sldVolume.onValueChanged.AddListener(OnVolumeChanged);
        togAudio.onValueChanged.AddListener(onAudioChanged);
        //
        panel.gameObject.SetActive(false);
    }
    /// <summary>
    /// 开关点击开启/关闭声音
    /// </summary>
    /// <param name="result"></param>
    private void onAudioChanged(bool result)
    {
        //TODO
    }
    /// <summary>
    /// 滑动条音量大小
    /// </summary>
    /// <param name="value"></param>
    private void OnVolumeChanged(float value)
    {
      //TODO
    }

    private void OnDestroy()
    {
        btnSetting.onClick.RemoveListener(OnSettingClick);
        btnClose.onClick.RemoveListener(OnCloseClick);
        btnQuit.onClick.RemoveListener(OnQuitClick);
        sldVolume.onValueChanged.RemoveListener(OnVolumeChanged);
        togAudio.onValueChanged.RemoveListener(onAudioChanged);
    }
    private void OnQuitClick()
    {
        Application.Quit();
    }

    private void OnCloseClick()
    {
        panel.gameObject.SetActive(false);
    }

    private void OnSettingClick()
    {
        panel.gameObject.SetActive(true);
    }

    void Update()
    {

    }
}
