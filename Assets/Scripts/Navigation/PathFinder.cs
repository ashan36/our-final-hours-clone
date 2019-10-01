using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFinder : MonoBehaviour
{
    NavGrid grid;

    void Awake()
    {
        grid = GetComponent<NavGrid>();
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(CreatePath(startPos, targetPos));
    }

    IEnumerator CreatePath(Vector3 startPos, Vector3 targetPos)
    {
        NavNode startNode = grid.GetNodeFromWorld(startPos);
        NavNode targetNode = grid.GetNodeFromWorld(targetPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Debug.Log("Creating Path");
        if (startNode.walkable && targetNode.walkable)
        {
            Debug.Log("Creating Path inside first check");
            Heap<NavNode> openSet = new Heap<NavNode>(grid.MaxSize);
            HashSet<NavNode> closedSet = new HashSet<NavNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                NavNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    Debug.Log("Path Found!");
                    pathSuccess = true;
                    break;
                }

                foreach (NavNode neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                        else
                            openSet.UpdateItem(neighbor);
                    }
                }
            }
        }
        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        PathRequestManager.Instance.ProcessingComplete(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(NavNode startNode, NavNode endNode)
    {
        List<NavNode> path = new List<NavNode>();
        NavNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        //Vector3[] waypoints = new Vector3[path.Count];
        //for (int i = 0; i < path.Count; i++)
        //{
        //    waypoints[i] = path[i].worldPosition;
        //}
        //Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<NavNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i-1].worldPosition);
            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
    }

    int GetDistance(NavNode nodeA, NavNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return (141 * dstY) + (100 * (dstX - dstY));
        else if (dstX < dstY)
            return (141 * dstX) + (100 * (dstY - dstX));
        else
            return 141 * dstX;
    }
}