using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 枚举扩展类
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// 获取枚举的描述信息
    /// </summary>
    public static string GetDescription(this Enum em)
    {
        Type type = em.GetType();
        FieldInfo fd = type.GetField(em.ToString());
        if (fd == null)
            return string.Empty;
        object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
        string name = string.Empty;
        foreach (DescriptionAttribute attr in attrs)
        {
            name = attr.Description;
        }
        return name;
    }
}
