using UnityEngine;
using System.Collections;

public class ZombieChaseState : FSMState 
{

    public ZombieChaseState() 
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
            npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerReached);
        }

        if (playerDist >= 8.0f)
        {
            npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerLost);
        }

        if (npcHealth.isDead)
        {
            Debug.Log("Switch to Dead state");
            npc.GetComponent<ZombieController>().SetTransition(Transition.NoHealth);
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
}
