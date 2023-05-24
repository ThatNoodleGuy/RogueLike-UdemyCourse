using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance { get; private set; }

    public event EventHandler OnPlayerDamageTaken;
    public event EventHandler OnPlayerHeal;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerSpawn;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] float damageInvincLength = 1f;

    private float invincibilityCounter;
    private SpriteRenderer playerSprite;

    private void Awake()
    {
        instance = this;
        OnPlayerSpawn?.Invoke(this, EventArgs.Empty);

        currentHealth = maxHealth;
    }

    private void Start()
    {
        playerSprite = PlayerController.instance.GetSpriteRenderer();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            if (invincibilityCounter <= 0)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void DamagePlayer()
    {
        if (invincibilityCounter <= 0)
        {
            currentHealth--;

            OnPlayerDamageTaken?.Invoke(this, EventArgs.Empty);

            invincibilityCounter = damageInvincLength;
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.4f);

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void MakeInvincible(float invincibilityLength)
    {
        invincibilityCounter = invincibilityLength;
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.4f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnPlayerHeal?.Invoke(this, EventArgs.Empty);
    }
}