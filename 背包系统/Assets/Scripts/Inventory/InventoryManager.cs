using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InventoryManager : Singleton<InventoryManager>
{
    private List<Item> itemList = new List<Item>();
    private void Start()
    {
        ParseItemJson();
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
        itemList = JsonConvert.DeserializeObject<List<Item>>(jsonStr);
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
}
