using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public int slotCount;//背包格子数量
    public GameObject slotPrefab;
    public GameObject slotPanel;//背包格子父对象
    //
    private Slot[] slotArr;//slot（物品槽）数组
    public virtual void Start()
    {
        //生成指定数量的物品槽
        for (int i = 0; i < slotCount; i++)
        {
            GameObject go = Instantiate(slotPrefab);
            go.transform.SetParent(slotPanel.transform, false);
        }
        //获取所有的slot（物品槽）
        slotArr = GetComponentsInChildren<Slot>();
    }
    /// <summary>
    /// 存储物品
    /// </summary>
    /// <returns></returns>
    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemById(id);
        return StoreItem(item);
    }
    /// <summary>
    /// 储存物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool StoreItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("物品不存在");
            return false;
        }
        //检测物品存储容量
        if (item.Capacity == 1)
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.LogWarning("没有空的物品槽");
                return false;
            }
            else
            {
                //把物品存储到空的物品槽
                slot.StoreItem(item);
            }
        }
        else
        {
            Slot slot = FindSameTypeSlot(item);
            if (slot != null)
            {
                slot.StoreItem(item);
            }
            else
            {
                Slot emptySlot = FindEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                }
                else
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// 查找空的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindEmptySlot()
    {
        foreach (var slot in slotArr)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
    /// <summary>
    /// 查找是否存在相同的物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private Slot FindSameTypeSlot(Item item)
    {
        foreach (var slot in slotArr)
        {
            if (slot.transform.childCount == 0)
            {
                continue;
            }
            ItemUI itemUI = slot.GetItme();
            //检测是否相同物品 并检测当前物品的容量是否已到达最大
            if (slot.transform.childCount >= 1
                && itemUI.item.Id == item.Id && itemUI.amount != itemUI.item.Capacity)
            {
                return slot;
            }
        }
        return null;
    }
}
