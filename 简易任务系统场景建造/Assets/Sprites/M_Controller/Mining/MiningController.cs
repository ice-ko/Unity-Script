using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 采矿控制器
/// </summary>
public class MiningController : SingleObject<MiningController>
{
    /// <summary>
    /// 工作量
    /// </summary>
    public float workload;


    public Action<GameObject, Vector3> onMining;

    private MiningView view;

    void Start()
    {
        onMining += MiningTask;
        view = GetComponent<MiningView>();
    }

    void Update()
    {

    }
    public void MiningTask(GameObject go, Vector3 position)
    {
       var info =GameData.Instance.rawmaterialWorkloadDic[MenuType.Mining];

        view.onUpdateWorload?.Invoke(info.Workload);

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
                      var currentWorkload = currentTask.Workload;
                      FunctionUpdater.Create(() =>
                      {
                          currentWorkload  -= Time.deltaTime;

                          view.onUpdateWorload?.Invoke(info.Workload);

                          go.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentWorkload);

                          PlayerController.Instance.PlayMining(true);

                          if (currentWorkload <= 0f)
                          {
                              SimplePool.Despawn(go);

                              GameObject newGo = SimplePool.Spawn(ResourcesManager.LoadPrefab("Prefabs/General_Stone"), go.transform.parent.position, Quaternion.identity);
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
