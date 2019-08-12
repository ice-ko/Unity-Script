using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

/// <summary>
/// 目标
/// </summary>
public class CharacterGoals
{
    /// <summary>
    /// 填补饥饿
    /// </summary>
    public static string FillHunger = "FillHunger";
    /// <summary>
    /// 填充心灵
    /// </summary>
    public static string FillMind = "FillMind";
    /// <summary>
    /// 其他
    /// </summary>
    public static string FillOther = "FillOther";
}
/// <summary>
/// 人员状态管理
/// </summary>
public class CharacterStatus : MonoBehaviour, ICharacterStatus
{
    /// <summary>
    /// 饥饿值
    /// </summary>
    public int Hunger = 100;
    /// <summary>
    /// 心神值
    /// </summary>
    public int Mind = 100;
    public void Init()
    {
        _goalsWeight.Add(CharacterGoals.FillHunger, 0);
        _goalsWeight.Add(CharacterGoals.FillMind, 0);
        _goalsWeight.Add(CharacterGoals.FillOther, 0);
    }
    /// <summary>
    /// 费用时间
    /// </summary>
    private float _costTime = 0;
    /// <summary>
    /// 消耗
    /// </summary>
    /// <param name="goap"></param>
    public void Tick(IGoap goap)
    {
        _costTime += Time.deltaTime;
        if (_costTime >= 1)
        {
            Hunger -= 2;
            Mind -= 2;
            _costTime = 0;
        }
    }
    /// <summary>
    /// 移除
    /// </summary>
    public void Release()
    {

    }
    /// <summary>
    /// 目标重量
    /// </summary>
    readonly Dictionary<string, int> _goalsWeight = new Dictionary<string, int>();
    /// <summary>
    /// 排序标签
    /// </summary>
    readonly Dictionary<string, bool> _sortedTags = new Dictionary<string, bool>();
    /// <summary>
    /// 下一个目标
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, bool> NextGoal()
    {
        _goalsWeight[Goals.FillHunger] = GetHungerWeight();
        _goalsWeight[Goals.FillMind] = GetMindWeight();
        _goalsWeight[Goals.FillOther] = GetOtherWeight();

        var items = from pair in _goalsWeight
                    orderby pair.Value descending
                    select pair;

        _sortedTags.Clear();
        foreach (KeyValuePair<string, int> pair in items)
        {
            _sortedTags.Add(pair.Key,true); 
        }
        return _sortedTags;
    }
    /// <summary>
    /// 获得其他重量
    /// </summary>
    /// <returns></returns>
    private int GetOtherWeight()
    {
        return Normal;
    }
    /// <summary>
    /// 获得心灵重量
    /// </summary>
    /// <returns></returns>
    private int GetMindWeight()
    {
        if (Mind < 30)
            return Height;
        else if (Mind < 60)
            return Normal;
        else
            return Low;
    }
    /// <summary>
    /// 低
    /// </summary>
    private int Low = 1;
    /// <summary>
    /// 正常
    /// </summary>
    private int Normal = 2;
    /// <summary>
    /// 高 
    /// </summary>
    private int Height = 3;
    /// <summary>
    /// 获得饥饿感
    /// </summary>
    /// <returns></returns>
    private int GetHungerWeight()
    {
        if (Hunger < 30)
            return Height;
        else if (Hunger < 60)
            return Normal;
        else
            return Low;
    }
}
