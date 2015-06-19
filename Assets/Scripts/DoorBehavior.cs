using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour, IEventListener
{

    BoxCollider BoxCollider;
    Material doorMat;
    public bool open;

    public IEventListener doorBehaviorInstance;

    public Trigger wiredTrigger { get; set; }
    public Vector3 objectPosition { get; set; }
    public int identifier { get; set; }
    public TriggerManager managerRef { get; set; }

    public void Awake ()
    {
        BoxCollider = GetComponent<BoxCollider>();
        doorMat = GetComponent<MeshRenderer>().material;


        managerRef = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<TriggerManager>();
        doorBehaviorInstance = this.GetComponent<DoorBehavior>();
        objectPosition = this.transform.position;
        ConnectToTrigger();
    }

    public void ConnectToTrigger()
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

    public void OnDisable()
    {
        wiredTrigger.tripTrigger -= doAction;
    }

    public void doAction ()
    {
        if (!open)
        {
            BoxCollider.enabled = false;
            Color color = doorMat.color;
            color.a = 0f;
            doorMat.color = color;
            open = true;
        }
        else
        {
            BoxCollider.enabled = true;
            Color color = doorMat.color;
            color.a = 1f;
            doorMat.color = color;
            open = false;
        }
    }
  
}
