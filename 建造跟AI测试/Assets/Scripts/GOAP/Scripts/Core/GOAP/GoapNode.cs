using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/**
 * Used for building up the graph and holding the running costs of actions.
 */
///<summary>
/// 用于建立图表并保存操作的运行成本。
///</summary>
public class GoapNode
{
    /// <summary>
    /// 最大Id
    /// </summary>
    private static int MaxID;
    public int ID;
    /// <summary>
    /// 动作
    /// </summary>
    public GoapAction action;
    /// <summary>
    /// 父级
    /// </summary>
    public GoapNode parent;
    /// <summary>
    /// 运行成本
    /// </summary>
    public float runningCost;
    /// <summary>
    /// 状态
    /// </summary>
    public Dictionary<string, bool> state;
    /// <summary>
    /// 重量
    /// </summary>
    public float weight;

    public GoapNode(GoapNode parent, float runningCost, float weight, Dictionary<string, bool> state,
        GoapAction action)
    {
        ID = MaxID++;
        ReInit(parent, runningCost, weight, state, action);
    }
    /// <summary>
    /// 重新初始化
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="runningCost"></param>
    /// <param name="weight"></param>
    /// <param name="state"></param>
    /// <param name="action"></param>
    public void ReInit(GoapNode parent, float runningCost, float weight, Dictionary<string, bool> state,
        GoapAction action)
    {
        Clear();
        this.parent = parent;
        this.runningCost = runningCost;
        this.weight = weight;
        this.state = state;
        this.action = action;
    }
    /// <summary>
    /// 清楚
    /// </summary>
    private void Clear()
    {
        this.parent = null;
        this.runningCost = 0;
        this.weight = 0;
        this.state = null;
        this.action = null;
    }

    /// <summary>
    ///     compare node
    ///     比较节点
    /// </summary>
    /// <param name="cheapest"></param>
    /// <returns></returns>
    public bool BetterThen(GoapNode rh)
    {
        //            return runningCost < rh.runningCost;
        if (weight > rh.weight && runningCost < rh.runningCost)
            return true;
        if (weight < rh.weight && runningCost > rh.runningCost)
            return false;
        //make weight > cost
        var better = (weight / rh.weight - 1) >= (runningCost / rh.runningCost - 1);
        return better;
    }
}

public class NodeManager
{
    static Stack<GoapNode> _usedNodes = new Stack<GoapNode>();
    static Stack<GoapNode> _freeNodes = new Stack<GoapNode>();
    /// <summary>
    /// 获得免费节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="runningCost"></param>
    /// <param name="weight"></param>
    /// <param name="state"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static GoapNode GetFreeNode(GoapNode parent, float runningCost, float weight, Dictionary<string, bool> state,
        GoapAction action)
    {
        GoapNode free = null;
        if(_freeNodes.Count <= 0)
            free= new GoapNode(parent,runningCost,weight,state,action);
        else
        {
            free = _freeNodes.Pop();
            free.ReInit(parent, runningCost, weight, state, action);
        }

        _usedNodes.Push(free);
        return free;
    }
    /// <summary>
    /// 发布节点
    /// </summary>
    public static void ReleaseNode()
    {
        while (_usedNodes.Count > 0)
        {
            _freeNodes.Push(_usedNodes.Pop());
        }
    }
    static Stack<Dictionary<string, bool>> _usedState = new Stack<Dictionary<string, bool>>();
    static Stack<Dictionary<string, bool>> _freeState = new Stack<Dictionary<string, bool>>();
    /// <summary>
    /// 获取自由状态
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, bool> GetFreeState()
    {
        Dictionary<string, bool> free = null;
        if(_freeState.Count>0)
            free= _freeState.Pop();
        else
            free = new Dictionary<string, bool>();

        _usedState.Push(free);
        return free;
    }
    /// <summary>
    /// 释放状态
    /// </summary>
    private static void ReleaseState()
    {
        while (_usedState.Count > 0)
        {
            _freeState.Push(_usedState.Pop());
        }
    }

    static Stack<HashSet<GoapAction>> _usedSubset = new Stack<HashSet<GoapAction>>();
    static Stack<HashSet<GoapAction>> _freeSubset = new Stack<HashSet<GoapAction>>();
    /// <summary>
    /// GetFreeActionSet
    /// </summary>
    /// <returns></returns>
    public static HashSet<GoapAction> GetFreeActionSet()
    {
        HashSet<GoapAction> free = null;
        if (_freeSubset.Count > 0)
        {
            free = _freeSubset.Pop();
            free.Clear();
        }
        else
            free = new HashSet<GoapAction>();

        _usedSubset.Push(free);
        return free;
    }
    /// <summary>
    /// 释放子集
    /// </summary>
    private static void ReleaseSubset()
    {
        while (_usedSubset.Count > 0)
        {
            _freeSubset.Push(_usedSubset.Pop());
        }
    }
    /// <summary>
    /// 释放
    /// </summary>
    public static void Release()
    {
        ReleaseNode();
        ReleaseState();
        ReleaseSubset();
    }
}
