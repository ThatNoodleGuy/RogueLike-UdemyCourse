using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public static event EventHandler OnBuyItem;
    public static event EventHandler OnCanNotBuy;

    [SerializeField] private TextMeshProUGUI purchaseText;
    [SerializeField] private bool isHealthRestore, isHealthUpgrade, isWeapon;
    [SerializeField] private int itemCost;
    [SerializeField] private int healthUpgradeAmount;

    private bool inBuyZone;

    private void Awake()
    {
        purchaseText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inBuyZone && Input.GetKeyDown(KeyCode.E))
        {
            if (LevelManager.instance.GetCurrentCoins() >= itemCost)
            {
                LevelManager.instance.SpendCoins(itemCost);
                if (isHealthRestore)
                {
                    PlayerHealth.instance.HealPlayer(PlayerHealth.instance.GetMaxHealth());
                }

                if (isHealthUpgrade)
                {
                    PlayerHealth.instance.UpgradeHealth(healthUpgradeAmount);
                }

                if (isWeapon)
                {
                    // Get new Weapon
                }

                gameObject.SetActive(false);
                inBuyZone = false;

                OnBuyItem?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnCanNotBuy?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            purchaseText.gameObject.SetActive(true);
            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            purchaseText.gameObject.SetActive(false);
            inBuyZone = false;
        }
    }
}