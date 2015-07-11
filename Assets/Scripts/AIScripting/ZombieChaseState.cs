using UnityEngine;
using System.Collections;

public class ZombieChaseState : FSMState 
{
    bool playerDetected;
    Transform playerTrans;
    Transform npcTrans;
    ObjectHealth npcHealth;
    AINavAgent npcNav;
    int doOnce = 0;

    public ZombieChaseState() 
    {
        stateID = StateID.Chasing;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        playerTrans = player.transform;
        npcTrans = npc.transform;
        npcHealth = npc.GetComponent<ObjectHealth>();
        destination = playerTrans.position;

        float playerDist = Vector3.Distance(npcTrans.position, destination);
        playerDetected = npc.GetComponent<ZombieController>().sightDetector.Detect(player, npcTrans);

        if (playerDist <= 0.9f)
        {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerReached);
        }

        //if (playerDist >= 7.0f && !playerDetected)
        //{
        //    Debug.Log("Switch to Idle state");
        //    npc.GetComponent<ZombieController>().SetTransition(Transition.PlayerLost);
        //}

        if (npcHealth.isDead)
        {
            Debug.Log("Switch to Dead state");
            npc.GetComponent<ZombieController>().SetTransition(Transition.NoHealth);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        playerTrans = player.transform;
//        Transform npcTrans = npc.transform;
        destination = playerTrans.position;

        npcNav = npc.GetComponent<AINavAgent>();
        npcNav.enabled = true;

        npcNav.speed = 2f;

        if (doOnce == 0)
        {
            npcNav.SetDestination(destination);
            doOnce++;
        }

        npcNav.TrackMovingTarget(player.GetComponent<PlayerController>() as IAITrackable);
    }
	// Use this for initialization
}
