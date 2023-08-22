using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(BoxCollider))]
public class SpawnPoint : MonoBehaviour
{
    public enum SpawnType
    {
        NORTH,
        EAST,
        SOUTH,
        WEST,
    }
    private BoxCollider coll;
    private Rigidbody rb;
    public SpawnType spawnType;
    public bool isStartSpawnPoint;
    public bool isEmpty;
    public bool isBlocked;
    public bool isBlockedByRoom;
    public bool isActivated;
    public int spawnId;
    public SpawnStatus spawnStatus;
    private bool isChecked;

    private bool isLastSpawn;

    public  enum SpawnStatus
    {
        BLOCKED,
        EMPTY,
        SPAWNED
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<BoxCollider>();
        spawnId = gameObject.GetInstanceID();
        spawnStatus = SpawnStatus.EMPTY;
    }
    void Start()
    {
        GameEvents.instance.OnSpawn += SpawnRoom;
        rb.mass = 0;
        rb.useGravity = false;
        coll.isTrigger = true;
        if (isStartSpawnPoint)
        {
            spawnStatus = SpawnStatus.SPAWNED;
            ActiveSpawner();
            GameEvents.instance.Spawn(spawnId);
        }
        StartCoroutine(Checking());
    }



    public void Update()
    {
        if (!isBlocked)
        {
            if (CheckRoomsLeft() && isActivated)
            {
                spawnStatus = SpawnStatus.SPAWNED;
            }
            else
            {
                spawnStatus = SpawnStatus.EMPTY;
            }
        }
        else
        {
            if (isBlockedByRoom)
            {
                spawnStatus = SpawnStatus.BLOCKED;
            }
            else
            {
                spawnStatus = SpawnStatus.EMPTY;
            }
        }
    }


    private IEnumerator Checking()
    {
        yield return new WaitForSeconds(1f);
        isChecked = true;
    }


    private void SpawnRoom(int spawnId)
    {
        if(spawnStatus == SpawnStatus.SPAWNED)
        {
            RoomSO newRoomSO = RoomsGenerator.instance.GetRoom();
            GameObject newRoom = Instantiate(newRoomSO.RoomPlane(), gameObject.transform.position, Quaternion.identity);
            newRoom.SetActive(false);
            Room roomScript = newRoom.AddComponent(typeof(Room)) as Room;
            if (isStartSpawnPoint)
            {
                roomScript.SetEssentials(newRoomSO);
            }
            else
            {
                if(RoomsGenerator.instance.GetRoomsLeft() == 0)
                {
                    isLastSpawn = true;
                }
                roomScript.SetEssentials(newRoomSO, spawnType, isLastSpawn);
            }
            newRoom.SetActive(true);

        }

        gameObject.tag = "UnactivatedSpawner";
        GameEvents.instance.OnSpawn -= SpawnRoom;
    }

    private bool CheckRoomsLeft()
    {
        int roomsLeft = RoomsGenerator.instance.GetRoomsLeft();
        if (roomsLeft > 0)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RoomPlane"))
        {
            isBlocked = true;
            isBlockedByRoom = true;
            gameObject.tag = "UnactivatedSpawner";
        }

        if (other.CompareTag("ActivatedSpawner"))
        {
            isBlocked = true;
            gameObject.tag = "UnactivatedSpawner";
        }
    }
    public void ActiveSpawner()
    {
        isActivated = true;
        gameObject.tag = "ActivatedSpawner";
    }

    public bool IsChecked()
    {
        return isChecked;
    }

    public SpawnStatus GetSpawnStatus()
    {
        return spawnStatus;
    }
}
