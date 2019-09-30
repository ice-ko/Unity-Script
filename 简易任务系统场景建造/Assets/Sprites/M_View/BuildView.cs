using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildView : UIView
{
    /// <summary>
    /// 工作量
    /// </summary>
    public float workload;

    public Action<float> onUpdateWorload;

    private BuildController controller;
    private void Awake()
    {
        controller = GetComponent<BuildController>();

        onUpdateWorload += UpdateWorkload;
    }
    
    public void UpdateWorkload(float workload)
    {
        this.workload = workload;
    }
    public void BuildTask(GameObject go, Vector3 position)
    {
        controller.onBuild?.Invoke(go, position);
    }
   
}