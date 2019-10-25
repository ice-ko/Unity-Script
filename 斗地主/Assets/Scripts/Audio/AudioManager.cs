using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音模块管理器
/// </summary>
public class AudioManager : ManagerBase
{
    public const int PLAY_AUDIO = 0;

    public static AudioManager Instance = null;

    void Awake()
    {
        Instance = this;
    }

}
