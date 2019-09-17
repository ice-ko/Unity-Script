using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定期执行
/// </summary>
public class FunctionPeriodic
{

    /// <summary>
    /// 将动作挂钩到MonoBehaviour的类
    /// </summary>
    private class MonoBehaviourHook : MonoBehaviour
    {
        /// <summary>
        /// 在更新委托
        /// </summary>
        public Action OnUpdate;

        private void Update()
        {
            if (OnUpdate != null) OnUpdate();
        }

    }

    /// <summary>
    /// 保留对所有活动计时器的引用
    /// </summary>
    private static List<FunctionPeriodic> funcList;
    /// <summary>
    /// 用于初始化类的全局游戏对象在场景更改时被销毁
    /// </summary>
    private static GameObject initGameObject;

    /// <summary>
    /// 初始化
    /// </summary>
    private static void InitIfNeeded()
    {
        if (initGameObject == null)
        {
            initGameObject = new GameObject("FunctionPeriodic_Global");
            funcList = new List<FunctionPeriodic>();
        }
    }



    /// <summary>
    /// 创建通用场景对象
    /// </summary>
    /// <param name="action">委托</param>
    /// <param name="testDestroy">是否销毁对象委托</param>
    /// <param name="timer">时间</param>
    /// <returns></returns>
    public static FunctionPeriodic Create_Global(Action action, Func<bool> testDestroy, float timer)
    {
        FunctionPeriodic functionPeriodic = Create(action, testDestroy, timer, "", false, false, false);
        MonoBehaviour.DontDestroyOnLoad(functionPeriodic.gameObject);
        return functionPeriodic;
    }


    /// <summary>
    /// 创建指定时间[timer]触发委托[action]，在触发动作后执行[testDestroy]，如果返回true则销毁
    /// </summary>
    /// <param name="action">委托</param>
    /// <param name="testDestroy">是否销毁对象委托</param>
    /// <param name="timer">时间</param>
    /// <returns></returns>
    public static FunctionPeriodic Create(Action action, Func<bool> testDestroy, float timer)
    {
        return Create(action, testDestroy, timer, "", false);
    }
    /// <summary>
    /// 创建指定时间[timer]触发委托[action]
    /// </summary>
    /// <param name="action">委托</param>
    /// <param name="timer">时间</param>
    /// <returns></returns>
    public static FunctionPeriodic Create(Action action, float timer)
    {
        return Create(action, null, timer, "", false, false, false);
    }
    /// <summary>
    /// 创建指定时间[timer]触发委托[action]，在触发动作后执行[functionName]方法
    /// </summary>
    /// <param name="action">委托</param>
    /// <param name="timer">时间</param>
    /// <param name="functionName">方法名称</param>
    /// <returns></returns>
    public static FunctionPeriodic Create(Action action, float timer, string functionName)
    {
        return Create(action, null, timer, functionName, false, false, false);
    }
    /// <summary>
    /// 创建指定时间[timer]触发委托[action]，在触发动作后执行[testDestroy]，如果返回true则销毁
    /// </summary>
    /// <param name="callback">返回委托</param>
    /// <param name="testDestroy">是否销毁对象委托</param>
    /// <param name="timer">时间</param>
    /// <param name="functionName">方法名称</param>
    /// <param name="stopAllWithSameName">停止全部方法名称</param>
    /// <returns></returns>
    public static FunctionPeriodic Create(Action callback, Func<bool> testDestroy, float timer, string functionName, bool stopAllWithSameName)
    {
        return Create(callback, testDestroy, timer, functionName, false, false, stopAllWithSameName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="testDestroy"></param>
    /// <param name="timer"></param>
    /// <param name="functionName"></param>
    /// <param name="useUnscaledDeltaTime"></param>
    /// <param name="triggerImmediately"></param>
    /// <param name="stopAllWithSameName"></param>
    /// <returns></returns>
    public static FunctionPeriodic Create(Action action, Func<bool> testDestroy, float timer, string functionName, bool useUnscaledDeltaTime, bool triggerImmediately, bool stopAllWithSameName)
    {
        InitIfNeeded();

        if (stopAllWithSameName)
        {
            StopAllFunc(functionName);
        }

        GameObject gameObject = new GameObject("FunctionPeriodic Object " + functionName, typeof(MonoBehaviourHook));
        FunctionPeriodic functionPeriodic = new FunctionPeriodic(gameObject, action, timer, testDestroy, functionName, useUnscaledDeltaTime);
        gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionPeriodic.Update;

        funcList.Add(functionPeriodic);

        if (triggerImmediately) action();

        return functionPeriodic;
    }

    /// <summary>
    /// 移除计时器
    /// </summary>
    /// <param name="funcTimer"></param>
    public static void RemoveTimer(FunctionPeriodic funcTimer)
    {
        InitIfNeeded();
        funcList.Remove(funcTimer);
    }
    /// <summary>
    /// 停止计时器
    /// </summary>
    /// <param name="_name"></param>
    public static void StopTimer(string _name)
    {
        InitIfNeeded();
        for (int i = 0; i < funcList.Count; i++)
        {
            if (funcList[i].functionName == _name)
            {
                funcList[i].DestroySelf();
                return;
            }
        }
    }
    /// <summary>
    /// 停止所有Func
    /// </summary>
    /// <param name="_name"></param>
    public static void StopAllFunc(string _name)
    {
        InitIfNeeded();
        for (int i = 0; i < funcList.Count; i++)
        {
            if (funcList[i].functionName == _name)
            {
                funcList[i].DestroySelf();
                i--;
            }
        }
    }
    /// <summary>
    /// Func是否活跃
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool IsFuncActive(string name)
    {
        InitIfNeeded();
        for (int i = 0; i < funcList.Count; i++)
        {
            if (funcList[i].functionName == name)
            {
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// 游戏对象
    /// </summary>
    private GameObject gameObject;
    /// <summary>
    /// 时间
    /// </summary>
    private float timer;
    /// <summary>
    /// 基础时间
    /// </summary>
    private float baseTimer;
    /// <summary>
    /// 使用非标定增量时间
    /// </summary>
    private bool useUnscaledDeltaTime;
    /// <summary>
    /// 方法名称
    /// </summary>
    private string functionName;
    /// <summary>
    /// 委托
    /// </summary>
    public Action action;
    /// <summary>
    /// 是否销毁
    /// </summary>
    public Func<bool> testDestroy;


    private FunctionPeriodic(GameObject gameObject, Action action, float timer, Func<bool> testDestroy, string functionName, bool useUnscaledDeltaTime)
    {
        this.gameObject = gameObject;
        this.action = action;
        this.timer = timer;
        this.testDestroy = testDestroy;
        this.functionName = functionName;
        this.useUnscaledDeltaTime = useUnscaledDeltaTime;
        baseTimer = timer;
    }
    /// <summary>
    /// 跳过定时器
    /// </summary>
    /// <param name="timer"></param>
    public void SkipTimerTo(float timer)
    {
        this.timer = timer;
    }

    void Update()
    {
        if (useUnscaledDeltaTime)
        {
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            action();
            if (testDestroy != null && testDestroy())
            {
                //Destroy
                DestroySelf();
            }
            else
            {
                //Repeat
                timer += baseTimer;
            }
        }
    }
    /// <summary>
    /// 销毁自己
    /// </summary>
    public void DestroySelf()
    {
        RemoveTimer(this);
        if (gameObject != null)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
