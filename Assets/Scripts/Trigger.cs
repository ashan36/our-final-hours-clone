using UnityEngine;
using System;
using System.Collections;

public class Trigger : MonoBehaviour {

    public delegate void activateTriggerDelegate();
    public event activateTriggerDelegate tripTrigger;

    public Vector3 triggerPosition { get; set; }
    public int identifier { get; set; }

    public virtual void Awake ()
    {
        triggerPosition = this.transform.position;
        RegisterWithManager();
    }

    public virtual void RegisterWithManager()
    {

    }

    protected virtual void OnTriggered()
    {
        activateTriggerDelegate handler = tripTrigger;
        if (handler != null)
        {
            handler();
        }
    }

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
