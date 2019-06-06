using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class Move : MonoBehaviour {

	public float xspeed = -4f;
	public bool follow = false;

	// Update is called once per frame
	void Update ()
	{
		if (!follow)
			return;

		transform.Translate (Time.deltaTime * xspeed, 0, 0);
	}

	public void Follow(){
		follow = true;
	}

	public void Stop(){
		follow = false;
	}
}
