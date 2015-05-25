using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour {

    BoxCollider BoxCollider;
    Material doorMat;

    void Awake ()
    {
        BoxCollider = GetComponent<BoxCollider>();
        doorMat = GetComponent<MeshRenderer>().material;
    }
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter (Collision other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            BoxCollider.enabled = false;
            Color color = doorMat.color;
            color.a = 0f;
            doorMat.color = color;
        }
    }
  
}
