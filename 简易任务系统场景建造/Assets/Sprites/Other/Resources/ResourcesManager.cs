using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理器
/// </summary>
public class ResourcesManager : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }
    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="pathName">路径名称</param>
    /// <returns></returns>
    public static GameObject LoadPrefab(string pathName)
    {
        return Resources.Load<GameObject>(pathName);
    }
}
