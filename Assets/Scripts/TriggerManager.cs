using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TriggerManager : MonoBehaviour
    {
        List<Trigger> triggerList;
        List<IEventListener> eventList;
        List<int> pairedList;
        bool paired;

        private static TriggerManager instance = null;
        public static TriggerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<TriggerManager>();
                return instance;
            }
        }

        void Awake ()
        {
            if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
                DestroyImmediate(gameObject); //Delete duplicate
            else
            {
                instance = this; //Make this object the only instance
                DontDestroyOnLoad(gameObject); //Set as do not destroy
            }

            paired = false;
            pairedList = new List<int>();
        }

        void Start()
        {
            NotificationsManager.DefaultNotifier.AddObserver(this, "OnLevelRestart");
        }
        
        void OnLevelRestart()
        {
            pairedList.Clear();
            triggerList.Clear();
            eventList.Clear();
            pairedList = new List<int>();
            paired = false;
        }

        public int RegisterTrigger(ref Trigger triggerObject)
        {
            if (triggerList == null)
            {
                triggerList = new List<Trigger>(5);
                Debug.Log("triggerList was null");
            }
            int identifier;
            triggerList.Add(triggerObject);
            identifier = triggerList.Count - 1;
            Debug.Log(triggerObject.GetType() + " assigned identifier " + identifier);
            return identifier;
        }

        public int RegisterEvent (ref IEventListener eventObject)
        {
            if (eventList == null)
            {
                eventList = new List<IEventListener>(5);
                Debug.Log("eventList was null");
            }

            int identifier;
            eventList.Add(eventObject);
            identifier = eventList.Count - 1;
            Debug.Log(eventObject.GetType() + " assigned identifier " + identifier);
            return identifier;
        }

        public void pairEvents()
        {
            if (eventList == null)
                Debug.Log("Eventlist is null in pairEvents()");
            if (triggerList == null)
                Debug.Log("Trigger is null in pairEvents()");
                    
            Debug.Log("!!Pairing events, eventlistcount: " + eventList.Count + "\ntriggerlistcount: " + triggerList.Count);
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
                                   // Debug.Log("identifier of match " + indexofMatch);
                                    break;
                                }
                                else
                                    break;

                            case 2:
                                if (triggerList[n] is IInteractable)
                                {
                                   // Debug.Log("Case 2 is interactable");
                                    break;
                                }
                                else
                                {
                                   // Debug.Log("Case 2 is not interactable");
                                    shortestDistance = currentDistance;
                                    indexofMatch = n;
                                  //  Debug.Log("identifier of match " + indexofMatch);
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

                pairedList.Add(indexofMatch);
                Debug.Log("List size of = " + pairedList.Count);

            }
            paired = true;
        }

        public Trigger getTrigger (int index)
        {
            if (!paired)
                pairEvents();

            Debug.Log("Index called " + index);
            Debug.Log("Array size = " + pairedList.Count);
            return triggerList[pairedList[index]];
        }

    }
