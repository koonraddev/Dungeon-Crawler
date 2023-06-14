using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class SpawnWalls : MonoBehaviour
{

    private List<DoorClass> doorClassList = new();

    [SerializeField] private List<DoorSO> doorslist;
    [SerializeField] private GameObject doorPrefab;

    [SerializeField] private GameObject plane;
    
    private BoxCollider doorCollider;
    private Vector3 planeSize;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 pointC;
    private Vector3 pointD;

    private Vector3 pointAx;
    private Vector3 pointAz;
    private Vector3 pointBx;
    private Vector3 pointBz;
    private Vector3 pointCx;
    private Vector3 pointCz;
    private Vector3 pointDx;
    private Vector3 pointDz;

    private Vector3 colliderSize;
    private Vector3 colliderCenter;

    [SerializeField] [HideInInspector] private int wallA;
    [SerializeField] [HideInInspector] private int wallB;
    [SerializeField] [HideInInspector] private int wallC;
    [SerializeField] [HideInInspector] private int wallD;

    [SerializeField] [HideInInspector] private bool generateEndingDoor;
    [SerializeField] [HideInInspector] private bool generateManyDoors;
    [SerializeField] [HideInInspector] private bool generateDoorRandomly;

    private int[] doorsArray;

    public class DoorClass
    {
        public DoorClass(int wallIndex, float positionInWall ,GameObject doorObject)
        {
            WallIndex = wallIndex;
            PositionInWall = positionInWall;
            DoorObject = doorObject;
        }
        public int WallIndex { get; }
        public GameObject DoorObject { get; }
        public float PositionInWall { get; }
    }
    

    void Start()
    {
        planeSize = plane.GetComponent<Collider>().bounds.size;
        doorCollider = doorPrefab.GetComponent<BoxCollider>();
        colliderSize = doorCollider.size;
        colliderCenter = doorCollider.center;
        pointA = new Vector3(-planeSize.x / 2, 0f, planeSize.z / 2);
        pointB = new Vector3(planeSize.x / 2, 0f, planeSize.z / 2);
        pointC = new Vector3(planeSize.x / 2, 0f, -planeSize.z / 2);
        pointD = new Vector3(-planeSize.x / 2, 0f, -planeSize.z / 2);

        pointAx = pointA + new Vector3(colliderSize.x, 0f, 0f);
        pointBx = pointB - new Vector3(colliderSize.x, 0f, 0f);
        pointCx = pointC - new Vector3(colliderSize.x, 0f, 0f);
        pointDx = pointD + new Vector3(colliderSize.x, 0f, 0f);

        pointAz = pointA - new Vector3(0f, 0f, colliderSize.x);
        pointBz = pointB - new Vector3(0f, 0f, colliderSize.x);
        pointCz = pointC + new Vector3(0f, 0f, colliderSize.x);
        pointDz = pointD + new Vector3(0f, 0f, colliderSize.x);

        GameEvents.instance.OnPrepareGame += Respawn;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnPrepareGame -= Respawn;
    }

    private int GetAxisPos(int minValue, int maxValue, HashSet<int> excludeHashSet = null, int attempt = 1)
    {
        if(maxValue <= minValue)
        {
            return 0;
        }
        int count = Mathf.Abs(minValue - maxValue + 1);

        var exclude = excludeHashSet;
        var rand = new System.Random();
        var range = Enumerable.Range(minValue, count).Where(i => !exclude.Contains(i));
        int index = rand.Next(0, range.Count() - exclude.Count());

        int newValue = range.ElementAt(index);

        if(attempt <= 10)
        {
            foreach (var item in exclude)
            {
                int distance = Mathf.Abs(newValue - item);
                if (distance < 2)
                {
                    newValue = GetAxisPos(minValue, maxValue, exclude, attempt + 1);
                }
            }
            return newValue;
        }
        return 0;
    }

    private void SpawnDoorsAndWalls()
    {
        if (!generateManyDoors)
        {
            doorsArray = new int[] { 0, 0, 0, 0 };
            int d = Random.Range(0, 4);
            doorsArray[d] = 1;
        }
        else
        {
            doorsArray = new int[] { wallA, wallB, wallC, wallD };
        }


        int endingDoorIndex = -1;
        if (generateEndingDoor)
        {
            int doorsAmount = 0;
            foreach (int amount in doorsArray)
            {
                doorsAmount += amount;
            }
            endingDoorIndex = Random.Range(0, doorsAmount);
        }

        int doorIterator = 0;
        for (int i = 0; i < doorsArray.Length; i++)
        {
            var exclude = new HashSet<int>() { };
            for (int j = 0; j < doorsArray[i]; j++)
            {
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;
                float positionInWall = j;

                if (!generateDoorRandomly)
                {
                    switch (i)
                    {
                        case 0:
                            pos = new Vector3(pointA.x + planeSize.x / (wallA + 1) * (j + 1), 0f, planeSize.z / 2);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                            positionInWall = pos.x;
                            break;
                        case 1:
                            pos = new Vector3(planeSize.x / 2, 0f, pointB.z - planeSize.z / (wallB + 1) * (j + 1));
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 90f));
                            positionInWall = pos.z;
                            break;
                        case 2:
                            pos = new Vector3(pointC.x - planeSize.x / (wallC + 1) * (j + 1), 0f, -planeSize.z / 2);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 180f));
                            positionInWall = pos.x;
                            break;
                        case 3:
                            pos = new Vector3(-planeSize.x / 2, 0f, pointD.z + planeSize.z / (wallD + 1) * (j + 1));
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 270f));
                            positionInWall = pos.z;
                            break;
                        default:
                            positionInWall = j;
                            break;
                    }
                }
                else
                {
                    int positionInCurrentWall = 0;
                    switch (i)
                    {
                        case 0:
                            positionInCurrentWall = GetAxisPos((int)pointAx.x, (int)pointBx.x, exclude);
                            exclude.Add(positionInCurrentWall);

                            pos = new Vector3(positionInCurrentWall, 0f, planeSize.z / 2);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                            positionInWall = positionInCurrentWall;
                            break;
                        case 1:
                            positionInCurrentWall = GetAxisPos((int)pointCz.z, (int)pointBz.z, exclude);
                            exclude.Add(positionInCurrentWall);

                            pos = new Vector3(planeSize.x / 2, 0f, positionInCurrentWall);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 90f));
                            positionInWall = positionInCurrentWall;
                            break;
                        case 2:
                            positionInCurrentWall = GetAxisPos((int)pointCx.x, (int)pointDx.x, exclude);
                            exclude.Add(positionInCurrentWall);

                            pos = new Vector3(positionInCurrentWall, 0f, -planeSize.z / 2);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 180f));
                            positionInWall = positionInCurrentWall;
                            break;
                        case 3:
                            positionInCurrentWall = GetAxisPos((int)pointDz.z, (int)pointAz.z, exclude);
                            exclude.Add(positionInCurrentWall);

                            pos = new Vector3(-planeSize.x / 2, 0f, positionInCurrentWall);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 270f));
                            positionInWall = positionInCurrentWall;
                            break;
                        default:
                            positionInWall = j;
                            pos = new Vector3(0f, 0f, 0f);
                            rot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                            break;
                    }
                }

                GameObject door = Instantiate(doorPrefab, pos, rot);
                Door doorScript = door.GetComponentInChildren<Door>();
                if (doorIterator == endingDoorIndex)
                {
                    doorScript.SetDoor(doorslist[Random.Range(0, doorslist.Count)], true);
                }
                else
                {
                    doorScript.SetDoor(doorslist[Random.Range(0, doorslist.Count)]);
                }

                DoorClass doorClass = new DoorClass(i, positionInWall, door);
                doorClassList.Add(doorClass);

                doorIterator++;
            }
        }

        List<DoorClass> doorsOrdered = doorClassList.OrderBy(order => order.PositionInWall).ToList();

        for (int i = 0; i < doorsArray.Length; i++)
        {
            List<DoorClass> currentWallDoors = doorsOrdered.Where(door => door.WallIndex == i).ToList();

            if(doorsArray[i] == 0)
            {
                ZeroDoors(i);
            }
            else
            {
                MultipleDoors(doorsArray, i, currentWallDoors);
            }
        }
    }

    private void GenerateWall(Vector3 startPoint, Vector3 endPoint, Color color)
    {
        if(doorCollider!= null)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 distanceStartEnd = startPoint - endPoint;
            wall.GetComponent<MeshRenderer>().material.color = color;
            wall.tag = "Wall";
            wall.transform.localScale = new Vector3(0.5f, colliderSize.z * doorPrefab.transform.localScale.y, distanceStartEnd.magnitude);
            wall.transform.position = (startPoint + endPoint) / 2f + new Vector3(0f, colliderCenter.z * doorPrefab.transform.localScale.y, 0f);
            wall.transform.LookAt(endPoint + new Vector3(0f, colliderCenter.z * doorPrefab.transform.localScale.y, 0f));
        }
    }


    private void ZeroDoors(int wallIndex)
    {
        Vector3 startPoint;
        Vector3 endPoint;
        switch (wallIndex)
        {
            case 0:
                startPoint = pointA;
                endPoint = pointB;
                break;
            case 1:
                startPoint = pointB;
                endPoint = pointC;
                break;
            case 2:
                startPoint = pointC;
                endPoint = pointD;
                break;
            case 3:
                startPoint = pointD;
                endPoint = pointA;
                break;
            default:
                startPoint = Vector3.zero;
                endPoint = Vector3.zero;
                break;
        }
        GenerateWall(startPoint, endPoint, Color.yellow);
    }

    private void MultipleDoors(int[] doorsArray, int wallIndex, List<DoorClass> newList)
    {
        int numberOfDoors = doorsArray[wallIndex];
        for (int j = 0; j < doorsArray[wallIndex]; j++)
        {
            Vector3 endPoint;
            Vector3 doorPrefabOffset = Vector3.zero;
            Vector3 startPoint;
            switch (wallIndex)
            {
                case 0:
                    startPoint = pointA;
                    doorPrefabOffset = new Vector3(colliderSize.x/2 * doorPrefab.transform.localScale.x, 0f, 0f);
                    break;
                case 1:
                    startPoint = pointC;
                    doorPrefabOffset = new Vector3(0f, 0f, colliderSize.x / 2 * doorPrefab.transform.localScale.x);
                    break;
                case 2:
                    startPoint = pointD;
                    doorPrefabOffset = new Vector3(colliderSize.x / 2 * doorPrefab.transform.localScale.x, 0f, 0f);
                    break;
                case 3:
                    startPoint = pointD;
                    doorPrefabOffset = new Vector3(0f, 0f, colliderSize.x / 2 * doorPrefab.transform.localScale.x);
                    break;
                default:
                    startPoint = Vector3.zero;
                    break;
            }
            if (numberOfDoors >= 1)
            {
                if (numberOfDoors == doorsArray[wallIndex])
                {
                    endPoint = newList[j].DoorObject.transform.position - doorPrefabOffset;
                    if (Vector3.Distance(startPoint, endPoint) >= colliderSize.x/2)
                    {
                        GenerateWall(startPoint, endPoint, Color.blue);
                    }

                }
                else
                {
                    startPoint = newList[j - 1].DoorObject.transform.position + doorPrefabOffset;
                    endPoint = newList[j].DoorObject.transform.position - doorPrefabOffset;

                    float distance = Vector3.Distance(newList[j - 1].DoorObject.transform.position, newList[j].DoorObject.transform.position);
                    if (distance >= 3)
                    {
                        GenerateWall(startPoint, endPoint, Color.green);
                    }
                }

                if (numberOfDoors == 1)
                {
                    startPoint = newList[j].DoorObject.transform.position + doorPrefabOffset;
                    endPoint = wallIndex switch
                    {
                        0 => pointB,
                        1 => pointB,
                        2 => pointC,
                        3 => pointA,
                        _ => Vector3.zero,
                    };
                    if (Vector3.Distance(startPoint, endPoint) >= colliderSize.x/2)
                    {
                        GenerateWall(startPoint, endPoint, Color.cyan);
                    }
                }
                numberOfDoors--;
            }
        }
    }

    private void DeleteOldDoors()
    {
        foreach(var item in doorClassList)
        {
            Destroy(item.DoorObject);
        }
        doorClassList.Clear();
    }

    private void DeleteOldWalls()
    {
        GameObject[] oldWalls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in oldWalls)
        {
            Destroy(wall);
        }
    }

    private void Respawn()
    {
        DeleteOldDoors();
        DeleteOldWalls();

        SpawnDoorsAndWalls();
    }
}
