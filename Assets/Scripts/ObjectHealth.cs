using UnityEngine;
using System.Collections;

public class ObjectHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public bool killable;

    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    SphereCollider sphereCollider;
    BoxCollider boxCollider;
    bool isDead;

    void Awake ()
    {
        hitParticles = GetComponentInChildren<ParticleSystem>();

        if (this.GetComponent<SphereCollider>() != null)
        sphereCollider = GetComponent<SphereCollider>();
        if (this.GetComponent<CapsuleCollider>() != null)
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (this.GetComponent<BoxCollider>() != null)
        boxCollider = GetComponent<BoxCollider>();

        currentHealth = startingHealth;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (currentHealth <= 0 && killable)
        {
            Destroy(this.gameObject, 0.1f);
            isDead = true;
        }
	}

    public void TakeDamage(int dmgAmount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        if (hitParticles != null)
        {
            hitParticles.transform.position = hitPoint;
            hitParticles.Play();
        }

        currentHealth -= dmgAmount;
    }
}
