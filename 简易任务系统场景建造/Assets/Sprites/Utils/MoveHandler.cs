using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用移动脚本
/// </summary>
public class MoveHandler : MonoBehaviour
{
    void Start()
    {
       
    }


    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
