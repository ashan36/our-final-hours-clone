using UnityEngine;
using System.Collections;

public class ColliderTrigger : Trigger
{
    public bool triggerIsTripped = false;

    public Trigger triggerInstance;

    public override void Awake()
    {
        triggerInstance = this.GetComponent<ColliderTrigger>();
        triggerPosition = this.transform.position;
        RegisterWithManager();
    }

    public override void RegisterWithManager()
    {
        identifier = TriggerManager.Instance.RegisterTrigger(ref triggerInstance);
       // Debug.Log("Collider identifier = " + identifier);
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
