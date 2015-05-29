using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class EventBaseClass : MonoBehaviour
{
    public Trigger wiredTrigger;
    public Vector3 objectPosition { get; set; }
    public int identifier { get; set; }
    TriggerManager managerRef;

    public virtual void Awake ()
    {
        managerRef = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<TriggerManager>();
        objectPosition = this.transform.position;
        ConnectToTrigger ();
    }

    public virtual void ConnectToTrigger ()
    {
        wiredTrigger = managerRef.getTrigger(identifier);
    }

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public abstract void OnDisable();
   

    public abstract void doAction();
}