using UnityEngine;
using System.Collections;

public class ZombieController : FSMSystem

{
    protected Transform playerTransform;
    float elapsedTime;
    protected GameObject PlayerGO;
    public float attackDamage;
    public float attackRange;
    public float attackAngle = 45f;

    public float sightRange = 8f;
    public float sightAngle = 45f;
    public float hearingSensitivity = 20f;

    public int pointsAmt = 5;

    public SightDetection sightDetector;
    public SoundDetection soundDetector;

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

        sightDetector = new SightDetection(sightRange, sightAngle);
        soundDetector = new SoundDetection(hearingSensitivity);

        InitializeFSM();
    }

    void Start()
    {
        NotificationsManager.DefaultNotifier.AddObserver(this, "OnEnemyHurt");
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
        yield return new WaitForSeconds(1f);
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

        yield return new WaitForSeconds(1.5f);
    }

    void OnEnemyHurt()
    {   
        enemyHealth.hurt = false;
        Debug.Log("Zombie Hurt");

    }

    private void InitializeFSM()
    {
        ZombieChaseState chase = new ZombieChaseState();
        chase.AddTransition(Transition.PlayerLost, StateID.Idling);
        chase.AddTransition(Transition.PlayerReached, StateID.Attacking);
        chase.AddTransition(Transition.NoHealth, StateID.Dead);

        ZombieIdleState idle = new ZombieIdleState();
        idle.AddTransition(Transition.PlayerSpotted, StateID.Chasing);
        idle.AddTransition(Transition.PlayerHeard, StateID.Alert);
        idle.AddTransition(Transition.NoHealth, StateID.Dead);

        ZombieAttackingState attack = new ZombieAttackingState();
        attack.AddTransition(Transition.PlayerOutOfRange, StateID.Chasing);
        attack.AddTransition(Transition.NoHealth, StateID.Dead);

        ZombieAlertState alert = new ZombieAlertState();
        alert.AddTransition(Transition.PlayerLost, StateID.Idling);
        alert.AddTransition(Transition.PlayerSpotted, StateID.Chasing);
        alert.AddTransition(Transition.NoHealth, StateID.Dead);

        ZombieDeadState dead = new ZombieDeadState();

        AddState(attack);
        AddState(chase);
        AddState(idle);
        AddState(alert);
        AddState(dead);
    }
}
