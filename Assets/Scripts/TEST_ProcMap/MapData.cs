using UnityEngine;
using System.Collections;

public class MapData : MonoBehaviour {

	public int [,] tile;
	int x,y;
	public int mapX;
	public int mapY;
	public int homeX;
	public int homeY;

	public GameObject mapTileStart;
	public GameObject mapTileA;
	public GameObject mapTileB;
	public GameObject mapTileC;

	Material theMat;
	public Material mat1;
	public Material mat2;
	public Material mat3;

	// Use this for initialization
	void Awake () 
	{
		theMat = mapTileStart.GetComponent <Material> ();

		tile = new int[mapX, mapY];

		for (int x = 0; x < mapX; x++)
		{
			for (int y = 0; y < mapY; y++)
			{
				tile[x,y] = Random.Range (0,4);
				if (x == homeX && y == homeY)
				{
					SpawnHomeTile (x,y);
				}
				else
				{
					SpawnMapTile (x,y);
				}
			}
		}


	}

	void SpawnMapTile (int x, int y) 
	{
		if (tile[x,y] == 1)
		{
			Instantiate(mapTileA, new Vector3(x*8,0,y*8), Quaternion.identity);
		}
		else if (tile[x,y] == 2)
		{
			Instantiate(mapTileB, new Vector3(x*8,0,y*8), Quaternion.identity);
		}
		else if (tile[x,y] == 3)
		{
			Instantiate(mapTileC, new Vector3(x*8,0,y*8), Quaternion.identity);
		}
	}

	void SpawnHomeTile (int x, int y) 
	{
		Instantiate(mapTileStart, new Vector3(x*8,0,y*8), Quaternion.identity);
		Debug.Log ("X: " + x + ", Y: " + y + ", MapPiece: " + tile[x,y]);
	}
}
