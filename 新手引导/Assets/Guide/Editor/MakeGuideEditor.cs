using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(MakeGuide))]
[ExecuteInEditMode] 
public class MakeGuideEditor : Editor {

    string filePath;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Event.current.type == EventType.DragExited)   
        {
            UpdatePath();
        }

        EditorGUILayout.LabelField("----------------------------------------------------------");
        EditorGUILayout.LabelField("文件路径：");
        EditorGUILayout.TextField(filePath);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清空路径"))
        {
            filePath = "";
        }
        if (GUILayout.Button("读取文件(.csv)"))
        {
            Load();
        }
        if (GUILayout.Button("保存"))
        {
            Save();
        }
        EditorGUILayout.EndHorizontal();
    }

    string hierarchyPath;
    string GetHierarchyPath(Transform t, bool initPath = true)
    {
        if (initPath) hierarchyPath = "";
        hierarchyPath = t.name + hierarchyPath;
        if (t.parent.name != "Canvas")
        {
            Transform parent = t.parent;
            hierarchyPath = "/" + hierarchyPath;
            GetHierarchyPath(parent, false);
        }
        return hierarchyPath;
    }

    void UpdatePath()
    {
        MakeGuide makeGuide = (MakeGuide)target;
        List<GuideUI> guideList = makeGuide.guideList;
        foreach (GuideUI guideUI in guideList)
        {
            if (guideUI.go != null)
            {
                guideUI.hierarchyPath = GetHierarchyPath(guideUI.go.transform);
            }
        }
    }

    void Load()
    {
        filePath = EditorUtility.OpenFilePanel("读取文件(.csv)", Application.dataPath, "csv");
        string content = File.ReadAllText(filePath);
        string[] paths = content.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        MakeGuide makeGuide = (MakeGuide)target;
        makeGuide.guideList = new List<GuideUI>();
        Transform canvasTra = GameObject.Find("Canvas").transform;
        foreach(string path in paths)
        {
            GameObject go = canvasTra.Find(path).gameObject;
            makeGuide.guideList.Add(new GuideUI(go, path));
        }
    }

    void Save()
    {
        if (string.IsNullOrEmpty(filePath))//创建一个新文件并保存
        {
            string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, "", "csv");
            File.WriteAllText(path, GetInspectorInfo());     
        }
        else//直接保存在读取的文件
        {
            File.WriteAllText(filePath, GetInspectorInfo());
        }
        AssetDatabase.Refresh();
        Debug.Log("保存成功");
    }

    string GetInspectorInfo()
    {
        string content = "";
        MakeGuide makeGuide = (MakeGuide)target;
        List<GuideUI> guideList = makeGuide.guideList;
        foreach (GuideUI guideUI in guideList)
        {
            if (guideUI.go != null)
            {
                content += guideUI.hierarchyPath + "\r\n";
            }
        }
        return content;
    }

}
