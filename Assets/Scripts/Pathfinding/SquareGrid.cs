using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquareGrid : MonoBehaviour {


	public GameObject tile;
	public Material gridMaterial;
	public int gridWidth;
	public int gridHeight;
	public float gridElevation; // height of grid above surface
	public Vector2 spacing = new Vector2(1, 1);
	public Vector3 offset;

	GameObject gridHolder;


	public float[] moveCosts;
	Vector3[] coords;
	//public string[] vertical_neighbors;

	//Grid data
	public GridCell[] all_cells;
	public int[] cell_status;
	//public bool[] impassible;
	//public GridUI gui;
	public int[,] neighbor_ids; // stores 4 id indices for each cell


	// Use this for initialization
	void Awake () {
		Vector3[] ngrd = NewGridCoords(gridWidth, gridHeight, offset);
		int layerMask = 1 << LayerMask.NameToLayer("NavMesh");
		coords = HitCoords(ngrd, layerMask);
	
		CreateGridObj();

		all_cells = Coords2Cells(coords);
		//gui.MarkGrid(all_cells);
		neighbor_ids = new int[all_cells.Length, 4];
		for (int i = 0; i < all_cells.Length; i++) {
			int[] cell_nghbrs = FindNeighbors(i); 
			for (int j = 0; j < cell_nghbrs.Length; j++) {
				neighbor_ids[i,j] = cell_nghbrs[j];
			}

		}
	
		RefreshCellStatus();
		//tile_container.SetActive(false);
		//impassible = new bool[all_cells.Length];
	}
	/*void Update(){
		for (int i = 0; i < coords.Length; i++) {
			Debug.DrawRay(coords[i], Vector3.down*100, Color.blue);
		}
	}*/

	public Vector3[] NewGridCoords(int width, int height, Vector3 offset){
		Vector3[] result = new Vector3[width * height];
		//Debug.Log(width+ " * "+ height+" = "+result.Length);
		Vector3 pos = transform.position;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				int index = height * i + j;
				//Debug.Log(height+"*"+i+"+"+j+"="+index);
				result[index] = new Vector3(pos.x + offset.x + i*spacing.x, pos.y + offset.y, pos.z + offset.z + j*spacing.y);

			}
		}
		return result;
	}

	public Vector3[] HitCoords(Vector3[] origins){
		int all_layers = 1 << 2;
		all_layers = ~all_layers;
		Vector3[] result = HitCoords(origins, all_layers);
		return result;
	}


	///Fires Raycasts downwards to find grid positions on geometry
	public Vector3[] HitCoords(Vector3[] origins, int layer_mask){
		List<Vector3> result = new List<Vector3>();
		//List<Vector3> multihits = new List<Vector3>();
		//List<string> multilayer = new List<string>();
		float depth = 100;
		for (int i = 0; i < origins.Length; i++) {
			Ray ray = new Ray(origins[i], Vector3.down*depth);
			RaycastHit[] hits = Physics.RaycastAll(ray, depth, layer_mask);
		//	string mul_descr = i+"";

			for (int j = 0; j < hits.Length; j++) {
				//Debug.Log(hits[j].collider.name +" hit at " +hits[j].point);
				result.Add(hits[j].point + Vector3.up * gridElevation);


			}
		

		}
		//layered_coords = multihits.ToArray();
		//vertical_neighbors = multilayer.ToArray();
		Debug.Log (result.Count);
		return result.ToArray();
	}

	void MultilayerCheck(){

	}

	Mesh CreateGridMesh(Vector3[] coords){
		Mesh result = new Mesh();
		Mesh mesh = MeshGen.Square(0.5f);
		CombineInstance[] combineDetails = new CombineInstance[coords.Length];
		for (int i = 0; i < coords.Length; i++) {
			Matrix4x4 translation = TranslationMatrix(coords[i]);

			combineDetails[i].mesh = mesh;
			combineDetails[i].transform = translation;
		}

		result.CombineMeshes(combineDetails);
		return result;
	}

	void CreateGridObj(){
		gridHolder = new GameObject("TheGrid");
		MeshFilter mf = gridHolder.AddComponent <MeshFilter>();
		MeshRenderer mr = gridHolder.AddComponent <MeshRenderer>();

		mf.mesh = CreateGridMesh(coords);//MeshGen.Square(0.5f);
		mf.mesh.name = "GridMesh";
		RecalculateMeshHeights(mf.mesh);
		mf.mesh.RecalculateBounds();
		mf.mesh.RecalculateNormals();
		mr.material = gridMaterial;//tile.GetComponent<MeshRenderer>().sharedMaterial;
	}

	public static Matrix4x4 TranslationMatrix(Vector3 offset) {
		Matrix4x4 matrix = Matrix4x4.identity;
		matrix.m03 = offset.x;
		matrix.m13 = offset.y;
		matrix.m23 = offset.z;
		return matrix;
	}

	void RecalculateMeshHeights(Mesh mesh){
		Vector3[] newVerts = new Vector3[mesh.vertexCount];
		for (int i = 0; i < mesh.vertices.Length; i++) {

			newVerts[i] = CheckVertHeight(mesh.vertices[i], 0.5f);
		}
		mesh.vertices = newVerts;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	Vector3 CheckVertHeight(Vector3 origin, float margin){
		Vector3 result = origin;
		Ray ray_up = new Ray(origin, Vector3.up);
		Ray ray_down = new Ray (origin, Vector3.down);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray_up, out hit, margin)){
			result = new Vector3(origin.x, hit.point.y + gridElevation, origin.z);
			//Debug.Log(result);
		}
		else if (Physics.Raycast(ray_down, out hit, margin)){
			result = new Vector3(origin.x, hit.point.y + gridElevation, origin.z);

		}

		return result;
	}

	GridCell[] Coords2Cells(Vector3[] vectors){
		GridCell[] result = new GridCell[vectors.Length];
		for (int i = 0; i < vectors.Length; i++) {
			result[i] = GetCell(vectors[i]);
		}
		return result;
	}

	public GridCell GetCell(Vector3 pos){
		int new_x = Mathf.FloorToInt(pos.x);
		int new_y = Mathf.FloorToInt(pos.z);

		GridCell result = new GridCell(new_x, new_y, pos.y);
		return result;
	}

	int[] FindNeighbors(int id){
		int[] results = new int[4];
		for (int i = 0; i < results.Length; i++) {
			results[i] = -1;
			GridCell n_cell = all_cells[id].Neighbor(i);
			int cell_id = GetCellId(n_cell);
			//invalidates neighbors with too much Z diff
			if (cell_id != -1){
				float z_diff = Mathf.Abs(all_cells[id].z - all_cells[cell_id].z);
				if (z_diff <= 1){
					results[i] = cell_id;
				}
			}
		}
		return results;
	}

	/// <summary>
	/// Gets the cell identifier by iterating through all cells and choses one that is closer on alt axis.
	/// </summary>
	/// <param name="cell">GridCell that .</param>
	public int GetCellId(GridCell cell){
		int result = -1;
		float best_z_diff = Mathf.Infinity;
		for (int i = 0; i < all_cells.Length; i++) {

			if (all_cells[i].x == cell.x && all_cells[i].y == cell.y){
				float z_diff = Mathf.Abs(cell.z - all_cells[i].z);
				if (z_diff < best_z_diff){
					//Debug.Log(z_diff);
					best_z_diff = z_diff;
					result = i;
				}
				//
			}
		}
		//Debug.Log(cell.x+":"+cell.y+" id is "+result);
		return result;
	}

	public int GetCellId(Vector3 point){
		GridCell cell = GetCell(point);
		return GetCellId(cell);
	}



	public void SwitchPassible(int id){
		cell_status[id] ++;
		if (cell_status[id] >= moveCosts.Length){
			cell_status[id] = 0;
		}
	}

	public float Distance(int id1, int id2){
		if (id1 != -1 && id2 != -1){
			Vector3 pos1 = all_cells[id1].Position();
			Vector3 pos2 = all_cells[id2].Position();
			return Vector3.Distance(pos1, pos2);		
		}
		else{
			return Mathf.Infinity-1;
		}

	}

	public int[] GetNeighbors(int id){
		int[] result = new int[neighbor_ids.GetLength(1)];
		for (int i = 0; i < result.Length; i++) {
			result[i] = neighbor_ids[id, i]; 
		}
		return result;
	}

	GridCell[] Ids2CellArray(int[] ids){
		GridCell[] result = new GridCell[ids.Length];
		for (int i = 0; i < ids.Length; i++) {
			//Debug.Log(i + " "+ids[i]);
			result[i] = all_cells[ids[i]];
		}
		return result;
	}

	public void RefreshCellStatus(){
		cell_status = new int[all_cells.Length];
		for (int i = 0; i < all_cells.Length; i++) {
			cell_status[i] = GetCellStatus(i);
		}
	}

	int GetCellStatus(int id){
		int result = -1;
		Ray ray = new Ray(all_cells[id].Position()-Vector3.up*0.2f, Vector3.up);
		RaycastHit hit = new RaycastHit();
		int layerMask = 1 << LayerMask.NameToLayer("NavMesh");
		//results 0-free, 1-blocked, 2 - hard, 3 - player/ally, 4 - enemy, 5 - hazard

		if (Physics.Raycast(ray, out hit,1, ~layerMask)){
			if (hit.collider.tag == "Blocked"){
				result = 1;
			}
			else if (hit.collider.tag == "Hard"){
				result = 2;
			}
			else if (hit.collider.tag == "Player" || hit.collider.tag == "Ally"){
				Debug.Log("Hit Player!");
				result = 3;
			}
			else if (hit.collider.tag == "Enemy"){
				Debug.Log("Hit Enemy!");
				result = 4;
			}
		}
		else {
			result = 0;
		}
		return result;
	}

	public float GetCost(int id){
		if (cell_status[id] <=2 && cell_status[id] != -1){
			return moveCosts[cell_status[id]];
		}
		else {
			return Mathf.Infinity;
		}
	}

	public GameObject GridContainer{
		get{return gridHolder;}
	}
}
