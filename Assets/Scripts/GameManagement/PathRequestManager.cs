using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    PathFinder pathFinder;

    bool isProcessing;

    private static PathRequestManager instance = null;

    public static PathRequestManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("PathRequestManager").AddComponent<PathRequestManager>(); //create object if required
            return instance;
        }
    }

    void Awake()
    {
        if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
            DestroyImmediate(gameObject); //Delete duplicate
        else
        {
            instance = this; //Make this object the only instance
            DontDestroyOnLoad(gameObject); //Set as do not destroy
        }

        pathFinder = GameObject.FindGameObjectWithTag("Navigation").GetComponent<PathFinder>();
    }

    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessing && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessing = true;
            pathFinder.FindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void ProcessingComplete(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessing = false;
        TryProcessNext();
    }    int gridSizeX, gridSizeY;

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
