using System;
using System.Collections.Generic;
using UnityEngine;


public interface IEventListener

{
    Trigger wiredTrigger { get; set; }
    Vector3 objectPosition { get; set; }
    int identifier { get; set; }
    TriggerManager managerRef { get; set; }

    void ConnectToTrigger();

    void OnDisable();
   
    void doAction();
}