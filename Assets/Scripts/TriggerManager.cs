using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TriggerManager : MonoBehaviour
    {
        private List<Trigger> triggerList = new List<Trigger>(5);
        private List<IEventListener> eventList = new List<IEventListener>(5);

        private static int[] pairedList;

        void Awake ()
        {
            pairedList = new int[eventList.Count];
            pairEvents();
        }

        void Start ()
        {
            Debug.Log("Trigger and Event Lists \n ______________");
            for (int i = 0; i < triggerList.Count; i++)
            {
                Debug.Log("Trigger " + triggerList[i].ToString() + " " + i);
            }

            for (int i = 0; i < eventList.Count; i++)
            {
                Debug.Log("Event " + eventList[i].ToString() + " " + i);
            }
        }

        public int RegisterTrigger(ref Trigger triggerObject)
        {
            int identifier;
            triggerList.Add(triggerObject);
            identifier = triggerList.Count - 1;
            return identifier;
        }

        public int RegisterEvent (ref IEventListener eventObject)
        {
            int identifier;
            eventList.Add(eventObject);
            identifier = eventList.Count - 1;
            return identifier;
        }

        public void pairEvents()
        {
            Vector3 currentVector;
            float currentDistance;
            float shortestDistance = 200;

            for (int i = 0; i < eventList.Count; i++)
            {
                int indexofMatch = 0;
                for (int n = 0; n < triggerList.Count; n++)
                {
                    currentVector = triggerList[n].triggerPosition - eventList[i].objectPosition;
                    currentDistance = Math.Abs(currentVector.sqrMagnitude);
                    print("currentdistance " + currentDistance);

                    if (n == 0 || shortestDistance > currentDistance)
                    {
                        shortestDistance = currentDistance;
                        indexofMatch = n;
                        print("shortestdistance " + shortestDistance + " index of " + indexofMatch);
                    }
                }

                pairedList[i] = indexofMatch;

                Debug.Log("Event:" + i + " Trigger:" + pairedList[i] + "\n");
            }
        }

        public Trigger getTrigger (int index)
        {
            for (int i = 0; i < pairedList.Length; i++)
            {
                Debug.Log("Event:" + i + " Trigger:" + pairedList[i] + "\n");
            }


            return triggerList[pairedList[index]];
        }

    }
