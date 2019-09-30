using UnityEngine;
using System.Collections;

public class Panel1 : MonoBehaviour {

	void Start () 
    {
        EventTriggerListener.GetListener(transform.Find("Button").gameObject).onClick +=
            (go) => { gameObject.SetActive(false); };
        EventTriggerListener.GetListener(transform.Find("Button (1)").gameObject).onClick +=
            (go) => { Debug.Log("装上"); };
        EventTriggerListener.GetListener(transform.Find("Button (2)").gameObject).onClick +=
            (go) => { Debug.Log("卸下"); };
        EventTriggerListener.GetListener(transform.Find("Image/Button").gameObject).onClick +=
            (go) => { Debug.Log("查看属性"); };
	}
	
}
