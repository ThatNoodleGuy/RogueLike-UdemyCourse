using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    };

    [SerializeField] private GameObject layoutRoom;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private int distanceToEnd;
    [SerializeField] private Transform generatorPoint;
    [SerializeField] private Direction selectedDirection;
    [SerializeField] private float xOffset = 18f;
    [SerializeField] private float yOffset = 10f;
    [SerializeField] private LayerMask roomLayoutLayer;
    [SerializeField] private List<RoomPrefab> roomPrefabs = new List<RoomPrefab>();
    [SerializeField] private RoomCenter centerStart, centerEnd;
    [SerializeField] private RoomCenter[] potantialCenters;

    private GameObject endRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    private List<GameObject> generatedOutlines = new List<GameObject>();

    private void Awake()
    {
        GameObject startRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
        startRoom.GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)UnityEngine.Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)UnityEngine.Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayoutLayer))
            {
                MoveGenerationPoint();
            }
        }

        // Create room outline
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (generateCenter)
            {
                int centerSelect = UnityEngine.Random.Range(0, potantialCenters.Length);
                Instantiate(potantialCenters[centerSelect], outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
            }

        }
    }

    private void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.Up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.Down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.Right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.Left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
            default:
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool exitUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset), .2f, roomLayoutLayer);
        bool exitDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset), .2f, roomLayoutLayer);
        bool exitRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f), .2f, roomLayoutLayer);
        bool exitLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f), .2f, roomLayoutLayer);

        var roomPrefab = PickRoom(exitUp, exitRight, exitDown, exitLeft);
        if (roomPrefab != null)
        {
            GameObject room = Instantiate(roomPrefab.prefab, roomPosition, Quaternion.identity, transform);

            generatedOutlines.Add(room);
        }

    }

    private RoomPrefab PickRoom(bool exitUp, bool exitRight, bool exitDown, bool exitLeft)
    {
        foreach (RoomPrefab rp in roomPrefabs)
        {
            if (rp.exitUp == exitUp && rp.exitRight == exitRight && rp.exitDown == exitDown && rp.exitLeft == exitLeft)
            {
                return rp;
            }
        }
        return null;
    }
}

[System.Serializable]
public class RoomPrefab
{
    public bool exitUp;
    public bool exitRight;
    public bool exitDown;
    public bool exitLeft;
    public GameObject prefab;
}