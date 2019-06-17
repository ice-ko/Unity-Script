using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 装备槽管理
/// </summary>
public class EquipmentSlot : Slot
{
    public EequipmentType type;//装备类型
    public WeaponType wpType;//武器类型
    public override void OnPointerDown(PointerEventData eventData)
    {
        //右键点击脱掉装备
        if (eventData.button == PointerEventData.InputButton.Right && transform.childCount > 0)
        {
            ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
            CharacterPanel._Instance.PutOff(currentItemUI.item);
            Destroy(currentItemUI.gameObject);
            InventoryManager.Instance.HideToolTip();
        }

        //装备栏必须是鼠标左键才能操作
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        //判断当前跟是否有物品槽跟随鼠标移动
        if (InventoryManager.Instance.isClickItem)
        {
            //获取当前移动的物品
            ItemUI pickedItem = InventoryManager.Instance.clickItemUI;
            if (transform.childCount > 0)
            {
                //获取当前点击的物品槽子物体
                ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
                //判断是否可以放置 可以放置交换物品
                if (IsRightItem(pickedItem.item))
                {
                    base.SetItemParent(pickedItem, currentItem);
                }
            }
            else
            {
                //判断是否可以放置
                if (IsRightItem(pickedItem.item))
                {
                    base.SetCurrentItem(pickedItem);
                }
            }
        }
        else
        {
            //获取当前点击的物品槽子物体

            if (transform.childCount > 0)
            {
                ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
                base.SetMoveItemParent(currentItem);
            }

        }
    }
    /// <summary>
    /// 判断item是否合适放置在当前位置
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsRightItem(Item item)
    {
        //判断是否为相同的装备类型
        var equipType = (item is Equipment && ((Equipment)item).EquipType == this.type);
        //判断是否为相同的武器类型
        var weapon = (item is Weapon && ((Weapon)item).WeaponType == this.wpType);
        if (equipType || weapon)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
