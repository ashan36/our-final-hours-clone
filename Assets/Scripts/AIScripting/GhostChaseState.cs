using UnityEngine;
using System.Collections;

public class GhostChaseState : FSMState 
{

    public GhostChaseState() 
    {
        stateID = StateID.Chasing;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        Transform playerTrans = player.transform;
        Transform npcTrans = npc.transform;
        destination = playerTrans.position;

        float playerDist = Vector3.Distance(npcTrans.position, destination);
        if (playerDist <= 0.5f)
        {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<GhostController>().SetTransition(Transition.PlayerReached);
        }

        if (playerDist >= 7.0f)
        {
            Debug.Log("Switch to Idle state");
            npc.GetComponent<GhostController>().SetTransition(Transition.PlayerLost);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        Transform playerTrans = player.transform;
        Transform npcTrans = npc.transform;
        destination = playerTrans.position;

        NavMeshAgent npcNav = npc.GetComponent<NavMeshAgent>();
        npcNav.enabled = true;

        npcNav.SetDestination(playerTrans.position);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
