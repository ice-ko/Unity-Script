using UnityEngine;
using System.Collections;

public class Panel2 : MonoBehaviour {

	void Start () 
    {
        EventTriggerListener.GetListener(transform.Find("Button").gameObject).onClick +=
            (go) => { gameObject.SetActive(false); };
        EventTriggerListener.GetListener(transform.Find("Button (1)").gameObject).onClick +=
            (go) => { Debug.Log("整理"); };
	}

}
