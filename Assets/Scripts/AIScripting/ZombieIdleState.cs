using System;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : FSMState
{
        public ZombieIdleState() 
        {
            stateID = StateID.Idling;
        }

    public override void Reason(GameObject player, GameObject npc)
    {
        Transform playerTrans = player.transform;
        Transform npcTrans = npc.transform;
        ObjectHealth npcHealth = npc.GetComponent<ObjectHealth>();
        destination = playerTrans.position;

        float playerDist = Vector3.Distance(npcTrans.position, destination);

        if (playerDist < 8.0f)
        {
            Debug.Log("Switch to Chase state");
            npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerSpotted);
        }

        if (npcHealth.isDead)
        {
            Debug.Log("Switch to Dead state");
            npc.GetComponent<ZombieController>().SetTransition(Transition.NoHealth);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        NavMeshAgent npcNav = npc.GetComponent<NavMeshAgent>();
        npcNav.enabled = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
