using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : Inventory
{
    public static CharacterPanel _Instance;
    private void Awake()
    {
        _Instance = this;
    }
    /// <summary>
    /// 穿戴装备
    /// </summary>
    /// <param name="itemUI"></param>
    public void PutOn(ItemUI itemUI)
    {
        Item exitItem = null;
        foreach (Slot slot in slotArr)
        {
            if (slot is EquipmentSlot)
            {
                EquipmentSlot equipmentSlot = (EquipmentSlot)slot;
                if (equipmentSlot.IsRightItem(itemUI.item))
                {
                    if (equipmentSlot.transform.childCount > 0)
                    {
                        exitItem = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>().item;
                    }
                    equipmentSlot.StoreItem(itemUI.item);
                    break;
                }
            }
        }
        if (exitItem != null)
        {
            Inventory.Instance.StoreItem(exitItem);
        }
    }
    /// <summary>
    /// 脱掉装备
    /// </summary>
    /// <param name="item"></param>
    public void PutOff(Item item)
    {

        Inventory.Instance.StoreItem(item);
    }
}
