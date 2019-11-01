using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : ManagerBase
{
    public static SceneManagers Instance;

    /// <summary>
    /// 加载场景委托
    /// </summary>
    private Action onSceneLoadAction;
    private void Awake()
    {
        Instance = this;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }



    void Start()
    {
        Add(UIEvent.Scene, this);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Scene:
                LoadSceneMsg msg = message as LoadSceneMsg;
                LoadScene(msg);
                break;
        }
    }
    void Update()
    {

    }
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneIndex"></param>
    private void LoadScene(LoadSceneMsg msg)
    {
        if (msg.SceneIndex != -1)
        {
            SceneManager.LoadScene(msg.SceneIndex);
        }
        if (!string.IsNullOrEmpty(msg.SceneName))
        {
            SceneManager.LoadScene(msg.SceneName);
        }
        if (msg.OnSceneLoaded != null)
        {
            onSceneLoadAction = msg.OnSceneLoaded;
        }
    }
    /// <summary>
    /// 当场景加载完成调用
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        onSceneLoadAction?.Invoke();
    }
}
