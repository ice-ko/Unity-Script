using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品基本信息
/// </summary>
public class Item
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 物品名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 物品类型
    /// </summary>
    public ItemType Type { get; set; }
    /// <summary>
    /// 物品品质
    /// </summary>
    public Quality Quality { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Ddescription { get; set; }
    /// <summary>
    /// 容量
    /// </summary>
    public int Capacity { get; set; }
    /// <summary>
    /// 购买价格
    /// </summary>
    public decimal BuyPrice { get; set; }
    /// <summary>
    /// 出售价格
    /// </summary>
    public decimal SellPrice { get; set; }
    /// <summary>
    /// 图片路径
    /// </summary>
    public string Sprite { get; set; }
    public Item()
    {
        Id = -1;
    }
    public Item(Item item)
    {
        this.Id = item.Id;
        this.Name = item.Name;
        this.Type = item.Type;
        this.Quality = item.Quality;
        this.Ddescription = item.Ddescription;
        this.Capacity = item.Capacity;
        this.BuyPrice = item.BuyPrice;
        this.SellPrice = item.SellPrice;
        this.Sprite = item.Sprite;
    }
}
/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    /// <summary>
    /// 消耗品
    /// </summary>
    Consumable,
    /// <summary>
    /// 装备
    /// </summary>
    Equipment,
    /// <summary>
    /// 武器
    /// </summary>
    Weapon,
    /// <summary>
    /// 材料
    /// </summary>
    Material
}
/// <summary>
/// 物品品质
/// </summary>
public enum Quality
{
    /// <summary>
    /// 普通
    /// </summary>
    Common,
    /// <summary>
    /// 稀有
    /// </summary>
    Rare,
    /// <summary>
    /// 史诗
    /// </summary>
    Epic,
    /// <summary>
    /// 传说
    /// </summary>
    Legend,

}