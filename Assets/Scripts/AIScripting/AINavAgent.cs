using System;
using System.Collections;
using UnityEngine;

public class AINavAgent : MonoBehaviour
{
    public float speed;
    public float heightOffset;
    Vector3 destination;
    Vector3[] path;
    int targetIndex;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetDestination(PlayerController.playerTrans.position);
    }

    public void SetDestination(Vector3 dest)
    {
        destination = dest;
        Debug.Log("Destination set");
        PathRequestManager.Instance.RequestPath(transform.position, destination, OnPathFound);
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public void Resume()
    {
        PathRequestManager.Instance.RequestPath(transform.position, destination, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathFound)
    {
        if (pathFound)
        {
            path = newPath;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        targetIndex = 0;
        Vector3 currentWaypoint = path[targetIndex];
        Debug.Log("Following Path");
        Debug.Log("Path Length: " + path.Length);

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                Debug.Log("WayPoint Reached");
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    Debug.Log("Destination Reached");
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            Debug.Log("Index: " + targetIndex);
            currentWaypoint.y = heightOffset;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], Vector3.one * 0.5f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }
}
