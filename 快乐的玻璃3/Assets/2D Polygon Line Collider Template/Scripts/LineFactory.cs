using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
public class LineFactory : MonoBehaviour
{
	public GameObject linePrefab;
	[HideInInspector]
	public Line currentLine;
	public Transform lineParent;
	public RigidbodyType2D lineRigidBodyType = RigidbodyType2D.Kinematic;
	public LineEnableMode lineEnableMode = LineEnableMode.ON_CREATE;
	public static LineFactory instance;
	public Image lineLife;
	public bool enableLineLife;
	public bool isRunning;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (lineParent == null) {
			lineParent = GameObject.Find ("Lines").transform;
		}

		if (lineLife != null) {
			if (enableLineLife) {
				lineLife.gameObject.SetActive (true);
			} else {
				lineLife.gameObject.SetActive (false);
			}
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isRunning) {
			return;
		}

		if (Input.GetMouseButtonDown (0)) {
			CreateNewLine ();
		} else if (Input.GetMouseButtonUp (0)) {
			RelaseCurrentLine ();
		}

		if (currentLine != null) {
			currentLine.AddPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			UpdateLineLife ();
			if (currentLine.ReachedPointsLimit ()) {
				RelaseCurrentLine ();
			}
		}
	}

	private void CreateNewLine ()
	{
		currentLine = (Instantiate (linePrefab, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Line> ();
		currentLine.name = "Line";
		currentLine.transform.SetParent (lineParent);
		currentLine.SetRigidBodyType (lineRigidBodyType);

		if (lineEnableMode == LineEnableMode.ON_CREATE) {
			EnableLine ();
		}
	}

	private void EnableLine ()
	{
		currentLine.EnableCollider ();
		currentLine.SimulateRigidBody ();
	}

	private void RelaseCurrentLine ()
	{
		if (lineEnableMode == LineEnableMode.ON_RELASE) {
			EnableLine ();
		}

		currentLine = null;
	}

	private void UpdateLineLife ()
	{
		if (!enableLineLife) {
			return;
		}

		if (lineLife == null) {
			return;
		}

		lineLife.fillAmount = 1 - (currentLine.points.Count / currentLine.maxPoints);
	}

	public enum LineEnableMode
	{
		ON_CREATE,
		ON_RELASE}

	;
}
