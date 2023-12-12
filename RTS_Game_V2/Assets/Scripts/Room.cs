using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;


public class Room : MonoBehaviour
{
    private GameObject northSpawnPointObject, southSpawnPointObject, westSpawnPointObject, eastSpawnPointObject;
    private SpawnPoint northSpawnPoint, southSpawnPoint, westSpawnPoint, eastSpawnPoint;
    private SpawnPoint[] spawnsArray = new SpawnPoint[4];
    private Vector3 planeSize;
    private int[] doorsArray = new int[4];
    private RoomSO roomSO;
    private HashSet<int> exclude = new HashSet<int>() { };
    private GameObject spawner;

    public bool isLastRoom;
    int spawnersToActivate;

    private void Awake()
    {
        planeSize = GetComponent<Collider>().bounds.size;
        spawner = Resources.Load("StartSpawnPoint") as GameObject;
    }
    void Start()
    {
        var rand = new System.Random();
        spawnersToActivate = rand.Next(1, 5);
        if (exclude.Count() > 0)
        {
            if (spawnersToActivate > 1)
            {
                spawnersToActivate--;
            }
        }
        SpawnPoints();
    }

    public void SetEssentials(RoomSO roomSO, SpawnPoint.SpawnType spawnToExclude, bool isLastRoom = false)
    {
        this.roomSO = roomSO;
        this.isLastRoom = isLastRoom;
        switch (spawnToExclude)
        {
            case SpawnPoint.SpawnType.NORTH:
                exclude.Add(2);
                doorsArray[2] = 0;
                break;
            case SpawnPoint.SpawnType.EAST:
                exclude.Add(3);
                doorsArray[3] = 0;
                break;
            case SpawnPoint.SpawnType.SOUTH:
                exclude.Add(0);
                doorsArray[0] = 0;
                break;
            case SpawnPoint.SpawnType.WEST:
                exclude.Add(1);
                doorsArray[1] = 0;
                break;
            default:
                break;
        }
    }

    public void SetEssentials(RoomSO roomSO)
    {
        this.roomSO = roomSO;
    }

    public void SpawnPoints()
    {
        northSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(0f, 0f, planeSize.z), Quaternion.identity);
        northSpawnPointObject.name = "NorthSpawnPoint";
        northSpawnPointObject.transform.SetParent(gameObject.transform);
        northSpawnPoint = northSpawnPointObject.GetComponent<SpawnPoint>();
        northSpawnPoint.SpawnerType = SpawnPoint.SpawnType.NORTH;

        eastSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(planeSize.x, 0f, 0f), Quaternion.identity);
        eastSpawnPointObject.name = "EastSpawnPoint";
        eastSpawnPointObject.transform.SetParent(gameObject.transform);
        eastSpawnPoint = eastSpawnPointObject.GetComponent<SpawnPoint>();
        eastSpawnPoint.SpawnerType = SpawnPoint.SpawnType.EAST;

        southSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(0f, 0f, -planeSize.z), Quaternion.identity);
        southSpawnPointObject.name = "SouthSpawnPoint";
        southSpawnPointObject.transform.SetParent(gameObject.transform);
        southSpawnPoint = southSpawnPointObject.GetComponent<SpawnPoint>();
        southSpawnPoint.SpawnerType = SpawnPoint.SpawnType.SOUTH;

        westSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(-planeSize.x, 0f, 0f), Quaternion.identity);
        westSpawnPointObject.name = "WestSpawnPoint";
        westSpawnPointObject.transform.SetParent(gameObject.transform);
        westSpawnPoint = westSpawnPointObject.GetComponent<SpawnPoint>();
        westSpawnPoint.SpawnerType = SpawnPoint.SpawnType.WEST;

        spawnsArray[0] = northSpawnPoint;
        spawnsArray[1] = eastSpawnPoint;
        spawnsArray[2] = southSpawnPoint;
        spawnsArray[3] = westSpawnPoint;

        StartCoroutine(SO());
    }


    IEnumerator SO()
    {
        yield return new WaitForSeconds(0.5f);
        var rand = new System.Random();
        var range = Enumerable.Range(0, 4).Where(i => !exclude.Contains(i));

        for (int i = 0; i < spawnsArray.Length; i++)
        {
            if(spawnersToActivate == 0)
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

                    spawnsArray[newValue].ActiveSpawner();

                    spawnersToActivate--;
                }
            }
        }

        Check();
    }

    void Check()
    { 
        var rand = new System.Random();
        int iterator = 0;
        foreach (SpawnPoint spawnPoint in spawnsArray)
        {
            switch (spawnPoint.SpawnStatus)
            {
                case SpawnPoint.SpawnerStatus.BLOCKED:
                case SpawnPoint.SpawnerStatus.EMPTY:
                    doorsArray[iterator] = 0;
                    break;
                case SpawnPoint.SpawnerStatus.ENABLED:
                    doorsArray[iterator] = rand.Next(1, roomSO.MaxDoorsInWall + 1);
                    break;
                default:
                    break;
            }

            iterator++;
        }

        bool firstRoom = false;
        if(gameObject.transform.position.x == 0 && gameObject.transform.position.z == 0)
        {
            firstRoom = true;
        }

        roomSO.RoomBehavoiur(gameObject, isLastRoom);
        PortalSpawner portalSpawner = gameObject.AddComponent(typeof(PortalSpawner)) as PortalSpawner;
        portalSpawner.SetEssentials(roomSO, doorsArray, firstRoom);

        if (isLastRoom)
        {
            GameEvents.instance.LastRoomReady();
        }
        else
        {
            RoomsGenerator.instance.RunNextSpawner();
        }
    }
}
