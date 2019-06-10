using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour
{
    public GameObject itmePrefab;
    /// <summary>
    /// 把item放在当前物品槽
    /// 如果已经存在则数量加1 
    /// 如果没有则实例化新的物品item
    /// </summary>
    /// <param name="item"></param>
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            //创建新的物品
            var itemObj = Instantiate(itmePrefab);
            itemObj.transform.SetParent(transform);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localScale = new Vector3(1, 1, 1);
            itemObj.GetComponent<ItemUI>().SetItem(item);
            //调整ui显示
            var pos = itemObj.transform.localPosition;
            itemObj.transform.localPosition = new Vector3(pos.x, pos.y + 2f, pos.z);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }
    /// <summary>
    /// 获取当前物品槽存储的物品信息
    /// </summary>
    /// <returns></returns>
    public ItemUI GetItme()
    {
        return transform.GetChild(0).GetComponent<ItemUI>();
    }
}
