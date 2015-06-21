using UnityEngine;
using System.Collections;

public class GhostController : FSMSystem

{
    protected Transform playerTransform;
    float elapsedTime;
    protected GameObject PlayerGO;
    public float attackDamage;
    public float attackRange;
    public float attackAngle = 45f;

    public int pointsAmt = 5;

	/* health */
	ObjectHealth enemyHealth;

	/* for animating */
	private Animator animEnemy;


    void Awake()
    {
		animEnemy = this.GetComponentInChildren <Animator> ();

		enemyHealth = this.GetComponentInChildren <ObjectHealth> ();

        elapsedTime = 0.0f;
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = PlayerGO.transform;

        InitializeFSM();
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        elapsedTime += Time.deltaTime;
	}

    void FixedUpdate()
    {
        CurrentState.Reason(PlayerGO, this.gameObject);
        CurrentState.Act(PlayerGO, this.gameObject);
    }

    public void SetTransition(Transition t)
    {
        PerformTransition(t);
    }

    public IEnumerator death()
    {
        if (enemyHealth.isDead) // Enemy is dead
        {
            animEnemy.SetFloat("Action", 1f);
            NotificationsManager.DefaultNotifier.PostNotification(this, "OnEnemyKilled", pointsAmt as object);
        }
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator attackBehavior()
    {
        Debug.Log("Attacking");
        RaycastHit attackHit = new RaycastHit();
        Vector3 attackDirection = playerTransform.position - transform.position;

        if (Vector3.Angle(attackDirection, transform.forward) < attackAngle)
        {
            if (Physics.Raycast(transform.position, attackDirection, out attackHit, attackRange))
            {
                ObjectHealth objectHealth = attackHit.collider.GetComponent<ObjectHealth>();
                if (objectHealth != null)
                {
                  objectHealth.TakeDamage(attackDamage, attackHit.point);
                }
            }
        }

        yield return new WaitForSeconds(2f);
    }

    private void InitializeFSM()
    {
        GhostChaseState chase = new GhostChaseState();
        chase.AddTransition(Transition.PlayerLost, StateID.Idling);
        chase.AddTransition(Transition.PlayerReached, StateID.Attacking);
        chase.AddTransition(Transition.NoHealth, StateID.Dead);

        GhostIdleState idle = new GhostIdleState();
        idle.AddTransition(Transition.PlayerSpotted, StateID.Chasing);
        idle.AddTransition(Transition.NoHealth, StateID.Dead);

        GhostAttackingState attack = new GhostAttackingState();
        attack.AddTransition(Transition.PlayerOutOfRange, StateID.Chasing);
        attack.AddTransition(Transition.NoHealth, StateID.Dead);

        GhostDeadState dead = new GhostDeadState();

        AddState(attack);
        AddState(chase);
        AddState(idle);
        AddState(dead);
    }
}
