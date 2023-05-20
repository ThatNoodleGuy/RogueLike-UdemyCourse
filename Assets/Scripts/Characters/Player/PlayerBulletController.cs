using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public static event EventHandler OnBulletImpact;

    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private ParticleSystem bulletImpactEffect;
    [SerializeField] private int damageToGive = 50;

    private Rigidbody2D bulletRB;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        bulletRB.velocity = transform.right * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ParticleSystem impactEffect = Instantiate(bulletImpactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        OnBulletImpact?.Invoke(this, EventArgs.Empty);

        if (other.GetComponent<EnemyController>())
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2f);
    }
}