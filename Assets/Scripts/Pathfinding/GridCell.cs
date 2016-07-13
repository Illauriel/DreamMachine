using UnityEngine;
using System.Collections;

public struct GridCell{
	/// <summary>
	/// The cell's x coordinate on world x axis.
	/// </summary>
	public readonly int x;

	/// <summary>
	/// The cell's y coordinate on world z axis.
	/// </summary>
	public readonly int y;

	public readonly float z;

	public GridCell(int x, int y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}


	public Vector3 Position(){
		return new Vector3(x * 1+0.5f, z, y * 1+0.5f);
	}
	/// <summary>
	/// Returns neighbor with the specified index.
	/// </summary>
	/// <param name="index">Index 0 is to the right, others are counterclockwise.</param>
	public GridCell Neighbor(int index){
		
		return new GridCell(neighbors[index].x + this.x, neighbors[index].y + this.y, this.z);
	}

	///directions array
	static readonly GridCell[] neighbors = {

		new GridCell(1, 0, 0),
		new GridCell(0,-1, 0),
		new GridCell(-1,0, 0),
		new GridCell(0, 1, 0)
	};
}
