using UnityEngine;
using System.Collections;

public class TerminalInteraction : Trigger

{
    public bool On;
    Animator anim;

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

        anim = GetComponentInChildren<Animator>();
        currentSprite = GetComponentInChildren <SpriteRenderer>();
    }

    public override void RegisterWithManager()
    {
        identifier = managerRef.RegisterTrigger(ref triggerInstance);
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(triggerPosition - PlayerController.playerTrans.position) < 0.65f)
        {
            if (Input.GetButtonDown("Use"))
            {
                if (!On)
                {
                    On = true;
                    OnTriggered();
                    currentSprite.sprite = onSprite;
                }
                else
                {
                    On = false;
                    OnTriggered();
                    currentSprite.sprite = offSprite;
                }
            }
        }
    }
}
