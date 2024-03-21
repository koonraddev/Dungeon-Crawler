using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameObject northSpawnPointObject, southSpawnPointObject, westSpawnPointObject, eastSpawnPointObject;
    private SpawnPoint northSpawnPoint, southSpawnPoint, westSpawnPoint, eastSpawnPoint;
    private SpawnPoint[] spawnsArray = new SpawnPoint[4];
    private Vector3 planeSize;
    private Dictionary<SpawnType, bool> portalsDict;
    private RoomSO roomSO;
    private HashSet<int> exclude = new HashSet<int>() { };
    private GameObject spawner;

    private SpawnType entryPortal;
    private bool isLastRoom, isFirstRoom;
    int spawnersToActivate;

    private void Awake()
    {
        planeSize = GetComponent<Collider>().bounds.size;
        spawner = Resources.Load("StartSpawnPoint") as GameObject;
        portalsDict = new();

        foreach (SpawnType item in Enum.GetValues(typeof(SpawnType)))
        {
            portalsDict.Add(item, false);
        }
    }


    private void OnEnable()
    {
        GameEvents.instance.OnStartLevel += Ready;
    }

    private void Ready()
    {
        if (!isFirstRoom)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        var rand = new System.Random();
        spawnersToActivate = rand.Next(1, 5);
        if (exclude.Count() > 0 && spawnersToActivate > 1)
        {
            spawnersToActivate--;
        }
        SpawnPoints();
    }

    public void SetEssentials(RoomSO roomSO, SpawnType spawnerType)
    {
        this.roomSO = roomSO;
        isFirstRoom = roomSO is StartRoomSO;
        isLastRoom = roomSO is BossRoomSO;

        if (!isFirstRoom)
        {
            switch (spawnerType)
            {
                case SpawnType.NORTH:
                    entryPortal = SpawnType.SOUTH;
                    exclude.Add(2);
                    break;
                case SpawnType.EAST:
                    entryPortal = SpawnType.WEST;
                    exclude.Add(3);
                    break;
                case SpawnType.SOUTH:
                    entryPortal = SpawnType.NORTH;
                    exclude.Add(0);
                    break;
                case SpawnType.WEST:
                    entryPortal = SpawnType.EAST;
                    exclude.Add(1);
                    break;
                default:
                    break;
            }
        }
    }

    public void SpawnPoints()
    {
        northSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(0f, 0f, planeSize.z), Quaternion.identity);
        northSpawnPointObject.name = "NorthSpawnPoint";
        northSpawnPointObject.transform.SetParent(gameObject.transform);
        northSpawnPoint = northSpawnPointObject.GetComponent<SpawnPoint>();
        northSpawnPoint.SpawnerType = SpawnType.NORTH;

        eastSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(planeSize.x, 0f, 0f), Quaternion.identity);
        eastSpawnPointObject.name = "EastSpawnPoint";
        eastSpawnPointObject.transform.SetParent(gameObject.transform);
        eastSpawnPoint = eastSpawnPointObject.GetComponent<SpawnPoint>();
        eastSpawnPoint.SpawnerType = SpawnType.EAST;

        southSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(0f, 0f, -planeSize.z), Quaternion.identity);
        southSpawnPointObject.name = "SouthSpawnPoint";
        southSpawnPointObject.transform.SetParent(gameObject.transform);
        southSpawnPoint = southSpawnPointObject.GetComponent<SpawnPoint>();
        southSpawnPoint.SpawnerType = SpawnType.SOUTH;

        westSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(-planeSize.x, 0f, 0f), Quaternion.identity);
        westSpawnPointObject.name = "WestSpawnPoint";
        westSpawnPointObject.transform.SetParent(gameObject.transform);
        westSpawnPoint = westSpawnPointObject.GetComponent<SpawnPoint>();
        westSpawnPoint.SpawnerType = SpawnType.WEST;

        spawnsArray[0] = northSpawnPoint;
        spawnsArray[1] = eastSpawnPoint;
        spawnsArray[2] = southSpawnPoint;
        spawnsArray[3] = westSpawnPoint;

        StartCoroutine(SO());
    }


    IEnumerator SO()
    {
        var rand = new System.Random();
        for (int i = 0; i < spawnsArray.Length; i++)
        {
            var range = Enumerable.Range(0, 4).Where(i => !exclude.Contains(i));
            if (spawnersToActivate == 0)
            {
                break;
            }
            if (!exclude.Contains(i))
            {
                if(spawnersToActivate > 0)
                {
                    int index = rand.Next(0, 4 - exclude.Count());
                    int newValue = range.ElementAt(index);
                    exclude.Add(newValue);

                    if (spawnsArray[newValue].ActiveSpawner())
                    {
                        spawnersToActivate--;
                    }
                }
            }
        }
        yield return new WaitForEndOfFrame();
        Check();
    }

    void Check()
    { 
        foreach (SpawnPoint spawnPoint in spawnsArray)
        {
            switch (spawnPoint.SpawnStatus)
            {

                case SpawnPoint.SpawnerStatus.ENABLED:
                    portalsDict[spawnPoint.SpawnerType] = true;
                    break;
                case SpawnPoint.SpawnerStatus.UNCHECKED:
                case SpawnPoint.SpawnerStatus.BLOCKED:
                case SpawnPoint.SpawnerStatus.EMPTY:
                    portalsDict[spawnPoint.SpawnerType] = false;
                    break;
                default:
                    portalsDict[spawnPoint.SpawnerType] = false;
                    break;
            }

        }


        roomSO.RoomBehavoiur(gameObject, isLastRoom);
        PortalSpawner portalSpawner = gameObject.AddComponent(typeof(PortalSpawner)) as PortalSpawner;
        

        if (!isFirstRoom)
        {
            portalsDict[entryPortal] = true;
            portalSpawner.SetEssentials(roomSO, portalsDict, entryPortal);
        }
        else
        {
            portalSpawner.SetEssentials(roomSO, portalsDict);
        }
        RoomsGenerator.instance.RunNextSpawner();
    }


    private void OnDisable()
    {
        GameEvents.instance.OnStartLevel -= Ready;
    }
}
