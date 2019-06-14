using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using UnityEngine.EventSystems;

public class InventoryManager : Singleton<InventoryManager>
{
    public ToolTip toolTip;//提示面板
    public RectTransform moveParentTransform;//物品移动父对象

    private List<Item> itemList = new List<Item>();//物品信息
    [HideInInspector]
    public bool isToolTipShow = false;//是否显示面板
    [HideInInspector]
    public bool isClickItem = false;//是否点击物品
    private Vector2 toolTipPosionOffset = new Vector2(20, -25);//面板偏移位置
    [HideInInspector]
    public ItemUI clickItemUI;//点击后的物品
    private void Start()
    {
        ParseItemJson();
    }
    private void Update()
    {
        //物品跟随鼠标移动
        if (isClickItem && moveParentTransform.childCount > 0)
        {
            moveParentTransform.transform.localPosition = Utility.GetWorldToScreenPos() + new Vector2(15, -15);
            toolTip.Hide();
        }
        //提示信息面板跟随鼠标移动
        else if (isToolTipShow)
        {
            toolTip.transform.localPosition = Utility.GetWorldToScreenPos() + toolTipPosionOffset;
        }
        //判断当前鼠标左键下是否存在ui(丢弃物品)
        if (isClickItem && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(-1))
        {
            isClickItem = false;
            clickItemUI = null;
            Destroy(moveParentTransform.GetChild(0).gameObject);
        }
    }
    /// <summary>
    /// 解析物品json
    /// </summary>
    void ParseItemJson()
    {
        //读取json文件信息
        TextAsset itemText = Resources.Load<TextAsset>("JsonData/Items");
        //获取json文本
        var jsonStr = itemText.text;
        //反序列化JSON
        var jsonList = JsonConvert.DeserializeObject<List<JObject>>(jsonStr);
        foreach (var item in jsonList)
        {
            Item itemInfo = JsonConvert.DeserializeObject<Item>(item.ToString());
            ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), item["type"].ToString());
            switch (type)
            {
                case ItemType.Consumable:
                    itemList.Add(new Consumables(itemInfo, (int)item["hp"], (int)item["mp"]));
                    break;
                case ItemType.Equipment:
                    var equipmentInfo = JsonConvert.DeserializeObject<Equipment>(item.ToString());
                    itemList.Add(new Equipment(itemInfo, equipmentInfo));
                    break;
                case ItemType.Weapon:
                    var weaponInfo = JsonConvert.DeserializeObject<Weapon>(item.ToString());
                    itemList.Add(new Weapon(itemInfo, weaponInfo));
                    break;
                case ItemType.Material:
                    itemList.Add(itemInfo);
                    break;
                default:
                    itemList.Add(itemInfo);
                    break;
            }
        }
    }
    /// <summary>
    /// 根据id返回数据
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public Item GetItemById(int Id)
    {
        foreach (var item in itemList)
        {
            if (item.Id == Id)
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    ///显示物品提示框
    /// </summary>
    /// <param name="text"></param>
    public void ShowToolTip(string text)
    {
        if (isClickItem)
        {
            return;
        }
        toolTip.Show(text);
        isToolTipShow = true;
    }
    /// <summary>
    /// 隐藏物品提示框
    /// </summary>
    public void HideToolTip()
    {
        if (isClickItem)
        {
            return;
        }
        toolTip.Hide();
        isToolTipShow = false;
    }
    /// <summary>
    /// 移除当前跟随鼠标移动的物品
    /// </summary>
    public void RemoveItem()
    {
        isClickItem = false;
        clickItemUI = null;
        if (moveParentTransform.childCount > 0)
        {
            Destroy(moveParentTransform.GetChild(0).gameObject);
        }
    }
}
