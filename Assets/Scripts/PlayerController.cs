using UnityEngine;
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
	public static Transform playerTrans;

	//---test stuff---
	public float action;
	//---

	public Quaternion localRotation;
    float rotateSpeed = 7.0f;

    public static bool attacking = false;

	/* for jumping */
	public float jumpHeight= 10f;
	public static bool jumping = false;
	public static bool falling = false;
	public static bool isGrounded = true;

	/* for animating */
	public GameObject pAnim;
	public Animator animPlayer;
	public static bool walking;
    public static bool isInside = false;

	/* health */
	ObjectHealth playerHealth;

	public int weapon = 0;
	
	void Awake () 
	{
		pAnim = GameObject.Find ("PlayerAnimation");
		animPlayer = pAnim.GetComponent <Animator> ();

		player = GameObject.FindGameObjectWithTag ("Player");
		playerTrans = player.transform;
		speed = moveSpeed;

		playerHealth = this.GetComponentInChildren <ObjectHealth> ();
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

		Move (h, v);
		Animating (h, v);
	}

    void Update ()
    {
        FollowCam.S.poi = player;
        UpdateMouse();
	}

	void Move (float h, float v)
	{
		movement.Set (h, 0f, (v));
		
		movement = movement.normalized * speed * Time.deltaTime;

		playerTrans.position += movement;
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
            //print("is Outside");
		}

        if (coll.gameObject.tag == "InsideFloor")
        {
			isGrounded = true;
			falling = false;
			isInside = true;
            //print("is Inside");
        }
		
	}
	void Animating (float h, float v)
	{
		/* Action number code
		 * - - - - - - - - - -
		 * 0 = idle
		 * 1 = idle aiming
		 * 2 = idle shooting
		 * 3 = idle interaction
		 * 4 = [reserved]
		 * 5 = running
		 * 6 = walk forward
		 * 7 = walk aiming forward
		 * 8 = walk shooting forwad
		 * 9 = walking backward
		 * 10 = walk aiming backwards
		 * 11 = walk shooting backwards
		 * 12 = dodge forward
		 * 13 = dodge backwards
		 * 20 = death
		 */

		if (playerHealth.isDead) // Player is dead
		{
			animPlayer.SetFloat ("Action", 10f);
		}
		else // Player is not dead
		{
			walking = (h != 0f || v != 0f);
			if (walking)
			{
				animPlayer.SetFloat ("Action", 5f);
			}
			else if(!walking)
			{
				animPlayer.SetFloat ("Action", 0f);
			}
       		if (attacking)
       		{
				animPlayer.SetFloat ("Action", 7f);
        	}
       		else if (!attacking && !walking)
        	{
				animPlayer.SetFloat ("Action", 0f);
        	}
			else if (!attacking && walking)
			{
				animPlayer.SetFloat ("Action", 5f);
			}
		}
	}

    void UpdateMouse ()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position + new Vector3(0, 0, 0));
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        float HitDist = 0;

        if (playerPlane.Raycast(raycast, out HitDist))
        {
            Vector3 RayHitPoint = raycast.GetPoint(HitDist);

            Quaternion targetRotation = Quaternion.LookRotation(RayHitPoint - transform.position);

            if (targetRotation.eulerAngles.y >= 55.0f && targetRotation.eulerAngles.y <= 125.0f)
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            else if (targetRotation.eulerAngles.y <= 305 && targetRotation.eulerAngles.y >= 235)
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            else if (targetRotation.eulerAngles.y > 0 && targetRotation.eulerAngles.y < 55.0f)
            {
                targetRotation = Quaternion.AngleAxis(55.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
            else if (targetRotation.eulerAngles.y < 360 && targetRotation.eulerAngles.y > 305.0f)
            {
                targetRotation = Quaternion.AngleAxis(305.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
            else if (targetRotation.eulerAngles.y > 180 && targetRotation.eulerAngles.y < 235.0f)
            {
                targetRotation = Quaternion.AngleAxis(235.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
            else if (targetRotation.eulerAngles.y <= 180 && targetRotation.eulerAngles.y > 125.0f)
            {
                targetRotation = Quaternion.AngleAxis(125.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }
    }
}
