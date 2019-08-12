using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    //目标（小人）
    public GameObject Target;
    //最大小人数量
    public int MaxNum;
    BlackBoard bb = new BlackBoard();
    void Start()
    {
        StartCoroutine(GenerateAi());
        //添加仓库
        bb.AddData("Warehouse", FindObjectsOfType(typeof(Warehouse)));
    }

    private IEnumerator GenerateAi()
    {
        Target.SetActive(false);
        for (int i = 0; i < MaxNum; i++)
        {
            GameObject go = Instantiate(Target);
            go.name = i.ToString();
            go.SetActive(true);
            AddRandomWork(go);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public Material MatMiner;
    public Material MatLogger;
    public Material MatBlacksmith;
    public Material MatWoodCutter;
    /// <summary>
    /// 添加随机工作
    /// </summary>
    /// <param name="go"></param>
    private void AddRandomWork(GameObject go)
    {
        MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
        go.AddComponent<CuttingTrees>();
    }
}
