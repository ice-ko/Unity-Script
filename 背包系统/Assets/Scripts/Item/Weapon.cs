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
    public WeaponType WeaponType { get; set; }
    public Weapon()
    {
    }
    public Weapon(Item item, Weapon _weapon) : base(item)
    {
        Damage = _weapon.Damage;
        WeaponType = _weapon.WeaponType;
    }
    public override string GetToolTipText()
    {
        var text = base.GetToolTipText();
        var type =string.Empty;
        switch (WeaponType)
        {
            case WeaponType.OffHand:
                type = "副手"; break;
            case WeaponType.MainHand:
                type = "主手"; break;
        }
        return string.Format("{0}\n武器类型：{1}\n伤害：{2}", text, type, Damage);
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
    /// 主手
    /// </summary>
    MainHand
}
