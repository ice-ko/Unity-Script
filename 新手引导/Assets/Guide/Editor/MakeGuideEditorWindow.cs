using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeGuideEditorWindow : EditorWindow {

    [MenuItem("MakeGuide/AddScript")]
    static void AddScript()
    { 
        Selection.activeGameObject.AddComponent<MakeGuide>();
    }

    [MenuItem("MakeGuide/RemoveScript")]
    static void RemoveScript()
    {
        DestroyImmediate(Selection.activeGameObject.GetComponent<MakeGuide>());
    }

}
