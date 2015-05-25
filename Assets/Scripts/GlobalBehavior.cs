using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalBehavior : MonoBehaviour {

    static int maxCasings = 10;
    static bool casingLimit = false;
    static Queue<GameObject> casingStack = new Queue<GameObject>(20);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void CasingSpawned(GameObject casing)
    {
        if (casingLimit)
        {
            Destroy(casingStack.Dequeue());
        }

        casingStack.Enqueue(casing);
        if (casingStack.Count == maxCasings)
            casingLimit = true;

    }


}
