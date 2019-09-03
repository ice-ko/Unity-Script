using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    public static Warehouse Instance;
    /// <summary>
    /// 金矿数量
    /// </summary>
    public int goldmineCount;

    private void Awake()
    {
        Instance = this;
    }
}
