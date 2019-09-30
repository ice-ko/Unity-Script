using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Canvas1 : MonoBehaviour {

    void Awake()
    {
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button")).onClick +=
            (go) => { UIManager.Instance.Show("EquipPanel"); };
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button (1)")).onClick +=
            (go) => { UIManager.Instance.Show("BagPanel"); };
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button (2)")).onClick +=
            (go) => { GuideManager.Instance.Next(); };
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button (3)")).onClick +=
            (go) => { GuideManager.Instance.Next(); };

        GuideManager.Instance.Init(); 
    }

}
