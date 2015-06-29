using UnityEngine;
using System.Collections;

public class MapData : MonoBehaviour {

	public int [,] tile;
	int x,y;
	int roadVar;
	public int tileSize;
	public int mapX;
	public int mapY;
	public int homeX;
	public int homeY;

	public GameObject mapTileStart;
	public GameObject mapTileA;
	public GameObject mapTileB;
	public GameObject mapTileC;
	public GameObject mapHouseA;
	public GameObject mapTileRoadA;
	public GameObject mapTileRoadB;

	// Use this for initialization
	void Awake () 
	{

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
				else if (x > homeX && y == homeY)
				{
					SpawnRoad (x,y);
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
	

		if (tile[x,y] == 0)
		{
			Instantiate(mapHouseA, new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
		}
		else if (tile[x,y] == 1)
		{
			Instantiate(mapTileA, new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
		}
		else if (tile[x,y] == 2)
		{
			Instantiate(mapTileB, new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
		}
		else if (tile[x,y] == 3)
		{
			Instantiate(mapTileC, new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
		}
	}

	void SpawnHomeTile (int x, int y) 
	{
		Instantiate(mapTileStart, new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
		Debug.Log ("X: " + x + ", Y: " + y + ", MapPiece: " + tile[x,y]);
	}

	void SpawnRoad (int x, int y) 
	{
		roadVar = Random.Range (0,2);
		if (roadVar == 0)
		{
			Instantiate(mapTileRoadA, new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
			Debug.Log ("X: " + x + ", Y: " + y + ", MapPiece: " + tile[x,y]);
		}
		else if (roadVar == 1)
		{
			Instantiate(mapTileRoadB, new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
			Debug.Log ("X: " + x + ", Y: " + y + ", MapPiece: " + tile[x,y]);
		}
	}
}
