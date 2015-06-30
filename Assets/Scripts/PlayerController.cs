using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/* for movement */
    public float speed;
	public float walkSpeed = 2f;
    public float dodgeSpeed = 5f;
    public float jumpBackSpeed = 4.5f;
    public float runSpeed = 4f;
	public float aimSpeed = 1.1f; // player's movement speed while attacking, shooting, etc

	Vector3 movement;
	GameObject player;
	public static Transform playerTrans;

	//---test stuff---
	public float action;
	//---

	public Quaternion localRotation;
    float rotateSpeed = 7.0f;

	/* for jumping */
//	public float jumpHeight= 10f;
//	public static bool jumping = false;
//	public static bool falling = false;
	public static bool isGrounded = true;

	/* for animating */
	public GameObject pAnim;
	public Animator animPlayer;
  
	public bool walking;
    public static bool isInside = false;
    public bool attacking = false;
    public bool facingRight;
    public bool running;
    public bool dodging;
    public bool aiming;
    public bool idle;
    public bool interacting;
    public bool knockback;
    public bool hurt;
    public bool animComplete = true;

	/* health */
	ObjectHealth playerHealth;

    //Weapon references
	public int weapon = 0;
    Shooting shootingRef;
	
	void Awake () 
	{
		pAnim = GameObject.Find ("PlayerAnimation");
		animPlayer = pAnim.GetComponent <Animator> ();

		player = GameObject.FindGameObjectWithTag ("Player");
		playerTrans = player.transform;

		playerHealth = this.GetComponentInChildren<ObjectHealth>();

        shootingRef = this.GetComponentInChildren<Shooting>();
	}

    void Start ()
    {
        FollowCam.S.poi = player;
        NotificationsManager.DefaultNotifier.AddObserver(this, "OnPlayerHurt");
    }
	
	// FixedUpdate is called 50 times per second
	void FixedUpdate ()
	{

 /*         if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0f, jumpHeight);
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
                falling = true;
                speed = moveFallSpeed;
            }
  */
	}

    void Update ()
    {
        if (playerHealth.isDead)
        {
            GameManager.gamePaused = true;
        }

        if (!GameManager.gamePaused)
        {
            UpdateMouse();
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            StartCoroutine(decideState(h, v));

            Move(h, v);
        }

        FollowCam.S.poi = player;
	}

    public IEnumerator decideState(float h, float v)
    {
        //Aiming
        if (Input.GetKey(KeyCode.Mouse1) && isGrounded)
        {
            aiming = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && isGrounded)
        {
            aiming = false;
        }

        //Shooting
        if (Input.GetMouseButton(0) && aiming)
        {
            attacking = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            attacking = false;
            shootingRef.SendMessage("PullTrigger", attacking);
        }

        //Running
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !aiming)
        {
            running = true;
            walking = false;
            rotateSpeed = 2.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
        }

        //Dodging
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && animComplete)
        {
            dodging = true;
            rotateSpeed = 1.0f;
            animComplete = false;
            yield return StartCoroutine(Animating(h, v));
            animComplete = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && animComplete)
        {
            dodging = false;
        }
        else if (animComplete)
        {
            dodging = false;
        }

        // Walking
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !(h == 0 && v == 0) && isGrounded && !running && !dodging)
        {
            walking = true;
            rotateSpeed = 7.0f;
        }
        else
        {
            walking = false;
        }

        //Knock back attack (kicking)
        if (Input.GetKeyDown(KeyCode.F) && isGrounded && animComplete)
        {
            knockback = true;
            animComplete = false;
            yield return StartCoroutine(Animating(h, v));
        }
        else if (Input.GetKeyUp(KeyCode.F) && animComplete)
        {
            knockback = false;
            yield return StartCoroutine(Animating(h, v));
        }

        //Interacting
        if (Input.GetKeyDown(KeyCode.E) && isGrounded && animComplete)
        {
            interacting = true;
            animComplete = false;
            yield return StartCoroutine(Animating(h, v));
            if (animComplete)
                NotificationsManager.DefaultNotifier.PostNotification(this, "OnPlayerInteract");
        }
        else if (Input.GetKeyUp(KeyCode.E) && animComplete)
        {
            interacting = false;
            yield return StartCoroutine(Animating(h, v));
        }

        if (!Input.anyKeyDown)
        {
            idle = true;
        }
        else
        {
            idle = false;
        }

        yield return StartCoroutine(Animating(h, v));
    }

	void Move (float h, float v)
	{
		movement.Set (h, 0f, (v));

        if (walking)
        {
            movement = movement.normalized * speed * Time.deltaTime;
            playerTrans.position += movement;
        }

        if (running)
        {
            movement = playerTrans.forward * speed * Time.deltaTime;
            playerTrans.position += movement;
        }

        if (dodging)
        {
            if (facingRight && (h > 0 || running))
            {
                movement = playerTrans.forward * speed * Time.deltaTime;
                playerTrans.position += movement;
            }

            if (facingRight && h <= 0)
            {
                movement = playerTrans.forward * -speed * Time.deltaTime;
                playerTrans.position += movement;
            }

            if (!facingRight && h >= 0)
            {
                movement = playerTrans.forward * -speed * Time.deltaTime;
                playerTrans.position += movement;
            }

            if (!facingRight && (h < 0 || running))
            {
                movement = playerTrans.forward * speed * Time.deltaTime;
                playerTrans.position += movement;
            }


        }
	}

    void OnPlayerHurt()
    {
        return;
    }

	void OnCollisionEnter(Collision coll) 
	{
		if (coll.gameObject.tag == "Ground")
		{
			isGrounded = true;
            isInside = false;
            //print("is Outside");
		}

        if (coll.gameObject.tag == "InsideFloor")
        {
			isGrounded = true;
			isInside = true;
            //print("is Inside");
        }
		
	}
	public IEnumerator Animating (float h, float v)
	{
		/* Action number code
		 * - - - - - - - - - -
		 * 0 = idle
		 * 1 = idle aiming
		 * 2 = idle shooting
		 * 3 = idle interaction
		 * 4 = knockback
		 * 5 = running
		 * 6 = walk forward
		 * 7 = walk aiming forward
		 * 8 = walk shooting forwad
		 * 9 = walking backward
		 * 10 = walk aiming backwards
		 * 11 = walk shooting backwards
		 * 12 = dodge forward
		 * 13 = dodge backwards
		 * 14 = hurt
         * 15 = death
		 */

        if (playerHealth.isDead) 									//***dead***//
        {
	        animPlayer.SetFloat ("Action", 15f);
            yield return new WaitForSeconds(2f);
            animComplete = true;
        }
        else // Player is not dead
        {
            if (playerHealth.hurt)
            {
                animPlayer.SetFloat ("Action", 14f);
                speed = 0.1f;
                yield return new WaitForSeconds(0.35f);
                playerHealth.hurt = false;
                animComplete = true;
            }
            else
            {
		        if (dodging && facingRight) 			/***dodging & right***/
		        {
                    if (running || h > 0) //Moving righ, dodge roll forward
                    {
                        animPlayer.SetFloat("Action", 12f);
                        speed = dodgeSpeed;
                        yield return new WaitForSeconds(0.35f);
                    }
			        else if (h <= 0) //Moving left, dodge jumpback
			        {
				        animPlayer.SetFloat ("Action", 13f);
                        speed = jumpBackSpeed;
                        yield return new WaitForSeconds(0.2f);
			        }
                    dodging = false;
                    rotateSpeed = 7.0f;
		        }
		        else if (dodging && !facingRight) 			/***dodging & left***/
		        {
                    if (running || h < 0) //Moving left, dodge roll forward
                    {
                        animPlayer.SetFloat("Action", 12f);
                        speed = dodgeSpeed;
                        yield return new WaitForSeconds(0.35f);
                    }
                    else if (h >= 0) //Moving left, dodge jumpback
                    {
                        animPlayer.SetFloat("Action", 13f);
                        speed = jumpBackSpeed;
                        yield return new WaitForSeconds(0.2f);
                    }
                    dodging = false;
                    rotateSpeed = 7.0f;
		        }
		        else
                {
                    if (knockback) 									/***knockback***/
			        {
			            animPlayer.SetFloat ("Action", 4f);
                        yield return new WaitForSeconds(0.4f);
                        knockback = false;
                        animComplete = true;
			        }
			        else
			        {
				        if (aiming && h == 0 && v == 0) 					/***aiming & not moving***/
				        {
					        animPlayer.SetFloat ("Action", 1f);
                            speed = aimSpeed;
                            if (attacking)
                            {
                                animPlayer.SetFloat("Action", 2f); //idle aiming & attacking
                                shootingRef.SendMessage("PullTrigger", attacking);
                                yield return new WaitForEndOfFrame();
                            }
                            else { yield return new WaitForEndOfFrame(); }
				        }
				        if (aiming && facingRight) 		/***aiming & moving right***/
				        {
					        if(h < 0)
					        {
						        animPlayer.SetFloat ("Action", 10f);
                                speed = aimSpeed; ;
                                if (attacking)
                                {
                                    animPlayer.SetFloat("Action", 11f); //aiming while moving left & attacking (backward)
                                    shootingRef.SendMessage("PullTrigger", attacking);
                                    yield return new WaitForEndOfFrame();
                                }
                                else { yield return new WaitForEndOfFrame(); }
					        }
					        if (h >= 0)
					        {
						        animPlayer.SetFloat ("Action", 7f);
                                speed = aimSpeed; ;
                                if (attacking)
                                {
                                    animPlayer.SetFloat("Action", 8f); //aiming while moving right & attacking (forward)
                                    shootingRef.SendMessage("PullTrigger", attacking);
                                    yield return new WaitForEndOfFrame();

                                }
                                else { yield return new WaitForEndOfFrame(); }

					        }
				        }
				        if (aiming && !facingRight) 		/***aiming & moving left***/
				        {
					        if(h <= 0)
					        {
						        animPlayer.SetFloat ("Action", 7f);
                                speed = aimSpeed; ;

                                if (attacking)
                                {
                                    animPlayer.SetFloat("Action", 8f); //aiming while moving left & attacking (forward)
                                    shootingRef.SendMessage("PullTrigger", attacking);
                                    yield return new WaitForEndOfFrame();
                                }
                                else
                                { yield return new WaitForEndOfFrame(); }
					        }
					        if (h > 0)
					        {
						        animPlayer.SetFloat ("Action", 10f);
                                speed = aimSpeed; 
                                if (attacking)
                                {
                                    animPlayer.SetFloat("Action", 11f); //aiming while moving right & attacking (backward)
                                    shootingRef.SendMessage("PullTrigger", attacking);
                                    yield return new WaitForEndOfFrame();
                                }
                                else 
                                { yield return new WaitForEndOfFrame(); }
					        }
				        }
				        else
				        {
                            if (running && facingRight)
                            {
                                animPlayer.SetFloat ("Action", 5f);
                                speed = runSpeed;
                                yield return new WaitForEndOfFrame();
                            }
                            else if (running && !facingRight)
                            {
                                animPlayer.SetFloat ("Action", 5f);
                                speed = runSpeed;
                                yield return new WaitForEndOfFrame();
                            }
                            else
                            {   
						        if (walking && facingRight && h >= 0)
						        {
							        animPlayer.SetFloat ("Action", 6f);
                                    speed = walkSpeed;
                                    yield return new WaitForEndOfFrame();
						        }
                                else if (walking && !facingRight && h <= 0)
                                {
                                    animPlayer.SetFloat ("Action", 6f);
                                    speed = walkSpeed;
                                    yield return new WaitForEndOfFrame();
                                }
						        else if (walking && !facingRight && h > 0)
						        {
							        animPlayer.SetFloat ("Action", 9f);
                                    speed = walkSpeed;
                                    yield return new WaitForEndOfFrame();
						        }
                                else if (walking && facingRight && h < 0 )
                                {
                                    animPlayer.SetFloat ("Action", 9f);
                                    speed = walkSpeed;
                                    yield return new WaitForEndOfFrame();
                                }
                                else 
                                {
                                    if (interacting)
                                    {
                                        animPlayer.SetFloat ("Action", 3f);
                                        yield return new WaitForSeconds(0.35f);
                                        animComplete = true;
                                        interacting = false;
                                    }

                                    else
                                    {
                                        if (idle)
                                        {
                                            animPlayer.SetFloat ("Action", 0f);
                                            yield return new WaitForEndOfFrame();
                                        }
                                    }
                                }
                            }
				        }
			        }
		        }
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
            {
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                facingRight = true;
            }

            else if (targetRotation.eulerAngles.y <= 305 && targetRotation.eulerAngles.y >= 235)
            {
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                facingRight = false;
            }


            else if (targetRotation.eulerAngles.y > 0 && targetRotation.eulerAngles.y < 55.0f)
            {
                targetRotation = Quaternion.AngleAxis(55.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                facingRight = true;
            }
            else if (targetRotation.eulerAngles.y < 360 && targetRotation.eulerAngles.y > 305.0f)
            {
                targetRotation = Quaternion.AngleAxis(305.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                facingRight = false;
            }
            else if (targetRotation.eulerAngles.y > 180 && targetRotation.eulerAngles.y < 235.0f)
            {
                targetRotation = Quaternion.AngleAxis(235.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                facingRight = false;
            }
            else if (targetRotation.eulerAngles.y <= 180 && targetRotation.eulerAngles.y > 125.0f)
            {
                targetRotation = Quaternion.AngleAxis(125.0f, Vector3.up);
                playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                facingRight = true;
            }
        }
    }
}
