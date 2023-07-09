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
    private int[] doorsArray = new int[4];
    private RoomSO roomSO;
    private HashSet<int> exclude = new HashSet<int>() { };
    private int spawnersToActivate;
    private bool readyTo;
    private GameObject spawner;
    private bool pointsCanBeChecked;

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
        StartCoroutine(SpawnPoints());
    }


    public void SetEssentials(RoomSO roomSO, SpawnPoint.SpawnType spawnToExclude)
    {
        this.roomSO = roomSO;
        switch (spawnToExclude)
        {
            case SpawnPoint.SpawnType.NORTH:
                exclude.Add(2);
                break;
            case SpawnPoint.SpawnType.EAST:
                exclude.Add(3);
                break;
            case SpawnPoint.SpawnType.SOUTH:
                exclude.Add(0);
                break;
            case SpawnPoint.SpawnType.WEST:
                exclude.Add(1);
                break;
            default:
                break;
        }

    }

    public void SetEssentials(RoomSO roomSO)
    {
        this.roomSO = roomSO;
    }


    public IEnumerator SpawnPoints()
    {
        northSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(0f, 0f, planeSize.z), Quaternion.identity);
        northSpawnPointObject.name = "NorthSpawnPoint";
        northSpawnPointObject.tag = "UnactivatedSpawner";
        northSpawnPointObject.transform.SetParent(gameObject.transform);
        northSpawnPoint = northSpawnPointObject.GetComponent<SpawnPoint>();
        northSpawnPoint.spawnType = SpawnPoint.SpawnType.NORTH;

        yield return new WaitForEndOfFrame();

        eastSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(planeSize.x, 0f, 0f), Quaternion.identity);
        eastSpawnPointObject.name = "EastSpawnPoint";
        eastSpawnPointObject.tag = "UnactivatedSpawner";
        eastSpawnPointObject.transform.SetParent(gameObject.transform);
        eastSpawnPoint = eastSpawnPointObject.GetComponent<SpawnPoint>();
        eastSpawnPoint.spawnType = SpawnPoint.SpawnType.EAST;

        yield return new WaitForEndOfFrame();

        southSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(0f, 0f, -planeSize.z), Quaternion.identity);
        southSpawnPointObject.name = "SouthSpawnPoint";
        southSpawnPointObject.tag = "UnactivatedSpawner";
        southSpawnPointObject.transform.SetParent(gameObject.transform);
        southSpawnPoint = southSpawnPointObject.GetComponent<SpawnPoint>();
        southSpawnPoint.spawnType = SpawnPoint.SpawnType.SOUTH;

        yield return new WaitForEndOfFrame();

        westSpawnPointObject = Instantiate(spawner, gameObject.transform.position + new Vector3(-planeSize.x, 0f, 0f), Quaternion.identity);
        westSpawnPointObject.name = "WestSpawnPoint";
        westSpawnPointObject.tag = "UnactivatedSpawner";
        westSpawnPointObject.transform.SetParent(gameObject.transform);
        westSpawnPoint = westSpawnPointObject.GetComponent<SpawnPoint>();
        westSpawnPoint.spawnType = SpawnPoint.SpawnType.WEST;

        spawnsArray[0] = northSpawnPoint;
        spawnsArray[1] = eastSpawnPoint;
        spawnsArray[2] = southSpawnPoint;
        spawnsArray[3] = westSpawnPoint;
        pointsCanBeChecked = true;
    }

    public void Update()
    {
        if (pointsCanBeChecked)
        {
            ActivatePoints();
            if(!readyTo)
            {
                readyTo = AreSpawnersReady();
            }
        }

    }

    private bool AreSpawnersReady()
    {
        foreach (SpawnPoint spawn in spawnsArray)
        {
            if (!spawn.IsChecked())
            {
                return false;
            }
        }
        return true;
    }


    private void ActivatePoints()
    {
        if (spawnersToActivate > 0)
        {
            var rand = new System.Random();

            var range = Enumerable.Range(0, 4).Where(i => !exclude.Contains(i));
            int index = rand.Next(0, 4 - exclude.Count());
            int newValue = range.ElementAt(index);
            exclude.Add(newValue);

            spawnsArray[newValue].ActiveSpawner();

            spawnersToActivate--;

        }
        else
        {
            if (spawnersToActivate == 0)
            {
                StartCoroutine(Check());
            }
            spawnersToActivate--;
        }
    }

    IEnumerator Check()
    { 
        yield return new WaitWhile(() => readyTo == false); 
        var rand = new System.Random();
        int iterator = 0;
        foreach (SpawnPoint spawnPoint in spawnsArray)
        {
            switch (spawnPoint.GetSpawnStatus())
            {
                case SpawnPoint.SpawnStatus.BLOCKED:
                    doorsArray[iterator] = -1; 
                    break;
                case SpawnPoint.SpawnStatus.EMPTY:
                    doorsArray[iterator] = 0;
                    break;
                case SpawnPoint.SpawnStatus.SPAWNED:
                    doorsArray[iterator] = rand.Next(1, roomSO.MaxDoorsInWall() + 1);
                    GameEvents.instance.Spawn(spawnPoint.spawnId);
                    break;
                default:
                    break;
            }

            iterator++;
        }

        SpawnWalls spawnScript = gameObject.AddComponent(typeof(SpawnWalls)) as SpawnWalls;
        spawnScript.SetEssentials(roomSO, doorsArray);
        roomSO.RoomBehavoiur(this.gameObject);
    }
}
