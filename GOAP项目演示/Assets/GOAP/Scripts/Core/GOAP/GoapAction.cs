
using UnityEngine;
using System.Collections.Generic;

public abstract class GoapAction : MonoBehaviour
{

    /// <summary>
    /// 前提条件
    /// </summary>
    private Dictionary<string, bool> preconditions;
    /// <summary>
    /// 效果
    /// </summary>
    private Dictionary<string, bool> effects;

    private bool inRange = false;

    /* The Cost of performing the action. 
	 * Figure out a weight that suits the action. 
	 * Changing it will affect what actions are chosen during planning.
     *执行操作的成本。
     *找出适合行动的重量。
     *更改它会影响规划期间选择的操作。*/
    /// <summary>
    /// 成本
    /// </summary>
    public float Cost = 1f;
    /// <summary>
    /// 获得成本
    /// </summary>
    /// <returns></returns>
    public virtual float GetCost()
    {
        return Cost;
    }

    /* The risk of performing the action. */
    /// <summary>
    /// 执行操作的风险。
    /// </summary>
    public float Risk = 0f;
    /* The Benefits of performing the action. */
    /// <summary>
    /// 执行操作的好处。
    /// </summary>
    public float Return = 1f;
    /* Figure out a weight that suits the action. */
    /// <summary>
    /// 找出适合行动的重量。
    /// </summary>
    /// <returns></returns>
    public virtual float GetWeight()
    {
        return (1 - Risk) * Return;
    }

    /**
	 * An action often has to perform on an object. This is that object. Can be null. */
    ///<summary>
    ///通常必须对对象执行操作。 这是那个对象。 可以为null。
    ///</summary>
    public GameObject target;
    /// <summary>
    /// Goap动作
    /// </summary>
    public GoapAction()
    {
        preconditions = new Dictionary<string, bool>();
        effects = new Dictionary<string, bool>();
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void doReset()
    {
        inRange = false;
        target = null;
        Reset();
    }

    /// <summary>
    /// 获取目标坐标
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 GetTargetPos()
    {
        return target.transform.position;
    }

    /**
	 * Reset any variables that need to be reset before planning happens again.
	 */
    ///<summary>
    ///在计划再次发生之前，重置需要重置的所有变量。
    ///</summary>
    public abstract void Reset();

    /**
	 * Is the action done?
	 */

    ///<summary>
    ///行动完成了吗？
    ///</summary>
    /// <inheritdoc cref="GoapAction"/>
    public abstract bool IsDone();

    /**
	 * Procedurally check if this action can run. Not all actions
	 * will need this, but some might.
	 */

    ///<summary>
    ///程序检查此操作是否可以运行。 并非所有行动
    ///需要这个，但有些可能。
    ///</summary>
    /// <inheritdoc/>
    public abstract bool CheckProceduralPrecondition(GameObject agent, BlackBoard bb);

    /**
	 * Run the action.
	 * Returns True if the action performed successfully or false
	 * if something happened and it can no longer perform. In this case
	 * the action queue should clear out and the goal cannot be reached.
	 */
    ///<summary>
    ///执行
    ///如果操作成功则返回True,否则返回false，
    ///如果发生了什么事，它就再也无法执行了。 在这种情况下
    ///行动队列应清除，无法达到目标。
    ///</summary>
    public abstract bool Perform(GameObject agent, BlackBoard bb);

    /**
	 * Does this action need to be within range of a target game object?
	 * If not then the moveTo state will not need to run for this action.
	 */
    ///<summary>
    ///此操作是否需要在目标游戏对象的范围内？
    ///如果不是，则moveTo状态不需要为此操作运行。
    ///</summary>
    public abstract bool RequiresInRange();


    /**
	 * Are we in range of the target?
	 * The MoveTo state will set this and it gets reset each time this action is performed.
	 */
    ///<summary>
    ///我们是否在目标范围内？
    ///MoveTo状态将设置此值，并在每次执行此操作时重置。
    ///</summary>
    public bool IsInRange()
    {
        return inRange;
    }
    /// <summary>
    /// 设置范围
    /// </summary>
    /// <param name="inRange"></param>
    public void SetInRange(bool inRange)
    {
        this.inRange = inRange;
    }
    /// <summary>
    /// 添加前置条件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddPrecondition(string key, bool value)
    {
        preconditions.Add(key, value);
    }
    /// <summary>
    /// 删除前置条件
    /// </summary>
    /// <param name="key"></param>
    public void RemovePrecondition(string key)
    {
        if (preconditions.ContainsKey(key))
            preconditions.Remove(key);
    }
    /// <summary>
    /// 添加效果
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddEffect(string key, bool value)
    {
        effects.Add(key, value);
    }
    /// <summary>
    /// 移除效果
    /// </summary>
    /// <param name="key"></param>
    public void RemoveEffect(string key)
    {
        if (effects.ContainsKey(key))
            effects.Remove(key);
    }
    /// <summary>
    /// 前提条件
    /// </summary>
    public Dictionary<string, bool> Preconditions
    {
        get
        {
            return preconditions;
        }
    }
    /// <summary>
    /// 效果
    /// </summary>
    public Dictionary<string, bool> Effects
    {
        get
        {
            return effects;
        }
    }
}