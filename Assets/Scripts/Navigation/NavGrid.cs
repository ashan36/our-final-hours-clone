using System;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;

    Vector2 gridWorldSize;
    public float nodeDiameter;
    float nodeRadius;

    MapData mapObject;

    NavNode[,] grid;

    int gridSizeX, gridSizeY;
    public int MaxSize { get { return gridSizeX * gridSizeY; } }

    void Awake()
    {
        mapObject = this.GetComponent<MapData>();
        nodeRadius = nodeDiameter / 2;
    }

    void Start()
    {
        gridWorldSize = GetGridWorldSize();
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    Vector2 GetGridWorldSize()
    {
        gridWorldSize.x = mapObject.mapX * mapObject.tileSize;
        gridWorldSize.y = mapObject.mapY * mapObject.tileSize;

        return gridWorldSize;
    }

    void CreateGrid()
    {
        grid = new NavNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = Vector3.zero;
        worldBottomLeft.x = -(float)((mapObject.tileSize) / 2);
        worldBottomLeft.z = -(float)((mapObject.tileSize) / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new NavNode(walkable, worldPoint, x, y);
            }
        }
    }

    public List<NavNode> GetNeighbors(NavNode node)
    {
        List<NavNode> neighbors = new List<NavNode>(8);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbors.Add(grid[checkX, checkY]);
            }
        }
        return neighbors;
    }

    public NavNode GetNodeFromWorld(Vector3 worldPosition)
    {
        int x;
        int y;

        x = Mathf.RoundToInt((worldPosition.x / nodeDiameter) + (mapObject.tileSize / 2) - nodeRadius);
        y = Mathf.RoundToInt((worldPosition.z / nodeDiameter) + (mapObject.tileSize / 2) - nodeRadius);

        if (x < 0 || y < 0)
        Debug.Log("X: " + x + " Y: " + y);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        if (grid != null)
        {
            NavNode playerNode = GetNodeFromWorld(PlayerController.playerTrans.position);
            foreach (NavNode n in grid)
            {
                Gizmos.color = (n.walkable) ? new Color(1,1,1,0.2f) : Color.red;
                if (playerNode == n)
                    Gizmos.color = Color.cyan;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .05f));
            }
        }
    }

}

public class NavNode : IHeapItem<NavNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public NavNode parent;

    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get { return gCost + hCost; } }

    public int gridX { get; set; }
    public int gridY { get; set; }

    private int heapIndex;

    public NavNode(bool _walkable, Vector3 worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(NavNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}