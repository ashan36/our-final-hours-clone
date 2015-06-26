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

            for (int i = 0; i < eventList.Count; i++)
            {
                int indexofMatch = 0;
                float shortestDistance = 400;
                float currentDistance = 0;

                for (int n = 0; n < triggerList.Count; n++)
                {
                    currentVector = triggerList[n].triggerPosition - eventList[i].objectPosition;
                    currentDistance = Math.Abs(currentVector.sqrMagnitude);
                    

                    if (n == 0 || (shortestDistance > currentDistance))
                    {
                        switch (eventList[i].properties)
                        {
                            case 1:
                                if (triggerList[n] is IInteractable)
                                {
                                    shortestDistance = currentDistance;
                                    indexofMatch = n;
                                    Debug.Log("identifier of match " + indexofMatch);
                                    break;
                                }
                                else
                                    break;

                            case 2:
                                if (triggerList[n] is IInteractable)
                                {
                                    Debug.Log("Case 2 is interactable");
                                    break;
                                }
                                else
                                {
                                    Debug.Log("Case 2 is not interactable");
                                    shortestDistance = currentDistance;
                                    indexofMatch = n;
                                    Debug.Log("identifier of match " + indexofMatch);
                                    break;
                                }

                            case 3:
                                shortestDistance = currentDistance;
                                indexofMatch = n;
                                break;

                            default:
                                shortestDistance = currentDistance;
                                indexofMatch = n;
                                break;
                        }
                    }
                }

                pairedList[i] = indexofMatch;

            }
        }

        public Trigger getTrigger (int index)
        {
            return triggerList[pairedList[index]];
        }

    }
