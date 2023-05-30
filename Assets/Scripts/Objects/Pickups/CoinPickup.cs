using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public static event EventHandler OnCoinPickup;

    [SerializeField] private int coinValue = 1;
    [SerializeField] private float waitToBeCollected = 0.5f;

    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() && waitToBeCollected <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}