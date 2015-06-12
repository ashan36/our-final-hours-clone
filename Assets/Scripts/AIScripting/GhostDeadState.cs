using System;
using UnityEngine;
using System.Collections;

public class GhostDeadState : FSMState
    {
        public GhostDeadState() 
        {
            stateID = StateID.Dead;
        }

        bool actionFinished = false;

        public override void Reason(GameObject player, GameObject npc)
        {
            return;
        }

        public override void Act(GameObject player, GameObject npc)
        {
            if (!actionFinished)
            {
                ObjectHealth npcHealth = npc.GetComponent<ObjectHealth>();
                NavMeshAgent npcNav = npc.GetComponent<NavMeshAgent>();
                Rigidbody npcRB = npc.GetComponent<Rigidbody>();

                if (npcHealth.isDead)
                {
                    npcNav.enabled = false;
                    npc.GetComponent<GhostController>().StartCoroutine("death");
                    npcRB.isKinematic = true;
                    actionFinished = true;
                }
            }
        }
    }
