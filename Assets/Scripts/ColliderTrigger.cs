using UnityEngine;
using System.Collections;

public class ColliderTrigger : Trigger
{
    public bool triggerIsTripped = false;

    public Trigger triggerInstance;
    TriggerManager managerRef;

    public override void Awake()
    {
        managerRef = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<TriggerManager>();
        triggerInstance = this.GetComponent<ColliderTrigger>();
        triggerPosition = this.transform.position;
        RegisterWithManager();
    }

    public override void RegisterWithManager()
    {
        identifier = managerRef.RegisterTrigger(ref triggerInstance);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedWith = other.gameObject;

        if (collidedWith.tag == "Player" && !triggerIsTripped)
        {
            OnTriggered();
            triggerIsTripped = true;
        }
    }
}
