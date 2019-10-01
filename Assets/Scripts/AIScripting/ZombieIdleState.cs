using System;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : FSMState
{
    bool playerInSight = true;
    bool playerHeard;

    Transform playerTrans;
    Transform npcTrans;
    ObjectHealth npcHealth;
    AINavAgent npcNav;
        
    public ZombieIdleState() 
    {
        stateID = StateID.Idling;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        playerTrans = player.transform;
        npcTrans = npc.transform;
        npcHealth = npc.GetComponent<ObjectHealth>();
        destination = playerTrans.position;

        float playerDist = Vector3.Distance(npcTrans.position, destination);

        if (playerDist < 12)
        {
           playerInSight = npc.GetComponent<ZombieController>().sightDetector.Detect(player, npcTrans);
        }

        //if (playerDist < 15)
        //{
        //    playerHeard = npc.GetComponent<ZombieController>().soundDetector.Detect(player.GetComponent<PlayerController>(), playerDist);
        //}

        if (playerHeard && !playerInSight)
        {
            Debug.Log("Switch to Alert state");
            npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerHeard);
        }

        if (playerInSight)
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
        npcTrans = npc.transform;
        npcNav = npc.GetComponent<AINavAgent>();
        npcNav.Stop();
    }
}
