using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {
    public GameObject prefabCasing;
    Vector3 ejectionLoc;
    public float ejectionMult;

    public GameObject casing;
    Rigidbody casingRB;

    ParticleSystem smoke;
    public GameObject pelletGO;
    ParticleSystem pellets;

    public float lockTime = 0.5f;
    float fireRate = 0.5f;
    float lastShot;
    double approxDeltaShotTime;
    public bool isSemiAuto = false;

    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;              
    LineRenderer gunLine;
    public int damageAmt;


    void Awake()
    {
        Transform ejectionPointTrans = transform.FindChild("CasingSpawner");
        ejectionLoc = ejectionPointTrans.position;
        smoke = GetComponentInChildren<ParticleSystem>();
        if (pelletGO != null)
        pellets = pelletGO.GetComponent<ParticleSystem>();

        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
    }

	// Use this for initialization
	void Start () 
    {
        lastShot = -lockTime;
        if (!isSemiAuto)
        {
            fireRate = lockTime;
        }
        else
            fireRate = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("Fire", 0f, fireRate);
        }
        if(Input.GetMouseButtonUp(0))
        {
            CancelInvoke();
            PlayerController.attacking = false;
        }

        if (Mathf.Abs(lastShot - Time.time) > lockTime)
            gunLine.enabled = false;

        Transform ejectionPointTrans = transform.FindChild("CasingSpawner");
        ejectionLoc = ejectionPointTrans.position;
	}

    void Fire()
    {
        PlayerController.attacking = true;

        approxDeltaShotTime = System.Math.Round((Mathf.Abs(lastShot - Time.time)), 2);
        if ((approxDeltaShotTime - lockTime) < 0.1f)
            return;

        //Instantiates the prefab at the CasingSpawner loc, and sets it as a child of the handgun
        casing = Instantiate(prefabCasing) as GameObject;
        casing.transform.SetParent(this.transform, true);
        casing.transform.position = ejectionLoc;

        //Sends a reference of the just spawned casing to the GlobalBehavior Script for tracking
        GlobalBehavior.CasingSpawned(casing);

        //setup the rigidbody details for the casing
        casingRB = casing.GetComponent<Rigidbody>();
        Vector3 casingCOM = Vector3.zero;
        casingCOM.y = 0.008f;
        casingRB.centerOfMass = casingCOM;

        //Sets the ejection angle, then adds a bit of randomness to it.
        Vector3 ejectionVelocity;
        ejectionVelocity = (-transform.forward + (0.8f * transform.right)) * 1.25f;
        ejectionVelocity.y = 1f;

        Vector3 randVelocity = Random.insideUnitSphere;
        ejectionVelocity += (randVelocity * 0.4f);

        //Makes the casing spin a bit
        Vector3 randAngularVelocity = Random.insideUnitSphere;
        randAngularVelocity.x = -500;

        //Sets the ejection velocity to a random Vec3 times the multiplier
        casingRB.AddForce(ejectionVelocity * ejectionMult);
        casingRB.AddRelativeTorque(randAngularVelocity * 10, ForceMode.VelocityChange);

        //plays the GunSmoke particlesystem
        smoke.Play();

        if (pelletGO != null)
        pellets.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, ejectionLoc);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = ejectionLoc;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, 50f, shootableMask))
        {
            ObjectHealth objectHealth = shootHit.collider.GetComponent<ObjectHealth>();
            if (objectHealth != null)
            {
                objectHealth.TakeDamage(damageAmt, shootHit.point);
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * 50f);

        casing.transform.SetParent(null, true);

        //Sets the time of the last time this method was called
        lastShot = Time.time;
    }
}
