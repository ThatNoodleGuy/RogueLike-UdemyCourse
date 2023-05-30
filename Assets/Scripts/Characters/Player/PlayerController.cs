using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    public event EventHandler OnPlayerDash;
    public event EventHandler OnPlayerShooting;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private float activeMoveSpeed;
    [SerializeField] private float dashSpeed = 8f, dashLength = 0.5f, dashCooldown = 1f, dashInvincibility = 0.5f;
    private float dashCounter, dashCooldownCounter;
    private bool isDashing = false;
    [HideInInspector]
    [SerializeField] private bool canMove = true;

    [Header("Shooting")]
    [SerializeField] private Transform gunHand;
    [SerializeField] private Transform gunFirePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float timeBetweenShots;
    private float shotCounter;

    [Header("Misc")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Camera mainCamera;
    private Rigidbody2D playerRB;
    private Animator animator;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        activeMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (!canMove)
        {
            playerRB.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        if (!LevelManager.instance.IsPaused())
        {
            HandleMovement();
            HandleGun();
        }
    }

    private void HandleMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        playerRB.velocity = moveInput * activeMoveSpeed;

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCooldownCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;

                animator.SetTrigger("Dash");
                OnPlayerDash?.Invoke(this, EventArgs.Empty);
                PlayerHealth.instance.MakeInvincible(dashInvincibility);
            }
        }

        if (dashCounter > 0)
        {
            isDashing = true;
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCooldownCounter = dashCooldown;
                isDashing = false;
            }
        }

        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    private void HandleGun()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.localPosition);

        if (mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunHand.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunHand.localScale = Vector3.one;
        }

        Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunHand.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletGameObject = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
            shotCounter = timeBetweenShots;
            OnPlayerShooting?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter == 0)
            {
                GameObject bulletGameObject = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
                shotCounter = timeBetweenShots;
                OnPlayerShooting?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }

    public float GetDashCounter()
    {
        return dashCounter;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public void PlayerCanMove()
    {
        canMove = true;
    }

    public void PlayerCantMove()
    {
        canMove = false;
    }
}