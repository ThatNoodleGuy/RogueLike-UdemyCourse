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

    [Header("Chase Player")]
    [SerializeField] private bool shouldChasePlayer;
    [SerializeField] private float rangeToChasePlayer;
    private Vector3 moveDir;

    [Header("Run Away")]
    [SerializeField] private bool shouldRunAway;
    [SerializeField] private float runAwayRange;

    [Header("Wandering")]
    [SerializeField] private bool shouldWander;
    [SerializeField] private float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDir;

    [Header("Patrolling")]
    [SerializeField] private bool shouldPatrol;
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("Shooting")]
    [SerializeField] private bool shouldShoot;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate;
    private float fireCounter;
    [SerializeField] private float rangeToShootAtPlayer;

    [Header("Variables")]
    [SerializeField] private int health = 150;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject[] deathSplatters;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private bool shouldDropItem;
    [SerializeField] private GameObject[] itemsToDrop;
    [SerializeField] private float itemDropPercent;

    private Rigidbody2D bodyRB;
    private Animator animator;

    private void Awake()
    {
        bodyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (shouldWander)
        {
            pauseCounter = UnityEngine.Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
            wanderCounter = UnityEngine.Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);
        }
    }

    private void Update()
    {
        if (spriteRenderer.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDir = Vector3.zero;

            if ((Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer) && shouldChasePlayer)
            {
                moveDir = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if (wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        // move the enemy
                        moveDir = wanderDir;

                        if (wanderCounter <= 0)
                        {
                            pauseCounter = UnityEngine.Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                        }
                    }

                    if (pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if (pauseCounter <= 0)
                        {
                            wanderCounter = UnityEngine.Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);

                            wanderDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDir = patrolPoints[currentPatrolPoint].position - transform.position;

                    if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.2f)
                    {
                        currentPatrolPoint++;
                        if (currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            if (shouldRunAway && (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runAwayRange))
            {
                moveDir = transform.position - PlayerController.instance.transform.position;
            }

            if (moveDir.x > 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
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
        else
        {
            bodyRB.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
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

            if (shouldDropItem)
            {
                float dropChance = UnityEngine.Random.Range(0f, 100f);

                if (dropChance <= itemDropPercent)
                {
                    int randomitem = UnityEngine.Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomitem], transform.position, transform.rotation);
                }
            }

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, randomRotation * 90f));
        }
    }
}