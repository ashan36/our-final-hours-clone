using System;
using System.Collections;
using UnityEngine;

public class AINavAgent : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float heightOffset;

    bool followingPath = false;

    Vector3 currentWaypoint;
    Vector3 destination;
    Vector3[] path;
    int targetIndex;

    //For moving target tracking
    Vector3 previousPredictedTargetPos;
    float timer;


    void Update()
    {
        timer += Time.deltaTime;
    }

    public void SetDestination(Vector3 dest)
    {
        destination = dest;
        Debug.Log("Destination set");

        if (!followingPath)
            PathRequestManager.Instance.RequestPath(transform.position, destination, OnPathFound);
        else
        {
            Vector3 agentPredictedPos = Vector3.MoveTowards(transform.position, currentWaypoint, speed/1.6f);
            PathRequestManager.Instance.RequestPath(agentPredictedPos, destination, OnPathFound);
        }
    }

    public void TrackMovingTarget(IAITrackable TargetObject)
    {
        float targetSpeed = TargetObject.speed;
        float distToTarget = Vector3.Distance(TargetObject.trackingTransform.position, transform.position);
        Vector3 currentPredictedTargetPos = TargetObject.trackingTransform.position + TargetObject.currentHeading * targetSpeed * distToTarget/10;

        if (previousPredictedTargetPos != null)
        {
            Debug.Log("Target Prediction difference = " + Vector3.Distance(currentPredictedTargetPos, previousPredictedTargetPos));
            if (Vector3.Distance(currentPredictedTargetPos, previousPredictedTargetPos) > 2)
                SetDestination(currentPredictedTargetPos);
        }

        if (timer >= 1f)
        {
            timer = 0;
            previousPredictedTargetPos = TargetObject.trackingTransform.position + TargetObject.currentHeading * targetSpeed * distToTarget/10;
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        followingPath = false;
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
        followingPath = true;
        Quaternion rotation;

        targetIndex = 0;

        currentWaypoint = path[targetIndex];
        Debug.Log("Following Path");
        Debug.Log("Path Length: " + path.Length);

        while (true)
        {
            if ((currentWaypoint - transform.position).magnitude < 0.2f)
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
            currentWaypoint.y = heightOffset;

            if ((currentWaypoint - transform.position).sqrMagnitude > 2)
            {
                rotation = Quaternion.LookRotation(currentWaypoint - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }

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
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(previousPredictedTargetPos, 0.5f);
    }
}
