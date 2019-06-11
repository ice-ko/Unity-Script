using System;
using System.Collections.Generic;

/// <summary>
/// 消耗品
/// </summary>
public class Consumables : Item
{
    public int Hp { get; set; }
    public int Mp { get; set; }
    public Consumables(Item item, int hp, int mp) : base(item)
    {
        Hp = hp;
        Mp = mp;
    }
    public override string GetToolTipText()
    {
        var text = base.GetToolTipText();
        return string.Format("{0}\n<color=red>加血：{1}</color>\n<color=blue>加蓝：{2}</color>", text, Hp, Mp);
    }
}