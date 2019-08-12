using UnityEngine;
using System.Collections;

/**
 * A tool used for mining ore and chopping wood.
 * Tools have strength that gets used up each time
 * they are used. When their strength is depleted
 * they should be destroyed by the user.
 */
///<summary>
///用于开采矿石和砍伐木材的工具。
///工具的强度每次都会消耗殆尽
///他们被使用。 当他们的力量耗尽时
///它们应该被用户销毁。
///备注：工具耐久管理
///</summary>
public class ToolComponent : MonoBehaviour
{
    /// <summary>
    /// 0..1]或0％至100％
    /// </summary>
    public float strength; // [0..1] or 0% to 100%

    void Start()
    {
        //全力
        strength = 1; // full strength
    }

    /**
	 * Use up the tool by causing damage. Damage should be a percent
	 * from 0 to 1, where 1 is 100%.
	 */
    ///<summary>
    ///造成损坏，用完工具。 损害应该是一个百分比
    ///从0到1，其中1是100％。
    ///</summary>
    public void use(float damage)
    {
        strength -= damage;
    }
    /// <summary>
    /// 销毁没有耐久的工具
    /// </summary>
    /// <returns></returns>
    public bool destroyed()
    {
        return strength <= 0f;
    }
}

