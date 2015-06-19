using UnityEngine;
using System.Collections;

public class FrontWall : MonoBehaviour {
	
	public GameObject frontWall;
    Color originalWallColor;
    Color transparentWallColor;

	// Use this for initialization
	void Start () 
    {
        originalWallColor = frontWall.GetComponent<SpriteRenderer>().color;
        transparentWallColor = originalWallColor;
        transparentWallColor.a = 0.1f;
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
		frontWall.GetComponent<SpriteRenderer> ().color = transparentWallColor;
	}

	void BecomeOpaque () 
	{
		frontWall.GetComponent<SpriteRenderer> ().color = originalWallColor;
	}
}