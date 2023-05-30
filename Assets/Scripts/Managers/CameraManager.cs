using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera bigMapCamera;

    private bool bigMapActive;
    private bool playerCanMove;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        if (!LevelManager.instance.IsPaused())
        {
            bigMapActive = true;
            mainCamera.enabled = false;
            bigMapCamera.enabled = true;

            PlayerController.instance.PlayerCantMove();
            Time.timeScale = 0f;
            UIController.instance.GetMapDisplay().SetActive(false);
            UIController.instance.GetBigMapTextGameObject().SetActive(true);
        }
    }

    public void DeactivateBigMap()
    {
        if (!LevelManager.instance.IsPaused())
        {
            bigMapActive = false;
            mainCamera.enabled = true;
            bigMapCamera.enabled = false;

            PlayerController.instance.PlayerCanMove();
            Time.timeScale = 1f;
            UIController.instance.GetMapDisplay().SetActive(true);
            UIController.instance.GetBigMapTextGameObject().SetActive(false);
        }
    }
}