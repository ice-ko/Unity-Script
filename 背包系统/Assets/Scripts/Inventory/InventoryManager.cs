using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class InventoryManager : Singleton<InventoryManager>
{
    public ToolTip toolTip;//提示面板

    private List<Item> itemList = new List<Item>();//物品信息

    private bool isToolTipShow = false;//是否显示面板
    private Vector2 toolTipPosionOffset = new Vector2(20, -25);//面板偏移位置
    private void Start()
    {
        ParseItemJson();
    }
    private void Update()
    {
        if (isToolTipShow)
        {
            toolTip.transform.localPosition=Utility.GetWorldToScreenPos()+ toolTipPosionOffset;
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
                    itemList.Add(new Consumables(itemInfo, (int)item["hp"],(int)item["mp"]));
                    break;
                case ItemType.Equipment:
                    var equipmentInfo = JsonConvert.DeserializeObject<Equipment>(item.ToString());
                    itemList.Add(new Equipment(itemInfo, equipmentInfo));
                    break;
                case ItemType.Weapon:
                    var weaponInfo = JsonConvert.DeserializeObject<Weapon>(item.ToString());
                    itemList.Add(new Weapon(itemInfo,weaponInfo));
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
        toolTip.Show(text);
        isToolTipShow = true;
    }
    /// <summary>
    /// 隐藏物品提示框
    /// </summary>
    public void HideToolTip()
    {
        toolTip.Hide();
        isToolTipShow = false;
    }
}
