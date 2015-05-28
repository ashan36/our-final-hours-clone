using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnMonster : MonoBehaviour {

    public GameObject monsterPrefab;
    public int spawnAmount;
    private List<GameObject> spawnedMonsterList = new List<GameObject>(10);
    float monsterDiameter;

	// Use this for initialization
	void Start () 
    {
        TriggerEventManager.tripTrigger += spawnMonsters;
        monsterDiameter = (2 * monsterPrefab.GetComponent<CapsuleCollider>().radius) + 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDisable ()
    {
        TriggerEventManager.tripTrigger -= spawnMonsters;
    }

    void spawnMonsters ()
    {
        for (int i = spawnedMonsterList.Count; i < spawnAmount; i++)
        {
            GameObject monsterGO = Instantiate(monsterPrefab) as GameObject;
            spawnedMonsterList.Add(monsterGO);
            monsterGO.transform.position = this.transform.position;
            monsterGO.transform.position += new Vector3(0, 0, i * monsterDiameter);
        }
    }
}
