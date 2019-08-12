using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 * A general labourer class.
 * You should subclass this for specific Labourer classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 */

///<summary>
///普通劳动者阶级。
///您应该为特定的Laborer类子类化并实现它
///创建目标状态方法，该方法将填充GOAP的目标
///规划师。
///</summary>
public abstract class Labourer : MonoBehaviour, IGoap
{
    /// <summary>
    /// 临时背包
    /// </summary>
    public BackpackComponent backpack;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed = 1;
    /// <summary>
    /// 是否启用日志
    /// </summary>
    public bool EnableLog = false;

    void Start()
    {
        Init();
    }


    void Update()
    {
        Tick();
    }
    /// <summary>
    /// 世界数据
    /// </summary>
    Dictionary<string, bool> worldData = new Dictionary<string, bool>();
    /**
	 * Key-Value data that will feed the GOAP actions and system while planning.
     * 在规划时将为GOAP操作和系统提供信息的键值数据。
	 */
    public Dictionary<string, bool> GetWorldState()
    {
        worldData["hasOre"] = backpack.numOre > 0;
        worldData["hasLogs"] = backpack.numLogs > 0;
        worldData["hasFirewood"] = backpack.numFirewood > 0;
        worldData["hasTool"] = backpack.tool != null;
        worldData["hasMeat"] = backpack.numMeat > 0;

        return worldData;
    }

    BlackBoard bb = new BlackBoard();
    /// <summary>
    /// 获得黑板
    /// </summary>
    /// <returns></returns>
    public BlackBoard GetBlackBoard()
    {
        return bb;
    }

    /**
	 * Implement in subclasses
     * 在子类中实现
	 */
    public abstract Dictionary<string, bool> CreateGoalState();


    public void PlanFailed(Dictionary<string, bool> failedGoal)
    {
        // Not handling this here since we are making sure our goals will always succeed.
        // But normally you want to make sure the world state has changed before running
        // the same goal again, or else it will just fail.
        //因为我们确保我们的目标永远成功，所以不在此处理。
        //但通常你想在运行之前确保世界状态已经改变
        //再次使用相同的目标，否则它将失败。
    }
    /// <summary>
    /// 计划发现
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="actions"></param>
    public void PlanFound(KeyValuePair<string, bool> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal
        //我们为我们的目标找到了一个计划
        if (EnableLog)
            Debug.Log("<color=green> 计划发现</color> " + GoapAgent.PrettyPrint(actions));
    }
    /// <summary>
    /// 动作完成
    /// </summary>
    public void ActionsFinished()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        //一切都完成了，我们为这个gool完成了我们的行动。万岁！
        if (EnableLog)
            Debug.Log("<color=blue>动作完成</color>");
    }
    /// <summary>
    /// 计划中止
    /// </summary>
    /// <param name="aborter"></param>
    public void PlanAborted(GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        //一项保护计划的行动。 国家已经重置计划了。
        //记下发生的事情并确保再次运行相同的目标
        //它可以成功
        if (EnableLog)
            Debug.Log("<color=red>计划中止</color> " + GoapAgent.PrettyPrint(aborter));
    }
    /// <summary>
    /// 移动代理
    /// </summary>
    /// <param name="nextAction">下一个动作</param>
    /// <returns></returns>
    public bool MoveAgent(GoapAction nextAction)
    {
        //移向>下一个动作的目标
        float step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);

        if (gameObject.transform.position.Equals(nextAction.target.transform.position))
        {
            // we are at the target location, we are done
            //我们在目标位置，我们完成了
            nextAction.SetInRange(true);
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        if (backpack == null)
            backpack = gameObject.AddComponent<BackpackComponent>() as BackpackComponent;
        if (backpack.tool == null)
        {
            //加载工具
            GameObject prefab = Resources.Load<GameObject>(backpack.toolType);
            //实例化
            GameObject tool = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            backpack.tool = tool;
            // attach the tool
            //附上工具
            tool.transform.parent = transform; 
        }

        if (Brain == null)
            Brain = gameObject.GetComponent<Brain>();
        Brain.Init();

        //init world data 初始化世界数据
        worldData.Add("hasOre", (backpack.numOre > 0));
        worldData.Add("hasLogs", (backpack.numLogs > 0));
        worldData.Add("hasFirewood", (backpack.numFirewood > 0));
        worldData.Add("hasTool", (backpack.tool != null));
        worldData.Add("hasMeat", (backpack.numMeat > 0));

        //init blackboard 初始化黑板
        bb.AddData("backpack", backpack);
        bb.AddData("brain", Brain);
        bb.AddData("ironRock", FindObjectsOfType(typeof(IronRockComponent)));
        bb.AddData("appleTree", FindObjectsOfType(typeof(AppleTreeComponet)));
        bb.AddData("forge", FindObjectsOfType(typeof(ForgeComponent)));
        bb.AddData("tree", FindObjectsOfType(typeof(TreeComponent)));
        bb.AddData("wolfDen", FindObjectsOfType(typeof(WolfDen)));
        bb.AddData("choppingBlock", FindObjectsOfType(typeof(ChoppingBlockComponent)));
        bb.AddData("supplyPiles", FindObjectsOfType(typeof(SupplyPileComponent)));
        bb.AddData("camp", FindObjectsOfType(typeof(CampComponent)));
    }

    public virtual void Tick()
    {
        Brain.Tick(this);
    }
    /// <summary>
    /// 移除人员管理
    /// </summary>
    public virtual void Release()
    {
        Brain.Release();
    }
    /// <summary>
    /// 代理
    /// </summary>
    public IAgent Agent { get; set; }
    /// <summary>
    /// 人员管理
    /// </summary>
    public IBrain Brain { get; set; }
}

