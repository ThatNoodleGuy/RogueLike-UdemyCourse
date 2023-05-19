using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject[] brokenPieces;
    [SerializeField] private int maxPieces = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (PlayerController.instance.GetDashCounter() > 0)
            {
                Destroy(gameObject);

                int piecesToDrop = UnityEngine.Random.Range(1, maxPieces);

                for (int i = 0; i < piecesToDrop; i++)
                {
                    int randomPiece = UnityEngine.Random.Range(0, brokenPieces.Length);

                    Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
                }
            }
        }
    }
}