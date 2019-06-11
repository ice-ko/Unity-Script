using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knapsack : Singleton<Inventory>
{
    public int slotCount;//背包格子数量
    public GameObject slotPrefab;
    public GameObject slotPanel;//背包格子父对象
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject go = Instantiate(slotPrefab);
            go.transform.SetParent(slotPanel.transform,false);
        } 
    }
    void Update()
    {
        
    }
}
