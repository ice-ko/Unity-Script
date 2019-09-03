/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class GameHandler : MonoBehaviour {

    private static GameHandler instance;
    
    [SerializeField] private Transform goldNode1Transform;
    [SerializeField] private Transform goldNode2Transform;
    [SerializeField] private Transform goldNode3Transform;
    [SerializeField] private Transform storageTransform;

    private void Awake() {
        instance = this;
    }

    private Transform GetResourceNode() {
        List<Transform> resourceNodeList = new List<Transform>() { goldNode1Transform, goldNode2Transform, goldNode3Transform };
        return resourceNodeList[UnityEngine.Random.Range(0, resourceNodeList.Count)];
    }

    public static Transform GetResourceNode_Static() {
        return instance.GetResourceNode();
    }

    private Transform GetStorage() {
        return storageTransform;
    }

    public static Transform GetStorage_Static() {
        return instance.GetStorage();
    }
}
