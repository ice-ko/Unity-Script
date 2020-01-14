using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EffectAudio : UIBase
{
    private AudioSource audioSource;
    private void Start()
    {
        Bind(UIEvent.EffectAudio);

        audioSource = GetComponent<AudioSource>();
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.EffectAudio:
                PlayChatEffectAudio(message.ToString());
                break;
        }
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">文件路径+名称</param>
    private void PlayChatEffectAudio(string name)
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/" + name);
        audioSource.Play();
    }
}