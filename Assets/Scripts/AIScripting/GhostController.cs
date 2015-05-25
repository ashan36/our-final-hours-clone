using UnityEngine;
using System.Collections;

public class GhostController : FSMSystem

{
    protected Transform playerTransform;
    float elapsedTime;
    protected GameObject PlayerGO;

    void Awake()
    {
        elapsedTime = 0.0f;
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = PlayerGO.transform;

        InitializeFSM();
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        elapsedTime += Time.deltaTime;
	}

    void FixedUpdate()
    {
        CurrentState.Reason(PlayerGO, this.gameObject);
        CurrentState.Act(PlayerGO, this.gameObject);
    }

    public void SetTransition(Transition t)
    {
        PerformTransition(t);
    }

    private void InitializeFSM()
    {
        GhostChaseState chase = new GhostChaseState();
        chase.AddTransition(Transition.PlayerLost, StateID.Idling);
        chase.AddTransition(Transition.PlayerReached, StateID.Attacking);
        chase.AddTransition(Transition.NoHealth, StateID.Dead);

        GhostIdleState idle = new GhostIdleState();
        idle.AddTransition(Transition.PlayerSpotted, StateID.Chasing);
        idle.AddTransition(Transition.NoHealth, StateID.Dead);

        GhostAttackingState attack = new GhostAttackingState();
        attack.AddTransition(Transition.PlayerOutOfRange, StateID.Chasing);
        attack.AddTransition(Transition.NoHealth, StateID.Dead);

        AddState(attack);
        AddState(chase);
        AddState(idle);
    }
}
