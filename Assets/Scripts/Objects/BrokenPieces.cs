using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float lifeTime = 3f;

    private Vector3 moveDirection;
    private SpriteRenderer spriteRenderer;
    private float fadeTime = 2.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        moveDirection.x = UnityEngine.Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = UnityEngine.Random.Range(-moveSpeed, moveSpeed);
    }

    private void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r,
                                             spriteRenderer.color.g,
                                             spriteRenderer.color.b,
                                             Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeTime * Time.deltaTime));
            
            if (spriteRenderer.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}