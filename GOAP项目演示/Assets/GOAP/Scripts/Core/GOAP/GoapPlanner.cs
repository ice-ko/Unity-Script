using System.Collections.Generic;
using UnityEngine;

/**
 * Plans what actions can be completed in order to fulfill a goal state.
 */

///<summary>
/// 计划可以完成哪些操作以实现目标状态。
///</summary>
public class GoapPlanner
{
    /**
	 * Plan what sequence of actions can fulfill the goal.
	 * Returns null if a plan could not be found, or a list of the actions
	 * that must be performed, in order, to fulfill the goal.
     * 计划可以实现目标的行动顺序。
     * 如果找不到计划，或者操作列表，则返回null
     * 必须按顺序执行才能实现目标。
	 */

    ///<summary>
    ///计划
    ///</summary>
    public Queue<GoapAction> Plan(GameObject agent,
        HashSet<GoapAction> availableActions,
        Dictionary<string, bool> worldState,
        KeyValuePair<string, bool> goal,
        IGoap goap)
    {
        // reset the actions so we can start fresh with them
        //重置动作，以便我们可以重新开始
        foreach (var a in availableActions)
        {
            a.doReset();
        }

        // check what actions can run using their checkProceduralPrecondition
        //使用检查程序前置条件检查可以运行的操作
        var usableActions = NodeManager.GetFreeActionSet();
        foreach (var a in availableActions)
        {
            if (a.CheckProceduralPrecondition(agent, goap.GetBlackBoard()))
                usableActions.Add(a);
        }

        // we now have all actions that can run, stored in usableActions
        //我们现在拥有可以运行的所有操作，存储在usefulActions中
        // build up the tree and record the leaf nodes that provide a solution to the goal.
        //构建树并记录提供目标解决方案的节点。
        var leaves = new List<GoapNode>();

        // build graph
        //构建图表
        var start = NodeManager.GetFreeNode(null, 0, 0, worldState, null);
        //构建图
        var success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            // oh no, we didn't get a plan
            //不，我们没有得到一个计划
            //            Debug.Log("NO PLAN");
            return null;
        }

        // get the cheapest leaf
        //得到最便宜的节点
        GoapNode cheapest = null;
        foreach (var leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.BetterThen(cheapest))
                    cheapest = leaf;
            }
        }

        // get its node and work back through the parents
        //获取其节点并通过父节点返回
        var result = new List<GoapAction>();
        var n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                //在前面插入动作
                result.Insert(0, n.action); // insert the action in the front
            }
            n = n.parent;
        }

        NodeManager.Release();
        // we now have this action list in correct order
        //我们现在按正确的顺序拥有此操作列表
        var queue = new Queue<GoapAction>();
        foreach (var a in result)
        {
            queue.Enqueue(a);
        }

        // hooray we have a plan!
        //万岁，我们有一个计划！
        return queue;
    }

    /**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest Cost will be the best action
	 * sequence.
	 */

    ///<summary>
    ///如果找到至少一个解，则返回true。
    ///可能的路径存储在叶子列表中。 每个节点都有一个
    ///runningCost 值，其中最低成本将是最佳操作
    ///序列。
    ///</summary>
    private bool BuildGraph(GoapNode parent, List<GoapNode> leaves
        , HashSet<GoapAction> usableActions, KeyValuePair<string, bool> goal)
    {
        var foundOne = false;

        // go through each action available at this node and see if we can use it here
        //浏览此节点上的每个可用操作，看看我们是否可以在此处使用它
        foreach (var action in usableActions)
        {
            // if the parent state has the conditions for this action's preconditions, we can use it here
            //如果父状态具有此操作的前置条件的条件，我们可以在此处使用它
            if (InState(action.Preconditions, parent.state))
            {
                // apply the action's effects to the parent state
                //将动作的效果应用于父状态
                var currentState = PopulateState(parent.state, action.Effects);
                //Debug.Log(GoapAgent.prettyPrint(currentState));
                var node = NodeManager.GetFreeNode(parent, parent.runningCost + action.GetCost(), parent.weight + action.GetWeight(),
                    currentState, action);

                //force child.precondition in parent.effects or child.precondition is empty.
                //在parent.effects中强制child.precondition或child.precondition为空。
                if (action.Preconditions.Count == 0 && parent.action != null ||
                    parent.action != null && !CondRelation(action.Preconditions, parent.action.Effects))
                    continue;

                if (FillGoal(goal, currentState))
                {
                    // we found a solution!
                    //我们找到了解决方案！
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    // not at a solution yet, so test all the remaining actions and branch out the tree
                    //还没有解决方案，所以测试所有剩余的动作并分支出树
                    var subset = ActionSubset(usableActions, action);
                    var found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }

    //if there is one true relationship
    //如果有一个真正的关系
    private bool CondRelation(Dictionary<string, bool> preconditions
                            , Dictionary<string, bool> effects)
    {
        foreach (var t in preconditions)
        {
            var match = effects.ContainsKey(t.Key) && effects[t.Key] == t.Value;
            if (match)
                return true;
        }
        return false;
    }

    /**
	 * Create a subset of the actions excluding the removeMe one. Creates a new set.
	 */
    ///<summary>
    ///创建除removeMe之外的操作子集。 创建一个新集。
    ///</summary>
    private HashSet<GoapAction> ActionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
    {
        var subset = NodeManager.GetFreeActionSet();
        foreach (var a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }

    /**
	 * Check that all items in 'test' are in 'state'. If just one does not match or is not there
	 * then this returns false.
	 */
    ///<summary>
    ///检查'test'中的所有项目是否处于'state'状态。 如果只有一个不匹配或不存在
    ///那么这会返回false。
    ///</summary>
    private bool InState(Dictionary<string, bool> test, Dictionary<string, bool> state)
    {
        var allMatch = true;
        foreach (var t in test)
        {
            var match = state.ContainsKey(t.Key) && state[t.Key] == t.Value;
            if (!match)
            {
                allMatch = false;
                break;
            }
        }
        return allMatch;
    }
    private bool FillGoal(KeyValuePair<string, bool> goal, Dictionary<string, bool> state)
    {
        var match = state.ContainsKey(goal.Key) && state[goal.Key] == goal.Value;
        return match;
    }

    /**
	 * Apply the stateChange to the currentState
	 */
    ///<summary>
    ///将状态更改应用于当前状态
    ///</summary>
    private Dictionary<string, bool> PopulateState(Dictionary<string, bool> currentState,
        Dictionary<string, bool> stateChange)
    {
        Dictionary<string, bool> state = NodeManager.GetFreeState();
        state.Clear();
        foreach (var s in currentState)
        {
            state.Add(s.Key, s.Value);
        }

        foreach (var change in stateChange)
        {
            // if the key exists in the current state, update the Value
            //如果密钥存在于当前状态，请更新Value
            if (state.ContainsKey(change.Key))
            {
                state[change.Key] = change.Value;
            }
            else
            {
                state.Add(change.Key, change.Value);
            }
        }
        return state;
    }

}