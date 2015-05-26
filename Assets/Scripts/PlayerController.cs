﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/* for movement */
	float speed = 6f;
	public float moveSpeed = 4f;
	public float moveAttackSpeed = 2f; // player's movement speed while attacking, shooting, etc
	public float moveFallSpeed = 2f;
	public float direction = 1f; // for turning left and right
	Vector3 movement;
	GameObject player;
	Transform playerTrans;

	//---test stuff---
	Transform chestTrans;
	public float chestAngle;
	//---

	public Quaternion localRotation;

	/* for jumping */
	GameObject ground;
	public float jumpHeight= 10f;
	public bool jumping = false;
	public bool falling = false;
	public bool isGrounded = true;

	/* for animating */
	public GameObject legs;
	public Animator animLegs;
	public GameObject chest;
	public Animator animChest;
	public bool walking;
    public static bool isInside = false;

	public int weapon = 0;
	
	void Awake () 
	{
		legs = GameObject.Find ("Legs");
		animLegs = legs.GetComponent <Animator> ();

		chest = GameObject.Find ("Chest");
		animChest = chest.GetComponent <Animator> ();
		chestTrans = chest.transform;

		player = GameObject.FindGameObjectWithTag ("Player");
		playerTrans = player.transform;
		speed = moveSpeed;
	}

    void Start ()
    {
        FollowCam.S.poi = player;
    }
	
	// FixedUpdate is called 50 times per second
	void FixedUpdate ()
	{
		
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");;

		if (Input.GetKeyDown (KeyCode.Space) && isGrounded)
		{
			GetComponent<Rigidbody> ().velocity = new Vector3 (0f, jumpHeight);
			jumping = true;
			isGrounded = false;
			speed = moveFallSpeed;
		}
		else
		{
			jumping = false;
		}

		if (isGrounded)
		{
			speed = moveSpeed;
			falling = false;
		}
		else if (!isGrounded && !jumping)
		{
			falling =  true;
			speed = moveFallSpeed;
		}
		//attacking = Input.GetButtonDown ("Attack");
		//blocking = Input.GetButton ("Block");
		Move (h, v);
		Animating (h, v);
	}

    void Update ()
    {
        FollowCam.S.poi = player;
    }

	void Move (float h, float v)
	{
		movement.Set (h, 0f, (v));
		
		movement = movement.normalized * speed * Time.deltaTime;
		
		playerTrans.position += movement;

		if(h < 0f)
		{
			direction = -180f;
			chestAngle = 35f;
			Flip ();
		}
		else if (h > 0f)
		{
			direction = 0f;
			chestAngle = -35f;
			Flip ();
		}
		
	}

	void Flip ()
	{
		playerTrans.localEulerAngles = new Vector3 (0,direction,0);
	}

	void OnCollisionEnter(Collision coll) 
	{
		if (coll.gameObject.tag == "Ground")
		{
			isGrounded = true;
			falling = false;
            isInside = false;
            print("is Outside");
		}

        if (coll.gameObject.tag == "InsideFloor")
        {
			isGrounded = true;
			falling = false;
			isInside = true;
            print("is Inside");
        }
		//else if
		//{
		//	isGrounded = false;
		//}
		
	}
	void Animating (float h, float v)
	{
		walking = (h != 0f || v != 0f);
		if (walking)
		{
			animLegs.SetBool ("IsWalking", true);
			animChest.SetBool ("IsWalking", true);
		}
		else if(!walking)
		{
			animLegs.SetBool ("IsWalking", false);
			animChest.SetBool ("IsWalking", false);
		}
	}
}