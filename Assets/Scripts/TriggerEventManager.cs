using UnityEngine;
using System.Collections;

public class TriggerEventManager : MonoBehaviour {

    public delegate void activateTriggerDelegate();
    public static event activateTriggerDelegate tripTrigger;

    public bool triggerisTripped = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter (Collider other)
    {
        GameObject collidedWith = other.gameObject;

        if (collidedWith.tag == "Player" && !triggerisTripped)
        {
            tripTrigger();
            triggerisTripped = true;
        }
    }
}
