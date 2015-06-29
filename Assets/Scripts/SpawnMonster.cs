using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnMonster : MonoBehaviour, IEventListener
{

    public GameObject monsterPrefab;
    public int spawnAmount;
    private List<GameObject> spawnedMonsterList = new List<GameObject>(10);
    float monsterDiameter;

    // From IEventListener
    public Trigger wiredTrigger { get; set; }
    public Vector3 objectPosition { get; set; }
    public int identifier { get; set; }
    public int properties { get { return 2; } }

    public IEventListener listeningObjectRef;

    public void Awake ()
    {
        listeningObjectRef = this.GetComponent<SpawnMonster>();
        objectPosition = this.transform.position;
        ConnectToTrigger();
    }

    public void ConnectToTrigger()
    {
        identifier = TriggerManager.Instance.RegisterEvent(ref listeningObjectRef);
      //  Debug.Log("Spawn identifier = " + identifier);
    }

	// Use this for initialization
	void Start () 
    {
        monsterDiameter = (2 * monsterPrefab.GetComponent<CapsuleCollider>().radius) + 0.1f;

        wiredTrigger = TriggerManager.Instance.getTrigger(identifier);
        wiredTrigger.tripTrigger += new Trigger.activateTriggerDelegate(doAction);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDisable ()
    {
        wiredTrigger.tripTrigger -= doAction;
    }

    public void doAction ()
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
