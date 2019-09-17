using System;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 任务处理
/// </summary>
public class TaskHandler : SingleObject<TaskHandler>
{
    public GameObject character;
    public Tilemap tilemap;

    public TileDictionary tileDic;

    public TaskSystem<Task> taskSystem;
    private static TaskSystem<TransporterTask> transporterTask;
    private void Start()
    {
        taskSystem = new TaskSystem<Task>();

        Worker worker = Worker.Create(character, new Vector3(0, 0));
        worker.gameObject.transform.GetChild(0).GetComponent<AStarTilemap>().tilemap = tilemap;
        worker.gameObject.transform.GetChild(0).GetComponent<MoveTargetPosition>().tilemap = tilemap;
        WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
        workerTaskAI.Setup(worker, taskSystem);


    }
    private void Update()
    {
      
    }
    void BuildWall(Vector3 pos)
    {

    }
}
