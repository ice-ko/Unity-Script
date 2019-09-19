using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 采矿控制器
/// </summary>
public class MiningController : SingleObject<MiningController>
{
    public OreType type;
    /// <summary>
    /// 工作量
    /// </summary>
    public float workload;

    /// <summary>
    /// 掉落的材料
    /// </summary>
    public GameObject prefabMaterial;

    void Start()
    {
     
    }

    void Update()
    {

    }
    public void MiningTask(GameObject go, Vector3 position)
    {
       var info =GameData.Instance.rawmaterialWorkloadDic[MenuType.Mining];
        go.transform.parent.gameObject.GetComponent<MiningController>().workload = info.Workload;
        var result = TaskHandler.Instance.taskSystem.EnqueueTask(() =>
          {
              Task task = new MiningTask.Excavate
              {
                  TargetPosition = position,
                  Name = "采矿",
                  PrefabGame = go,
                  Parent = go.transform.parent.gameObject,
                  Workload = info.Workload,
                  ExcavateAction = (MiningTask.Excavate currentTask) =>
                  {
                      FunctionUpdater.Create(() =>
                      {
                          var currentWorkload = currentTask.Parent.GetComponent<MiningController>().workload -= Time.deltaTime;
                          go.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentWorkload);

                          go.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentWorkload);

                          PlayerController.Instance.PlayMining(true);

                          if (currentWorkload <= 0f)
                          {
                              SimplePool.Despawn(go);

                              GameObject newGo = SimplePool.Spawn(prefabMaterial, go.transform.parent.position, Quaternion.identity);
                              newGo.transform.SetParent(go.transform.parent, false);
                              newGo.transform.position = go.transform.parent.position;

                              PlayerController.Instance.PlayMining(false);

                              WorkerTaskAI.Instance.FinishTheWork();

                              return true;
                          }
                          else
                          {
                              return false;
                          }
                      });
                  }
              };
              return task;
          });
        if (result)
        {
            SimplePool.Despawn(go);
        }
    }
}
