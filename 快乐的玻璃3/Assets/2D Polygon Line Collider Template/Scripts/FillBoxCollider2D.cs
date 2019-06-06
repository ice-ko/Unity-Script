using UnityEngine;
using System.Collections;

/*
 * 2D Polygon Line Collider Package
 *
 * @license		    Unity Asset Store EULA https://unity3d.com/legal/as_terms
 * @author		    Indie Studio - Baraa Nasser
 * @Website		    https://indiestd.com
 * @Asset Store     https://www.assetstore.unity3d.com/en/#!/publisher/9268
 * @Unity Connect   https://connect.unity.com/u/5822191d090915001dbaf653/column
 * @email		    info@indiestd.com
 *
 */

[DisallowMultipleComponent]
public class FillBoxCollider2D : MonoBehaviour {

	public float widthFillRatio = 1;
	public float heightFillRatio = 1;

	// Use this for initialization
	void Start () {
		RectTransform rectTransform = GetComponent<RectTransform> ();

		if (rectTransform == null) {
			return;
		}

		float width = rectTransform.rect.size.x;
		float height = rectTransform.rect.size.y;
			
		BoxCollider2D  [] colliders2D = GetComponents<BoxCollider2D> ();
		foreach (BoxCollider2D c in colliders2D) {
			c.size = new Vector2 (width * widthFillRatio, height * heightFillRatio);
		}
	}

}
