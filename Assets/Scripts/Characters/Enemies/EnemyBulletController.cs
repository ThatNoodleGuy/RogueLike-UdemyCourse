using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

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
        if (other.GetComponent<PlayerController>())
        {
            PlayerHealth.instance.DamagePlayer();
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2f);
    }
}