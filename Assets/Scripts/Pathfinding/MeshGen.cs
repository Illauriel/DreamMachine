using UnityEngine;
using System.Collections;

public class MeshGen : MonoBehaviour {

	public static Mesh Square(float extent){
		Mesh result = new Mesh();

		Vector3[] vertices = new Vector3[]
		{
			new Vector3( extent, 0,  extent),
			new Vector3( extent, 0, -extent),
			new Vector3(-extent, 0,  extent),
			new Vector3(-extent, 0, -extent)
		};

		Vector2[] uv = new Vector2[]
		{
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, 1),
			new Vector2(0, 0)
		};

		int[] triangles = new int[]
		{
			0, 1, 2,
			2, 1, 3
		};

		result.vertices = vertices;
		result.uv = uv;
		result.triangles = triangles;

		return result;
	}
}
