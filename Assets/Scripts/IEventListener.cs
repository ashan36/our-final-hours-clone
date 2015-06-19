using System;
using System.Collections.Generic;
using UnityEngine;

//Implement this interface on any class that needs to register with the trigger manager and receive a trigger to hook up with
public interface IEventListener

{
    Trigger wiredTrigger { get; set; }
    Vector3 objectPosition { get; set; }
    int identifier { get; set; }
    TriggerManager managerRef { get; set; }

    // Register with the triggermanager
    void ConnectToTrigger();

    // For unsubscribing when the listener is disabled or destroyed
    void OnDisable();
   
    // The action to do when the Event is called
    void doAction();
}