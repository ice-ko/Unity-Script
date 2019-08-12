using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GoapAgent : MonoBehaviour, IAgent
{
    private HashSet<GoapAction> availableActions;
    private Queue<GoapAction> currentActions;

    private IGoap dataProvider;
    // this is the implementing class that provides our world data and listens to feedback on planning
    //这是实现类，它提供我们的世界数据并监听有关规划的反馈
    /// <summary>
    /// finds something to do
    /// 找到要做的事情
    /// </summary>
    private FSM.FSMState idleState;
    /// <summary>
    /// moves to a target
    /// 移动到目标
    /// </summary>
    private FSM.FSMState moveToState;
    /// <summary>
    /// performs an action
    /// 执行一个动作
    /// </summary>
    private FSM.FSMState performActionState;
    /// <summary>
    /// 规划人员
    /// </summary>
    private GoapPlanner planner;
    /// <summary>
    /// 状态机
    /// </summary>
    private FSM stateMachine;
    /// <summary>
    /// 中止状态机
    /// </summary>
    public void AbortFsm()
    {
        stateMachine.ClearState();
        stateMachine.pushState(idleState);
    }
    /// <summary>
    /// 添加动作
    /// </summary>
    /// <param name="a"></param>
    public void AddAction(GoapAction a)
    {
        availableActions.Add(a);
    }
    /// <summary>
    /// 获取动作
    /// </summary>
    /// <param name="action">动作类型</param>
    /// <returns></returns>
    public GoapAction GetAction(Type action)
    {
        foreach (var g in availableActions)
        {
            if (g.GetType().Equals(action))
                return g;
        }
        return null;
    }
    /// <summary>
    /// 移除动作
    /// </summary>
    /// <param name="action">动作</param>
    public void RemoveAction(GoapAction action)
    {
        availableActions.Remove(action);
    }
    /// <summary>
    /// 开始
    /// </summary>
    private void Start()
    {
        //开始 初始化
        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        //结束初始化
        FindDataProvider();
        CreateIdleState();
        CreateMoveToState();
        CreatePerformActionState();
        stateMachine.pushState(idleState);
        LoadActions();
    }

    private void Update()
    {
        stateMachine.Update(gameObject);
    }
    /// <summary>
    /// 检测是否有行动计划
    /// </summary>
    /// <returns></returns>
    private bool HasActionPlan()
    {
        return currentActions.Count > 0;
    }
    /// <summary>
    /// 创建空闲状态
    /// </summary>
    private void CreateIdleState()
    {
        idleState = (fsm, gameObj) =>
        {
            // GOAP planning
            // GOAP计划

            // get the world state and the goal we want to plan for
            //获得世界状态和我们想要计划的目标
            var worldState = dataProvider.GetWorldState();
            //创建目标状态
            var goals = dataProvider.CreateGoalState();

            // search enable Plan
            //搜索启用计划
            Queue<GoapAction> plan = null;
            KeyValuePair<string, bool> lastGoal = new KeyValuePair<string, bool>();
            foreach (var goal in goals)
            {
                lastGoal = goal;
                plan = planner.Plan(gameObject, availableActions, worldState, goal,dataProvider);
                if (plan != null)
                    break;
            }
            if (plan != null)
            {
                // we have a plan, hooray!
                //我们有个计划，万岁！
                currentActions = plan;
                dataProvider.PlanFound(lastGoal, plan);
                //移动到“执行操作”状态
                fsm.popState(); // move to PerformAction state
                fsm.pushState(performActionState);
            }
            else
            {
                // ugh, we couldn't get a plan
                //呃，我们无法得到一个计划
                Debug.Log("<color=orange>失败的计划:</color>" + PrettyPrint(goals));
                dataProvider.PlanFailed(goals);
                //返回空闲操作状态
                fsm.popState(); // move back to IdleAction state
                fsm.pushState(idleState);
            }
        };
    }
    /// <summary>
    /// 创建移动状态
    /// </summary>
    private void CreateMoveToState()
    {
        moveToState = (fsm, gameObj) =>
        {
            // move the game object
            //移动游戏对象
            var action = currentActions.Peek();
//            if (action.requiresInRange() && action.target == null)
//            {
//                Debug.Log(
//                    "<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
//                fsm.popState(); // move
//                fsm.popState(); // perform
//                fsm.pushState(idleState);
//                return;
//            }

            // get the agent to move itself
            if (dataProvider.MoveAgent(action))
            {
                fsm.popState();
            }

            /*MovableComponent movable = (MovableComponent) gameObj.GetComponent(typeof(MovableComponent));
			if (movable == null) {
				Debug.Log("<color=red>Fatal error:</color> Trying to move an Agent that doesn't have a MovableComponent. Please give it one.");
				fsm.popState(); // move
				fsm.popState(); // perform
				fsm.pushState(idleState);
				return;
			}

			float step = movable.moveSpeed * Time.deltaTime;
			gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, action.target.transform.position, step);

			if (gameObj.transform.position.Equals(action.target.transform.position) ) {
				// we are at the target location, we are done
				action.setInRange(true);
				fsm.popState();
			}*/
        };
    }
    /// <summary>
    /// 创建执行操作状态
    /// </summary>
    private void CreatePerformActionState()
    {
        performActionState = (fsm, gameObj) =>
        {
            // perform the action
            //执行动作
            if (!HasActionPlan())
            {
                // no actions to perform
                //没有要执行的操作
                Debug.Log("<color=red>Done actions</color>");
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.ActionsFinished();
                return;
            }

            var action = currentActions.Peek();
            if (action.IsDone())
            {
                // the action is done. Remove it so we can perform the next one
                //动作完成 删除它，以便我们可以执行下一个
                currentActions.Dequeue();
            }

            if (HasActionPlan())
            {
                // perform the next action
                //执行下一个操作
                action = currentActions.Peek();
                var inRange = action.RequiresInRange() ? action.IsInRange() : true;

                if (inRange)
                {
                    // we are in range, so perform the action
                    //我们在范围内，所以执行操作
                    var success = action.Perform(gameObj,dataProvider.GetBlackBoard());

                    if (!success)
                    {
                        // action failed, we need to plan again
                        //操作失败，我们需要再次计划
                        fsm.popState();
                        fsm.pushState(idleState);
                        dataProvider.PlanAborted(action);
                    }
                }
                else
                {
                    // we need to move there first
                    // push moveTo state
                    //我们需要先移动到那里
                    //推动移动到状态
                    fsm.pushState(moveToState);
                }
            }
            else
            {
                // no actions left, move to Plan state
                //没有动作，转移到计划状态
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.ActionsFinished();
            }
        };
    }
    /// <summary>
    /// 找数据提供者
    /// </summary>
    private void FindDataProvider()
    {
        foreach (var comp in gameObject.GetComponents(typeof (Component)))
        {
            if (typeof (IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap) comp;
                dataProvider.Agent = this;
                return;
            }
        }
    }
    /// <summary>
    /// 加载动作
    /// </summary>
    private void LoadActions()
    {
        var actions = gameObject.GetComponents<GoapAction>();
        foreach (var a in actions)
        {
            availableActions.Add(a);
        }
//        Debug.Log("Found actions: " + prettyPrint(actions));
    }
    /// <summary>
    /// 打印
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string PrettyPrint(HashSet<KeyValuePair<string, object>> state)
    {
        var s = "";
        foreach (var kvp in state)
        {
            s += kvp.Key + ":" + kvp.Value;
            s += ", ";
        }
        return s;
    }
    /// <summary>
    /// 打印队列
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static string PrettyPrint(Queue<GoapAction> actions)
    {
        var s = "";
        foreach (var a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }
    /// <summary>
    /// 打印字典
    /// </summary>
    /// <param name="goals"></param>
    /// <returns></returns>
    public static string PrettyPrint(Dictionary<string, bool> goals)
    {
        var s = "";
        foreach (var a in goals)
        {
            s += a.Key;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }
    /// <summary>
    /// 打印数组
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static string PrettyPrint(GoapAction[] actions)
    {
        var s = "";
        foreach (var a in actions)
        {
            s += a.GetType().Name;
            s += ", ";
        }
        return s;
    }
    /// <summary>
    /// 打印动作名称
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static string PrettyPrint(GoapAction action)
    {
        var s = "" + action.GetType().Name;
        return s;
    }
}