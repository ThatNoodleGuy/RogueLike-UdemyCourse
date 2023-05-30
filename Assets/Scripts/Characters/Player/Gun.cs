using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static event EventHandler OnPlayerShooting;

    [SerializeField] private Transform gunFirePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float timeBetweenShots;
    private float shotCounter;

    private void Update()
    {
        if (PlayerController.instance.GetCanMove() && !LevelManager.instance.IsPaused())
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    GameObject bulletGameObject = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
                    shotCounter = timeBetweenShots;
                    OnPlayerShooting?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}