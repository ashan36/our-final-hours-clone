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
        ObjectHealth npcHealth = npc.GetComponent<ObjectHealth>();
        destination = playerTrans.position;

        float playerDist = Vector3.Distance(npcTrans.position, destination);
        if (playerDist <= 0.5f)
        {
            npc.GetComponent<GhostController>().SetTransition(Transition.PlayerReached);
        }

        if (playerDist >= 8.0f)
        {
            npc.GetComponent<GhostController>().SetTransition(Transition.PlayerLost);
        }

        if (npcHealth.isDead)
        {
            Debug.Log("Switch to Dead state");
            npc.GetComponent<GhostController>().SetTransition(Transition.NoHealth);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        Transform playerTrans = player.transform;
//        Transform npcTrans = npc.transform;
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
