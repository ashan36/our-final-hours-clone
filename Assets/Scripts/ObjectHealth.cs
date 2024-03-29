﻿using UnityEngine;
using System.Collections;

public class ObjectHealth : MonoBehaviour {

    public float startingHealth = 100;
    public float currentHealth;
    public bool killable;

    public ParticleSystem hitParticles;
    Collider colliderRef;

    public bool isDead;
    public bool hurt = false;

    void Awake ()
    {
        hitParticles = GetComponentInChildren<ParticleSystem>();

        if (this.GetComponent<SphereCollider>() != null)
        colliderRef = GetComponent<SphereCollider>();
        if (this.GetComponent<CapsuleCollider>() != null)
        colliderRef = GetComponent<CapsuleCollider>();
        if (this.GetComponent<BoxCollider>() != null)
        colliderRef = GetComponent<BoxCollider>();

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
            isDead = true;
            colliderRef.enabled = false;
            Destroy(this.gameObject, 3f);
        }
	}

    public void TakeDamage(float dmgAmount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        if (hitParticles != null)
        {
            hitParticles.transform.position = hitPoint;
            hitParticles.Play();
        }

        if (!hurt && this.tag == "Player")
        {
            currentHealth -= dmgAmount;
            hurt = true;
            NotificationsManager.DefaultNotifier.PostNotification(this, "OnPlayerHurt");
        }

        if (!hurt && this.tag == "EnemyNPC")
        {
            currentHealth -= dmgAmount;
            hurt = true;
            SendMessage("OnEnemyHurt");
        }
    }
}
