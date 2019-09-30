using System;
using System.Collections.Generic;
using UnityEngine;

public class MiningView : UIView
{
    /// <summary>
    /// 工作量
    /// </summary>
    public float workload;

    public Action<float> onUpdateWorload;

    private MiningController miningController;
    void Start()
    {
        miningController = GetComponent<MiningController>();

        onUpdateWorload += UpdateWorkload;
    }
    public void UpdateWorkload(float workload)
    {
        this.workload = workload;
    }
    public void MiningTask(GameObject go, Vector3 position)
    {
        miningController.onMining?.Invoke(go, position);
    }
}
