using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered = false;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject mapHider;

    private bool isRoomActive;

    private void Start()
    {
        mapHider.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            CameraManager.instance.ChangeTarget(transform);

            if (closeWhenEntered)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            isRoomActive = true;
            mapHider.SetActive(false);
        }
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            isRoomActive = false;
        }
    }

    public bool GetIsRoomActive()
    {
        return isRoomActive;
    }
}