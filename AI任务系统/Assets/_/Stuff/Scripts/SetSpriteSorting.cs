using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetSpriteSorting : MonoBehaviour {

    public string sortingLayerName;
    public int sortingOrder;

    void Awake() {
        Refresh();
    }
    void Refresh() {
        transform.GetComponent<MeshRenderer>().sortingLayerName = sortingLayerName;
        transform.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
    }
    #if UNITY_EDITOR
	void Update () {
		if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
			//this.enabled = false;
		} else {
			// editor code here!
            Refresh();
		}
	}
    #endif
}
