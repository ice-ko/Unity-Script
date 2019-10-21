// This script extends the editor to render the Sorting Layer options in the Inspector
// view

using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SortingLayerEditor : Editor
{
    [MenuItem("Edit/Bring Forwards #PGUP")]
    public static void BringForwards()
    {
        //Debug.Log("bring forward");
        ChangeSortingOrder(1);
    }

    [MenuItem("Edit/Push Backwards #PGDN")]
    public static void PushBackwards()
    {
        //Debug.Log("push backward");
        ChangeSortingOrder(-1);
    }

    private static void ChangeSortingOrder(int amount)
    {
        foreach (var selObject in Selection.gameObjects)
        {
            foreach (var renderer in selObject.GetComponentsInChildren<Renderer>())
            {
                //int val = renderer.sortingOrder;
                SerializedObject serializedObject = new SerializedObject(renderer);
                SerializedProperty serProperty = serializedObject.FindProperty("m_SortingOrder");
                serProperty.intValue = serProperty.intValue + amount;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    // Get the sorting layer names
    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    private void MyCallback( object obj)
    {
        Renderer renderer = (Renderer)target;
        string layerName = (string) obj;
        Debug.Log( "LayerName=" + layerName);
        renderer.sortingLayerName = (string) obj;
   }
    
    protected void SortingLayerGUI()
    {
        if ( ! (target is Renderer) )
            return;
        Renderer renderer = (Renderer)target;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sorting Layer");

        string[] sortingLayers = GetSortingLayerNames();
        if (GUILayout.Button(renderer.sortingLayerName))
        {
            var menu = new GenericMenu();
            foreach( var sortingLayer in sortingLayers)
                menu.AddItem(new GUIContent(sortingLayer) ,false, MyCallback, sortingLayer);
            menu.ShowAsContext();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sorting Order");

        int newOrder = EditorGUILayout.IntField(renderer.sortingOrder);
        //string orderString = GUILayout.TextField("" + renderer.sortingOrder);
        //int order = renderer.sortingOrder;
        //try
        //{
            //order = int.Parse(orderString);
        //}
        //catch (Exception)
        //{
            //order = 0;
        //}
        if (newOrder != renderer.sortingOrder)
        {
            SerializedObject serializedObject = new SerializedObject(renderer);
            SerializedProperty serProperty = serializedObject.FindProperty("m_SortingOrder");
            serProperty.intValue = newOrder;
            serializedObject.ApplyModifiedProperties();
        }
        
        GUILayout.EndHorizontal();
    }
}

[CustomEditor(typeof (MeshRenderer))]
public class MeshRendererEditor : SortingLayerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SortingLayerGUI();
    }
}

[CustomEditor(typeof(SkinnedMeshRenderer))]
public class SkinnedMeshRendererEditor : SortingLayerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SortingLayerGUI();
    }
}

//[CustomEditor(typeof(ParticleSystemRenderer))]
//public class ParticleSystemRendererEditor : SortingLayerEditor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        SortingLayerGUI();
//    }
//}

//[CustomEditor(typeof(ParticleRenderer))]
//public class ParticleRendererEditor : SortingLayerEditor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        SortingLayerGUI();
//    }
//}

//[CustomEditor(typeof(TrailRenderer))]
//public class TrailRendererEditor : SortingLayerEditor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        SortingLayerGUI();
//    }
//}

//[CustomEditor(typeof(LineRenderer))]
//public class LineRendererEditor : SortingLayerEditor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        SortingLayerGUI();
//    }
//}

