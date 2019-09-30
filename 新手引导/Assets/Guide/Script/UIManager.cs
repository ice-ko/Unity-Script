using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoSingletion<UIManager> {

    public Transform canvasTra;
    private Dictionary<string, GameObject> nameGoDir = new Dictionary<string, GameObject>();

    void Init()
    {
        if (canvasTra == null) canvasTra = GameObject.Find("Canvas").transform;
    }

    public void Clear()
    {
        nameGoDir.Clear();
    }

    public GameObject Find(string name)
    {
        Init();
        Transform t = canvasTra.Find(name);
        if(t == null) return null;
        else return t.gameObject;
    }

    public GameObject Show(string name)
    {
        if (!nameGoDir.ContainsKey(name))
        {
            GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>(name));
            go.transform.SetParent(canvasTra, false);
            nameGoDir.Add(name, go);
        }
        else
        {
            nameGoDir[name].SetActive(true);
        }
        return nameGoDir[name];
    }

}
