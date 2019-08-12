using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public interface IBrain
{
    void Init();
    void Tick(IGoap goap);
    void Release();
    Dictionary<string,bool> NextGoal();
}
