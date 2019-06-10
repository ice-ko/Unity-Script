using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 装备
/// </summary>
public class Eequipment : Item
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


    public Eequipment(Item item, Eequipment equipment) : base(item)
    {
        Strength = equipment.Strength;
        Intellect = equipment.Intellect;
        Agile = equipment.Agile;
        Stamina = equipment.Stamina;
        EquipType = equipment.EquipType;
    }
}
/// <summary>
/// 装备类型
/// </summary>
public enum EequipmentType
{
    /// <summary>
    /// 头部
    /// </summary>
    Head,
    /// <summary>
    /// 颈部
    /// </summary>
    Neck,
    /// <summary>
    /// 戒指
    /// </summary>
    Ring,
    /// <summary>
    /// 腿
    /// </summary>
    Leg,
    /// <summary>
    /// 护腕
    /// </summary>
    Bracers,
    /// <summary>
    /// 饰品
    /// </summary>
    Trinket,
    /// <summary>
    /// 肩
    /// </summary>
    Shoulder,
    /// <summary>
    /// 腰带
    /// </summary>
    Belt,
    /// <summary>
    /// 副手
    /// </summary>
    OffHand
}
