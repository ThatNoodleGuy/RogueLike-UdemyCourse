using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public static event EventHandler OnObjectBreak;

    [SerializeField] private GameObject[] brokenPieces;
    [SerializeField] private int maxPieces = 5;
    [SerializeField] private bool shouldDropItem;
    [SerializeField] private GameObject[] itemsToDrop;
    [SerializeField] private float itemDropPercent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.GetComponent<PlayerController>() && PlayerController.instance.GetDashCounter() > 0) || other.GetComponent<PlayerBulletController>())
        {
            Destroy(gameObject);
            OnObjectBreak?.Invoke(this, EventArgs.Empty);

            // Spawn broken pieces
            int piecesToDrop = UnityEngine.Random.Range(1, maxPieces);

            for (int i = 0; i < piecesToDrop; i++)
            {
                int randomPiece = UnityEngine.Random.Range(0, brokenPieces.Length);

                Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
            }

            // Drop items
            if (shouldDropItem)
            {
                float dropChance = UnityEngine.Random.Range(0f, 100f);

                if (dropChance <= itemDropPercent)
                {
                    int randomitem = UnityEngine.Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomitem], transform.position, transform.rotation);
                }
            }

        }
    }
}