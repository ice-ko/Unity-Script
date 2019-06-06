using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManage : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="index"></param>
    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }
}
