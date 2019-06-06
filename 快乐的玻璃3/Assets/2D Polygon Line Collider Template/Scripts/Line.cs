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

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[DisallowMultipleComponent]
public class Line : MonoBehaviour
{
	/// <summary>
	/// The Points list of the Line .
	/// </summary>
	[HideInInspector]
	public List<Vector2> points;

	/// <summary>
	/// The Poins list of the polygon collider 2D.
	/// </summary>
	private List<Vector2> polygon2DPoints;

	/// <summary>
	/// The line renderer reference.
	/// </summary>
	private LineRenderer lineRenderer;

	/// <summary>
	/// The polygon collider of the line.
	/// </summary>
	private PolygonCollider2D polygonCollider2D;

	/// <summary>
	/// The rigid body of the line.
	/// </summary>
	private Rigidbody2D rigidBody2D;

	/// <summary>
	/// The material of the line renderer.
	/// </summary>
	public Material lineMaterial;

	/// <summary>
	/// The point Z position.
	/// </summary>
	public float pointZPosition = -3;

	/// <summary>
	/// The minimum offset between points in the line.
	/// </summary>
	private float pointMinOffset = 0.05f;

	/// <summary>
	/// A temp vector.
	/// </summary>
	private static Vector2 tempVector;

	/// <summary>
	/// The direction between two points.
	/// </summary>
	private static Vector2 direction;

	/// <summary>
	/// The angle between two points in rad.
	/// </summary>
	private static float angle;

	/// <summary>
	/// The half of the lines's width.
	/// </summary>
	private static float halfWidth;

	/// <summary>
	/// Whether to add a point to the collider or not.
	/// </summary>
	public bool autoAddColliderPoint = true;

	/// <summary>
	/// The max points.
	/// </summary>
	[Range(0,5000)]
	public float maxPoints = Mathf.Infinity;

	// Use this for initialization
	void Awake ()
	{
		points = new List<Vector2> ();
		polygon2DPoints = new List<Vector2> ();
		lineRenderer = GetComponent<LineRenderer> ();
		polygonCollider2D = GetComponent<PolygonCollider2D> ();
		rigidBody2D = GetComponent<Rigidbody2D> ();

		if (lineMaterial == null) {
			//Create the material of the line
			lineMaterial = new Material (Shader.Find ("Sprites/Default"));
		}

		lineRenderer.material = lineMaterial;
		halfWidth = lineRenderer.endWidth / 2.0f;
	}

	/// <summary>
	/// Adds new point.
	/// </summary>
	/// <param name="point">Vector3 Point.</param>
	public void AddPoint (Vector3 point)
	{
		//If the given point already exists ,then skip it
		if (points.Contains (point)) {
			return;
		}

		if (points.Count > 1) {
			if (Vector2.Distance (point, points [points.Count - 1]) < pointMinOffset) {
				return;//skip the point
			}
		}

		//z-position of the point
		point.z = pointZPosition;

		//Add the point to the points list
		points.Add (point);
		lineRenderer.numPositions++;
		lineRenderer.SetPosition (lineRenderer.numPositions - 1, point);

		if (autoAddColliderPoint) {
			//Add the point to the collider of the line
			AddPointToCollider (points.Count - 1);
		}
	}

	/// <summary>
	/// Enable the collider of the line.
	/// </summary>
	public void EnableCollider ()
	{
		polygonCollider2D.enabled = true;
	}

	/// <summary>
	/// Set the type of the rigid body.
	/// </summary>
	/// <param name="type">Type.</param>
	public void SetRigidBodyType(RigidbodyType2D type){
		rigidBody2D.bodyType = type;
	}

	/// <summary>
	/// Simulate the rigid body.
	/// </summary>
	public void SimulateRigidBody ()
	{
		rigidBody2D.simulated = true;
	}

	/// <summary>
	/// Whether reached points limit or not.
	/// </summary>
	/// <returns><c>true</c>, if limit was reacheded, <c>false</c> otherwise.</returns>
	public bool ReachedPointsLimit(){
		return points.Count >= maxPoints;
	}

	/// <summary>
	/// Add the last point to the collider of the line.
	/// </summary>
	/// <param name="index">The index of the point in the points list.</param>
	public void AddPointToCollider (int index)
	{
		direction = points [index] - points [index + 1 < points.Count ? index + 1 : (index - 1 >= 0 ? index - 1 : index)];
		angle = Mathf.Atan2 (direction.x, -direction.y);

		tempVector = points [index];
		tempVector.x = tempVector.x + halfWidth * Mathf.Cos (angle);
		tempVector.y = tempVector.y + halfWidth * Mathf.Sin (angle);
		polygon2DPoints.Insert (polygon2DPoints.Count, tempVector);

		tempVector = points [index];
		tempVector.x = tempVector.x - halfWidth * Mathf.Cos (angle);
		tempVector.y = tempVector.y - halfWidth * Mathf.Sin (angle);
		polygon2DPoints.Insert (0, tempVector);

		polygonCollider2D.points = polygon2DPoints.ToArray ();
	}
}
