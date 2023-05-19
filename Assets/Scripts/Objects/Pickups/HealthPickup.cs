using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;
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
            PlayerHealth.instance.HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }
}