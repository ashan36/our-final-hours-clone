using UnityEngine;
using System.Collections;

public class DoorBehavior : EventBaseClass {

    BoxCollider BoxCollider;
    Material doorMat;

    public EventBaseClass doorBehaviorInstance;
    TriggerManager managerRef;

    public override void Awake ()
    {
        BoxCollider = GetComponent<BoxCollider>();
        doorMat = GetComponent<MeshRenderer>().material;


        managerRef = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<TriggerManager>();
        doorBehaviorInstance = this.GetComponent<DoorBehavior>();
        objectPosition = this.transform.position;
        ConnectToTrigger();
    }

    public override void ConnectToTrigger()
    {
        identifier = managerRef.RegisterEvent(ref doorBehaviorInstance);
    }

	// Use this for initialization
	void Start () 
    {

        wiredTrigger = managerRef.getTrigger(identifier);
        wiredTrigger.tripTrigger += new Trigger.activateTriggerDelegate(doAction);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnDisable()
    {
        wiredTrigger.tripTrigger -= doAction;
    }

    void OnCollisionEnter (Collision other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            BoxCollider.enabled = false;
            Color color = doorMat.color;
            color.a = 0f;
            doorMat.color = color;
        }
    }

    public override void doAction ()
    {
        BoxCollider.enabled = false;
        Color color = doorMat.color;
        color.a = 0f;
        doorMat.color = color;
    }
  
}
