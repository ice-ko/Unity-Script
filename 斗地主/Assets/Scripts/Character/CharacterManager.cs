using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : ManagerBase {

    public static CharacterManager Instance = null;

    void Awake()
    {
        Instance = this;
    }

}
