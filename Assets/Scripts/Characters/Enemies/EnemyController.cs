using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static event EventHandler OnEnemyHealthChange;
    public static event EventHandler OnEnemyDeath;
    public static event EventHandler OnEnemyShooting;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rangeToChasePlayer;
    [SerializeField] private float rangeToShootAtPlayer;
    [SerializeField] private int health = 150;
    [SerializeField] private GameObject[] deathSplatters;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private bool shouldShoot;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float fireCounter;
    private Rigidbody2D bodyRB;
    private Vector3 moveDir;
    private Animator animator;

    private void Awake()
    {
        bodyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (spriteRenderer.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            HandleMovement();
            HandleShooting();
        }
        else
        {
            bodyRB.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    private void HandleMovement()
    {
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
        {
            moveDir = (PlayerController.instance.transform.position - transform.position);
        }
        else
        {
            moveDir = Vector3.zero;
        }

        moveDir.Normalize();

        bodyRB.velocity = moveDir * moveSpeed;

        if (moveDir != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (PlayerController.instance.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void HandleShooting()
    {
        if (shouldShoot && (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToShootAtPlayer))
        {
            fireCounter -= Time.deltaTime;

            if (fireCounter <= 0)
            {
                fireCounter = fireRate;
                GameObject enemyBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                OnEnemyShooting?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        OnEnemyHealthChange?.Invoke(this, EventArgs.Empty);

        Instantiate(hitVFX, transform.position, transform.rotation);

        if (health <= 0)
        {
            Destroy(gameObject);
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);

            int selectedSplatter = UnityEngine.Random.Range(0, deathSplatters.Length);
            int randomRotation = UnityEngine.Random.Range(0, 30);

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, randomRotation * 90f));
        }
    }
}