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

    private void Update()
    {
        ////delete in final
        //switch (spawnStatus)
        //{
        //    case SpawnerStatus.UNCHECKED:
        //        render.material.color = Color.white;
        //        break;
        //    case SpawnerStatus.BLOCKED:
        //        render.material.color = Color.red;
        //        break;
        //    case SpawnerStatus.EMPTY:
        //        render.material.color = Color.blue;
        //        break;
        //    case SpawnerStatus.ENABLED:
        //        render.material.color = Color.green;
        //        break;
        //    default:
        //        break;
        //}
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RoomPlane"))
        {
            spawnStatus = SpawnerStatus.BLOCKED;
        }
    }
}
