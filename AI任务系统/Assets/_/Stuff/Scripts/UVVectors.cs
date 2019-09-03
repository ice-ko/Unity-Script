using UnityEngine;
using System.Collections;

[System.Serializable]

public class UVVectors {
	public Vector2 Vector_1,Vector_2,Vector_3,Vector_4;
	
	public UVVectors(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) {
		Vector_1 = v1;
		Vector_2 = v2;
		Vector_3 = v3;
		Vector_4 = v4;
	}
}