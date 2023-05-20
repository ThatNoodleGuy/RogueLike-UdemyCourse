using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public static event EventHandler OnBulletImpact;

    [SerializeField] private float moveSpeed;
    [SerializeField] private ParticleSystem bulletImpactEffect;

    private Vector3 direction;

    private void Start()
    {
        direction = (PlayerController.instance.transform.position - transform.position).normalized;
    }

    private void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ParticleSystem impactEffect = Instantiate(bulletImpactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        OnBulletImpact?.Invoke(this, EventArgs.Empty);

        if (other.GetComponent<PlayerController>())
        {
            PlayerHealth.instance.DamagePlayer();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2f);
    }
}