using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    public event EventHandler OnPlayerDash;
    //public event EventHandler OnPlayerShooting;

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
    [SerializeField] private List<Gun> availableGuns = new List<Gun>();
    private int currentGun;

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
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            playerRB.velocity = moveInput * activeMoveSpeed;

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

            if (moveInput != Vector2.zero)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            if (Input.GetKeyDown(KeyCode.Tab) || (Input.mouseScrollDelta.y != 0))
            {
                if (availableGuns.Count > 0)
                {
                    if (Input.mouseScrollDelta.y > 0)
                    {
                        {
                            currentGun++;
                            if (currentGun >= availableGuns.Count)
                            {
                                currentGun = 0;
                            }

                            SwitchGun();
                        }
                    }
                    else if (Input.mouseScrollDelta.y < 0)
                    {
                        if (currentGun == 0)
                        {
                            currentGun = availableGuns.Count;
                        }
                        currentGun--;

                        SwitchGun();
                    }
                    else
                    {
                        Debug.Log("Player Has No Guns!");
                    }
                }
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

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SwitchGun()
    {
        foreach (Gun gun in availableGuns)
        {
            gun.gameObject.SetActive(false);
        }

        availableGuns[currentGun].gameObject.SetActive(true);
    }
}