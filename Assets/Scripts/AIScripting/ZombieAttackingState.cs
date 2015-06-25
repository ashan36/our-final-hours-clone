using System;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackingState : FSMState
{
    public ZombieAttackingState() 
    {
            stateID = StateID.Attacking;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        Transform playerTrans = player.transform;
        Transform npcTrans = npc.transform;
        ObjectHealth npcHealth = npc.GetComponent<ObjectHealth>();
        destination = playerTrans.position;

        float playerDist = Vector3.Distance(npcTrans.position, destination);

        if (playerDist > 0.5f)
        {
            npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerOutOfRange);
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
        npc.GetComponent<MonoBehaviour>().StartCoroutine("attackBehavior");
        npcNav.enabled = false;
        Debug.Log("Attack Complete");
    }
}