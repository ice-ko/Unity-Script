using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeView : UIView
{
    /// <summary>
    /// 工作量
    /// </summary>
    public float workload;

    public Action<float> onUpdateWorload;

    private TreeController treeController;
    void Start()
    {
        treeController = GetComponent<TreeController>();

        onUpdateWorload += UpdateWorkload;
    }


    void Update()
    {

    }
    public void UpdateWorkload(float workload)
    {
        this.workload = workload;
    }
    public void FellingTreeTask(GameObject go, Vector3 position)
    {
        treeController.onFelling?.Invoke(go, position);
    }

}
