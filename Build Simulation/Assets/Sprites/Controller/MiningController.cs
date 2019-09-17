using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 采矿控制器
/// </summary>
public class MiningController : SingleObject<MiningController>
{
    void Start()
    {

    }


    void Update()
    {

    }
    public void MiningTask(GameObject go, Vector3 position)
    {
        var result = TaskHandler.Instance.taskSystem.EnqueueTask(() =>
          {
              Task task = new MiningTask.Excavate
              {
                  TargetPosition = position,
                  Name = "采矿",
                  PrefabGame = go,
                  ExcavateAction = () =>
                  {
                      float alpha = 1f;
                      FunctionUpdater.Create(() =>
                      {
                          alpha -= Time.deltaTime;
                          go.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
                          if (alpha <= 0f)
                          {
                              SimplePool.Despawn(go);
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
