using System;
using System.Reflection;

/// <summary>
/// 实体类操作
/// </summary>
public class EntityHelper
{
    /// <summary>
    /// 复制单个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    public static T Copy<T>(object model)
    {
        if (model == null)
        {
            return default(T);
        }
        T target = Activator.CreateInstance<T>();
        Type targetType = target.GetType();
        PropertyInfo[] perties = model.GetType().GetProperties();
        foreach (var item in perties)
        {
            var per = targetType.GetProperty(item.Name);
            if (per != null && per.CanWrite)
            {
                per.SetValue(target, item.GetValue(model, null), null);
            }
        }

        return target;
    }

    /// <summary>
    /// 两个实体之间相同属性的映射
    /// </summary>
    /// <typeparam name="TResult">输出</typeparam>
    /// <typeparam name="T">输入</typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    public static TResult Mapping<TResult, T>(T model)
    {
        TResult result = Activator.CreateInstance<TResult>();
        foreach (PropertyInfo info in typeof(TResult).GetProperties())
        {
            PropertyInfo pro = typeof(T).GetProperty(info.Name);
            if (pro != null)
                info.SetValue(result, pro.GetValue(model));
        }
        return result;
    }
}
