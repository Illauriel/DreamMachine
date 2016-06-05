using UnityEngine;
using System.Collections;

public class SquareGrid : MonoBehaviour {

	public GameObject tile;
	Vector3[] coords;
	// Use this for initialization
	void Awake () {
		coords = GenerateGridCoords(20, 20, new Vector3(-9.5f, 0.2f, -9.5f));
		GameObject tile_container = new GameObject("GridTiles");
		for (int i = 0; i < coords.Length; i++) {
			GameObject new_tile = (GameObject) Instantiate(tile, coords[i], tile.transform.rotation);
			new_tile.transform.parent = tile_container.transform;
			new_tile.AddComponent<Node>();
			new_tile.GetComponent<MeshRenderer>().enabled = false;
			new_tile.name = "Tile("+i+")";
		}
		//tile_container.SetActive(false);
	}

	public Vector3[] GenerateGridCoords(int width, int height, Vector3 offset){
		Vector3[] result = new Vector3[width * height];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				int index = width * i + j;

				result[index] = new Vector3(offset.x+i, offset.y, offset.z+j);

			}
		}
		return result;
	}
}
