using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnMonster : EventBaseClass {

    public GameObject monsterPrefab;
    public int spawnAmount;
    private List<GameObject> spawnedMonsterList = new List<GameObject>(10);
    float monsterDiameter;

    public EventBaseClass spawnMonsterInstance;
    TriggerManager managerRef;

    public override void Awake ()
    {
        managerRef = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<TriggerManager>();
        spawnMonsterInstance = this.GetComponent<SpawnMonster>();
        objectPosition = this.transform.position;
        ConnectToTrigger();
    }

    public override void ConnectToTrigger()
    {
        identifier = managerRef.RegisterEvent(ref spawnMonsterInstance);
        Debug.Log("identifier = " + identifier);
    }

	// Use this for initialization
	void Start () 
    {
        monsterDiameter = (2 * monsterPrefab.GetComponent<CapsuleCollider>().radius) + 0.1f;

        wiredTrigger = managerRef.getTrigger(identifier);
        wiredTrigger.tripTrigger += new Trigger.activateTriggerDelegate(doAction);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnDisable ()
    {
        wiredTrigger.tripTrigger -= doAction;
    }

    public override void doAction ()
    {
        for (int i = spawnedMonsterList.Count; i < spawnAmount; i++)
        {
            GameObject monsterGO = Instantiate(monsterPrefab) as GameObject;
            spawnedMonsterList.Add(monsterGO);
            monsterGO.transform.position = objectPosition;
            monsterGO.transform.position += new Vector3(0, 0, i * monsterDiameter);
        }
    }
}
