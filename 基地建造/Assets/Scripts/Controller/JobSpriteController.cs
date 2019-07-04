using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSpriteController : MonoBehaviour
{
    FurnitureSpriteController furniture;

    Dictionary<Job, GameObject> jobGameObjectMap;
    void Start()
    {
        jobGameObjectMap = new Dictionary<Job, GameObject>();

        furniture = GameObject.FindObjectOfType<FurnitureSpriteController>();
        WorldController.instance.world.jobsQueue.RegisterJobCreatedCallback(OnJobCreated);
    }
    /// <summary>
    /// 任务创建
    /// </summary>
    /// <param name="job"></param>
    void OnJobCreated(Job job)
    {

        GameObject job_go = new GameObject();
        jobGameObjectMap.Add(job, job_go);
        //设置属性
        job_go.name = "Job_" + job.jobObjectType + "_" + job.tile.x + "_" + job.tile.y;
        job_go.transform.position = new Vector3(job.tile.x, job.tile.y, 0);
        job_go.transform.parent = transform;
        //
        var sprite = job_go.AddComponent<SpriteRenderer>();
        sprite.sprite = furniture.GetSpriteFurniture(job.jobObjectType);
        sprite.color = new Color(1f, 1f, 1f, 0.25f);
        sprite.sortingLayerName = "Jobs";
        //注册委托
        job.RegisterJobCompleteCallback(OnJobEnded);
        job.RegisterJobCancelCallback(OnJobEnded);
    }
    /// <summary>
    /// 任务结束
    /// </summary>
    /// <param name="job"></param>
    void OnJobEnded(Job job)
    {
        GameObject job_go = jobGameObjectMap[job];
        job.UnregisterJobCompleteCallback(OnJobEnded);
        job.UnregisterJobCancelCallback(OnJobEnded);

        Destroy(job_go);
    }
}
