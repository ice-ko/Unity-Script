using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject boxPanel;
    public GameObject characterPanel;
    void Start()
    {
    }

    void Update()
    {
        //测试背包随机生成物品
        if (Input.GetKeyDown(KeyCode.A))
        {
            int id = Random.Range(1,18);
            Inventory.Instance.StoreItem(id);
        }
        //打开箱子
        if (Input.GetKeyDown(KeyCode.S))
        {
            boxPanel.SetActive(true);
        }
        //打开装备栏
        if (Input.GetKeyDown(KeyCode.C))
        {
            characterPanel.SetActive(true);
        }
    }
}
