using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleObject<GameManager>
{
    public OreDictionary oreSpriteDictionary;

    public TreeDictionary treeSpriteDictionary;

    void Start()
    {
        GameData.Instance.Init();
    }

    void Update()
    {
        
    }
}
