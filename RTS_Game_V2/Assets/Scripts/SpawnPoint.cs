using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(BoxCollider))]
public class SpawnPoint : MonoBehaviour
{
    //public Renderer render;
    public enum SpawnType
    {
        NORTH,
        EAST,
        SOUTH,
        WEST,
    }

    public enum SpawnerStatus
    {
        UNCHECKED,
        BLOCKED,
        EMPTY,
        ENABLED
    }

    [SerializeField] private BoxCollider coll;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isStartSpawnPoint;

    private SpawnerStatus spawnStatus;

    
    public SpawnType SpawnerType { get; set; }
    public SpawnerStatus SpawnStatus { get => spawnStatus; }
    private void Awake()
    {
        rb.mass = 0;
        rb.useGravity = false;
        coll.isTrigger = true;
        spawnStatus = SpawnerStatus.EMPTY;
    }

    private void Start()
    {
        if (isStartSpawnPoint)
        {
            ActiveSpawner();
            RoomsGenerator.instance.RunNextSpawner();
        }
    }

    public bool ActiveSpawner()
    {
        if(spawnStatus == SpawnerStatus.BLOCKED)
        {
            return false;
        }

        if (RoomsGenerator.instance.RoomsLeft > 0)
        {
            spawnStatus = SpawnerStatus.ENABLED;
            RoomsGenerator.instance.AddSpawnPoint(this);
            return true;
        }

        spawnStatus = SpawnerStatus.EMPTY;
        return false;
    }

    public bool EnableSpawn()
    {
        if(spawnStatus == SpawnerStatus.ENABLED)
        {
            RoomSO newRoomSO = RoomsGenerator.instance.GetRoom();
            GameObject newRoom = Instantiate(newRoomSO.RoomPlane, gameObject.transform.position, Quaternion.identity);
            GameController.instance.spawnedRooms.Add(newRoom);
            newRoom.SetActive(false);
            Room roomScript = newRoom.AddComponent(typeof(Room)) as Room;
            if (isStartSpawnPoint)
            {
                roomScript.SetEssentials(newRoomSO);
            }
            else
            {
                if (RoomsGenerator.instance.RoomsLeft == 0)
                {
                    roomScript.SetEssentials(newRoomSO, SpawnerType, true);
                }
                roomScript.SetEssentials(newRoomSO, SpawnerType);
            }
            newRoom.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomPlane"))
        {
            spawnStatus = SpawnerStatus.BLOCKED;
        }
        else
        {
            if (other.CompareTag("Spawner"))
            {
                SpawnPoint sp = other.gameObject.GetComponent<SpawnPoint>();

                if(sp.SpawnStatus == SpawnerStatus.ENABLED)
                {
                    spawnStatus = SpawnerStatus.BLOCKED;
                }
            }
        }


    }

}
