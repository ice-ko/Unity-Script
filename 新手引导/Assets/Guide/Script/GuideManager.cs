using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GuideManager : MonoSingletion<GuideManager> {

    private Transform maskTra;

    private string fileDir = "GuideFile/";
    private string nowCsvFile;
    private int nowIndex;
    private bool isFinish = false;//是否完成所有的新手引导
    private string[] nameArray;    

    public void Init()
    {
        //读取进度
        string content = Resources.Load<TextAsset>(fileDir + "GuideProgress").ToString();
        string[] temp = content.Split(',');
        nowCsvFile = temp[0];
        nowIndex = int.Parse(temp[1]);
        isFinish = bool.Parse(temp[2]);

        //读取需要高亮的组件的Hierarchy路径
        if (!isFinish)
        {
            string s = Resources.Load<TextAsset>(fileDir + nowCsvFile).ToString();
            nameArray = s.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);   
        } 
    }

    void OnDestroy()
    {
        //退出游戏后的处理
        Debug.Log("OnDestroy");
    }

    public void Next()
    {
        if (nowIndex < nameArray.Length)
        {
            ShowHightLight(nameArray[nowIndex]);
            nowIndex++;
        }
        else//加载下一个文件
        {
            maskTra.gameObject.SetActive(false);
     
            int index = int.Parse(nowCsvFile.Substring(nowCsvFile.Length - 1));
            index++;
            nowCsvFile = nowCsvFile.Substring(0, nowCsvFile.Length - 1) + index.ToString();
            string path = fileDir + nowCsvFile;

            string content = null;
            try
            {
                content = Resources.Load<TextAsset>(path).ToString();
            }
            catch (Exception e) 
            {
                isFinish = true;
                Debug.Log("finish");
                return;
            }
            nowIndex = 0;
            nameArray = content.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);  
        }
    }

    void ShowHightLight(string name, bool checkIsClone = true)
    {
        if(checkIsClone && name.Contains("/"))
        {
            name = name.Insert(name.IndexOf('/'), "(Clone)");
        }
        StartCoroutine(FindUI(name));
    }

    void CancelHightLight(GameObject go)
    {
        Destroy(go.GetComponent<GraphicRaycaster>());
        Destroy(go.GetComponent<Canvas>());
        Next();
        EventTriggerListener.GetListener(go).onClick -= CancelHightLight;
    }

    IEnumerator FindUI(string name)
    {
        //寻找目标
        GameObject go = UIManager.Instance.Find(name);
        while(go == null)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("wait");
            go = UIManager.Instance.Find(name);
        }
       
        //高亮
        maskTra = UIManager.Instance.Show("Mask").transform;
        maskTra.SetAsLastSibling();
        go.AddComponent<Canvas>().overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        go.GetComponent<Canvas>().sortingOrder = 1;
        //设置监听
        EventTriggerListener.GetListener(go).onClick += CancelHightLight;
    }

}
