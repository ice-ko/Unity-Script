using UnityEngine;
using System.Collections;

/**
 * Collect the world data for this Agent that will be
 * used for GOAP planning.
 */
using System.Collections.Generic;


/**
 * Any agent that wants to use GOAP must implement
 * this interface. It provides information to the GOAP
 * planner so it can plan what actions to use.
 * 
 * It also provides an interface for the planner to give 
 * feedback to the Agent and report success/failure.
 * 任何想要使用GOAP的代理都必须实现
 * 这个界面。 它向GOAP提供信息
 * 计划员，以便它可以计划使用什么行动。
 * 它还为计划者提供了一个界面
 * 反馈给代理并报告成功/失败。
 */
public interface IGoap
{
    /**
	 * The starting state of the Agent and the world.
	 * Supply what states are needed for actions to run.
     * 代理人和世界的起始状态。
     * 提供运行操作所需的状态
	 */
    Dictionary<string, bool> GetWorldState();

    /**
	 * Give the planner a new goal so it can figure out 
	 * the actions needed to fulfill it.
     * 给计划者一个新的目标，这样它就可以搞清楚
     * 实现它所需的行动。
	 */
    Dictionary<string, bool> CreateGoalState();


    /**
     * Get blackboard for environment
     * 获取环境黑板
     */
    BlackBoard GetBlackBoard();

    /**
	 * No sequence of actions could be found for the supplied goal.
	 * You will need to try another goal
     * 无法找到所提供目标的操作序列。
     * 您需要尝试另一个目标
	 */
    void PlanFailed(Dictionary<string, bool> failedGoal);

    /**
	 * A plan was found for the supplied goal.
	 * These are the actions the Agent will perform, in order.
     * 找到了提供目标的计划。
     * 这些是代理将按顺序执行的操作。
	 */
    void PlanFound(KeyValuePair<string, bool> goal, Queue<GoapAction> actions);

    /**
	 * All actions are complete and the goal was reached. Hooray!
	 * 所有操作都已完成，并且已达到目标。万岁！
     */
    void ActionsFinished();

    /**
	 * One of the actions caused the plan to abort.
	 * That action is returned.
     * 其中一项行动导致计划中止。
     * 该行动被退回。
	 */
    void PlanAborted(GoapAction aborter);

    /**
	 * Called during Update. Move the agent towards the target in order
	 * for the next action to be able to perform.
	 * Return true if the Agent is at the target and the next action can perform.
	 * False if it is not there yet.
     * 在更新期间调用。 按顺序将代理移动到目标
     * 用于下一步能够执行的操作。
     * 如果代理位于目标并且下一个操作可以执行，则返回true。
     * 如果还没有，那就错了。
	 */
    bool MoveAgent(GoapAction nextAction);
    /// <summary>
    /// 初始化
    /// </summary>
    void Init();
    /// <summary>
    /// 刷新时间
    /// </summary>
    void Tick();
    /// <summary>
    /// 发布
    /// </summary>
    void Release();

    /// <summary>
    /// save agent instance
    /// 保存代理实例
    /// </summary>
    IAgent Agent { get; set; }
}

