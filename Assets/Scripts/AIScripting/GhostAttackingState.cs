using System;
using System.Collections.Generic;
using UnityEngine;

    public class GhostAttackingState : FSMState
    {
         public GhostAttackingState() 
        {
            stateID = StateID.Attacking;
        }

    public override void Reason(GameObject player, GameObject npc)
    {
        Transform playerTrans = player.transform;
        Transform npcTrans = npc.transform;
        destination = playerTrans.position;

        float playerDist = Vector3.Distance(npcTrans.position, destination);

        if (playerDist > 0.5f)
        {
            Debug.Log("Switch to Chase state");
            npc.GetComponent<GhostController>().SetTransition(Transition.PlayerOutOfRange);
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