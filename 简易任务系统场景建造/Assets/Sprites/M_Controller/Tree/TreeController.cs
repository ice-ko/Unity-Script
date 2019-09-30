using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public Action<GameObject, Vector3> onFelling;

    private TreeView view;
    void Start()
    {
        onFelling += FellingTreeTask;
        view = GetComponent<TreeView>();
    }

    void Update()
    {

    }
    /// <summary>
    /// 砍伐树木任务
    /// </summary>
    /// <param name="go"></param>
    /// <param name="position"></param>
    public void FellingTreeTask(GameObject go, Vector3 position)
    {
        var info = GameData.Instance.rawmaterialWorkloadDic[MenuType.Felling];

        view.onUpdateWorload?.Invoke(info.Workload);

        var result = TaskHandler.Instance.taskSystem.EnqueueTask(() =>
        {
            Task task = new MiningTask.Excavate
            {
                TargetPosition = position,
                Name = "砍伐",
                PrefabGame = go,
                Parent = go.transform.parent.gameObject,
                Workload = info.Workload,
                ExcavateAction = (MiningTask.Excavate currentTask) =>
                {
                    var currentWorkload = currentTask.Workload;
                    FunctionUpdater.Create(() =>
                    {
                        currentWorkload -= Time.deltaTime;

                        view.onUpdateWorload?.Invoke(currentWorkload);

                        go.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentWorkload);

                        PlayerController.Instance.PlayMining(true);

                        if (currentWorkload <= 0f)
                        {
                            SimplePool.Despawn(currentTask.PrefabGame);

                            GameObject newGo = SimplePool.Spawn(ResourcesManager.LoadPrefab("Prefabs/Log"), go.transform.parent.position, Quaternion.identity);
                            newGo.transform.SetParent(go.transform.parent, false);
                            newGo.transform.position = go.transform.parent.position;


                            PlayerController.Instance.PlayMining(false);

                            WorkerTaskAI.Instance.FinishTheWork();

                            // Destroy(currentTask.PrefabGame.transform.parent.gameObject);

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