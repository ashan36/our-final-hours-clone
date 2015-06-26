using UnityEngine;
using System.Collections;

public class TerminalInteraction : Trigger, IInteractable

{
    public bool On;

    public Trigger triggerInstance;
    TriggerManager managerRef;

    public Sprite onSprite;
    public Sprite offSprite;
    SpriteRenderer currentSprite;

    public override void Awake()
    {
        managerRef = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<TriggerManager>();
        triggerInstance = this;
        triggerPosition = this.transform.position;
        RegisterWithManager();
		
        currentSprite = GetComponentInChildren <SpriteRenderer>();
    }

    public override void RegisterWithManager()
    {
        identifier = managerRef.RegisterTrigger(ref triggerInstance);
        Debug.Log("Terminal identifier = " + identifier);
    }

	// Use this for initialization
	void Start () 
    {
        NotificationsManager.DefaultNotifier.AddObserver(this, "OnPlayerInteract");
	}
	
	// Update is called once per frame
    public void OnPlayerInteract()
    {
        StartCoroutine(Switch());
    }

    public IEnumerator Switch()
    {
        Debug.Log("Switching");
        Debug.Log("Distance: " + Vector3.Magnitude(triggerPosition - PlayerController.playerTrans.position));
        if (Vector3.Magnitude(triggerPosition - PlayerController.playerTrans.position) < 0.8f)
        {
            Debug.Log("Switching in range");
            if (!On)
            {
                On = true;
                OnTriggered();
                currentSprite.sprite = onSprite;
                yield return new WaitForSeconds(0.4f);
            }

            else
            {
                On = false;
                OnTriggered();
                currentSprite.sprite = offSprite;
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}
