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
public class CameraFollow : MonoBehaviour
{
	// Distance in the x axis the target can move before the camera follows.
	public float xMargin = 1f;
	// Distance in the y axis the target can move before the camera follows.
	public float yMargin = 1f;
	// How smoothly the camera catches up with it's target movement in the x axis.
	public float xSmooth = 8f;
	// How smoothly the camera catches up with it's target movement in the y axis.
	public float ySmooth = 8f;
	// Reference to the target's transform.
	public Transform target;
	// Reference to the target's x postion.
	private float targetX;
	// Reference to the target's y postion.
	private float targetY;
	// Whether to follow the target's x position or not
	public bool followX = true;
	// Whether to follow the target's y position or not
	public bool followY = true;

	void Start ()
	{
	}

	void Update ()
	{
		TrackTarget ();
	}

	void TrackTarget ()
	{
		if (target == null) {
			return;
		}

		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		targetX = transform.position.x;
		targetY = transform.position.y;

		if (followX) {
			// ... the target x coordinate should be a Lerp between the camera's current x position and the target's current x position.
			targetX = Mathf.Lerp (transform.position.x, target.position.x - xMargin, xSmooth * Time.deltaTime);
		}

		if (followY) {
			// ... the target y coordinate should be a Lerp between the camera's current y position and the target's current y position.
			targetY = Mathf.Lerp (transform.position.y, target.position.y - yMargin, ySmooth * Time.deltaTime);
		}

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3 (targetX, targetY, transform.position.z);
	}
}
