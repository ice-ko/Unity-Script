using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 武器
/// </summary>
public class Weapon : Item
{
    /// <summary>
    /// 伤害
    /// </summary>
    public int Damage { get; set; }
    /// <summary>
    /// 武器类型
    /// </summary>
    public WeaponType wpType { get; set; }

    public Weapon(Item item, int damage, WeaponType type) : base(item)
    {
        Damage = damage;
        wpType = type;
    }
}
/// <summary>
/// 武器类型
/// </summary>
public enum WeaponType
{
    /// <summary>
    /// 副手
    /// </summary>
    OffHand,
    /// <summary>
    /// 住手
    /// </summary>
    MainHand
}
