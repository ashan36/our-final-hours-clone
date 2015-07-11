using System;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAlertState : FSMState
{
    bool playerInSight;
    bool playerHeard;

    Transform playerTrans;
    Transform npcTrans;
    ObjectHealth npcHealth;
    AINavAgent npcNav;

    public ZombieAlertState() 
    {
        stateID = StateID.Alert;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        playerTrans = player.transform;
        npcTrans = npc.transform;
        npcHealth = npc.GetComponent<ObjectHealth>();

        float playerDist = Vector3.Distance(npcTrans.position, playerTrans.position);

        if (playerDist < 12f)
        {
            playerInSight = npc.GetComponent<ZombieController>().sightDetector.Detect(player, npcTrans);
        }

        if (playerDist < 20f)
        {
            playerHeard = npc.GetComponent<ZombieController>().soundDetector.Detect(player.GetComponent<PlayerController>(), playerDist);
        }

        if (playerDist > 10f && !playerInSight && !playerHeard)
        {
            Debug.Log("Switch to Idle state");
            npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerLost);
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
        playerTrans = player.transform;
        npcTrans = npc.transform;
        npcNav = npc.GetComponent<AINavAgent>();
        npcNav.enabled = true;
        float playerDist = Vector3.Distance(npcTrans.position, playerTrans.position);

        if (npc.GetComponent<ZombieController>().soundDetector.Detect(player.GetComponent<PlayerController>(), playerDist))
        {
            Debug.Log("Heard by " + npc.ToString());
            destination = playerTrans.position;
            npcNav.SetDestination(destination);
        }

        npcNav.Resume();

        npcNav.speed = 1f;
    }
}
