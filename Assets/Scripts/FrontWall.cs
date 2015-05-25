using UnityEngine;
using System.Collections;

public class FrontWall : MonoBehaviour {
	
	public GameObject frontWall;


	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate () 
	{

	}

	void OnTriggerEnter (Collider coll)
	{
		BecomeTransparent();
	}

	void OnTriggerExit (Collider coll)
	{
		BecomeOpaque();
	}

	void BecomeTransparent () 
	{
		frontWall.GetComponent<SpriteRenderer> ().color = new Color(1f,1f,1f,0.1f);
	}

	void BecomeOpaque () 
	{
		frontWall.GetComponent<SpriteRenderer> ().color = new Color(1f,1f,1f,1f);
	}
}