using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private bool openWhenEnemiesCleared;

    public Room room;

    private void Start()
    {
        if (openWhenEnemiesCleared)
        {
            room.closeWhenEntered = true;
        }
    }

    private void Update()
    {
        if (enemies.Count > 0 && room.GetIsRoomActive() && openWhenEnemiesCleared)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            if (enemies.Count == 0)
            {
                room.OpenDoors();
            }
        }
    }
}