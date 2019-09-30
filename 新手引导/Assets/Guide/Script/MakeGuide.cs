using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class GuideUI
{
    public GameObject go;
    public string hierarchyPath;

    public GuideUI(GameObject go, string hierarchyPath)
    {
        this.go = go;
        this.hierarchyPath = hierarchyPath;
    }
}

public class MakeGuide : MonoBehaviour {

    public List<GuideUI> guideList = new List<GuideUI>();

}
