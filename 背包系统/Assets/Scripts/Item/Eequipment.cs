using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 装备
/// </summary>
public class Equipment : Item
{
    /// <summary>
    /// 力量
    /// </summary>
    public int Strength { get; set; }
    /// <summary>
    /// 智力（精神）
    /// </summary>
    public int Intellect { get; set; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agile { get; set; }
    /// <summary>
    /// 耐力（体力）
    /// </summary>
    public int Stamina { get; set; }
    /// <summary>
    /// 装备类型
    /// </summary>
    public EequipmentType EquipType { get; set; }

    public Equipment()
    {
    }
    public Equipment(Item item, Equipment equipment) : base(item)
    {
        Strength = equipment.Strength;
        Intellect = equipment.Intellect;
        Agile = equipment.Agile;
        Stamina = equipment.Stamina;
        EquipType = equipment.EquipType;
    }
    public override string GetToolTipText()
    {
        var text = base.GetToolTipText();
        return string.Format("{0}\n<color=blue>装备类型：{1}\n力量：{2}\n精神：{3}\n敏捷：{4}\n体力：{5}</color>", text, EquipType.GetDescription(), Strength, Intellect, Agile, Stamina);
    }
}
/// <summary>
/// 装备类型
/// </summary>
public enum EequipmentType
{
    [Description("没有类型")]
    None,
    /// <summary>
    /// 头部
    /// </summary>
    [Description("头部")]
    Head,
    /// <summary>
    /// 颈部
    /// </summary>
    [Description("颈部")]
    Neck,
    /// <summary>
    /// 胸部
    /// </summary>
    [Description("胸部")]
    Chest,
    /// <summary>
    /// 戒指
    /// </summary>
    [Description("戒指")]
    Ring,
    /// <summary>
    /// 腿
    /// </summary>
    [Description("腿")]
    Leg,
    /// <summary>
    /// 护腕
    /// </summary>
    [Description("护腕")]
    Bracer,
    /// <summary>
    /// 饰品
    /// </summary>
    [Description("饰品")]
    Trinket,
    /// <summary>
    /// 肩
    /// </summary>
    [Description("肩")]
    Shoulder,
    /// <summary>
    /// 腰带
    /// </summary>
    [Description("腰带")]
    Belt,
    /// <summary>
    /// 副手
    /// </summary>
    [Description("副手")]
    OffHand,
    /// <summary>
    /// 鞋子
    /// </summary>
    [Description("鞋子")]
    Boots
}
