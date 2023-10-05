using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class SpawnWalls : MonoBehaviour
{

    private List<DoorClass> doorClassList = new();
    private List<DoorSO> doorsList;
    private GameObject doorPrefab;

    private BoxCollider doorCollider;
    private RoomSO roomSO;

    private Vector3 pointA, pointAx, pointAz;
    private Vector3 pointB, pointBx, pointBz; 
    private Vector3 pointC, pointCx, pointCz; 
    private Vector3 pointD, pointDx, pointDz;

    private Vector3 colliderSize;
    private Vector3 colliderCenter;

    private bool generateDoorRandomly = true;

    private int[] doorsArray;
    private GameObject wallPrefab;

    public void SetEssentials(RoomSO roomSO, int[] doorsArray)
    {
        wallPrefab = Resources.Load("WallPrefab") as GameObject;
        this.doorPrefab = roomSO.DoorPrefab();
        this.doorsArray = doorsArray;
        this.doorsList = roomSO.RoomDoors();
        this.roomSO = roomSO;

        doorCollider = doorPrefab.GetComponent<BoxCollider>();
        colliderSize = doorCollider.size;
        colliderCenter = doorCollider.center;
        pointA = new Vector3(-1f * gameObject.transform.localScale.x, 0f, 1f * gameObject.transform.localScale.z);
        pointB = new Vector3(1f * gameObject.transform.localScale.x, 0f, 1f * gameObject.transform.localScale.z);
        pointC = new Vector3(1f * gameObject.transform.localScale.x, 0f, -1f * gameObject.transform.localScale.z);
        pointD = new Vector3(-1f * gameObject.transform.localScale.x, 0f, -1f * gameObject.transform.localScale.z);

        pointAx = pointA + new Vector3(colliderSize.x, 0f, 0f);
        pointBx = pointB - new Vector3(colliderSize.x, 0f, 0f);
        pointCx = pointC - new Vector3(colliderSize.x, 0f, 0f);
        pointDx = pointD + new Vector3(colliderSize.x, 0f, 0f);

        pointAz = pointA - new Vector3(0f, 0f, colliderSize.x);
        pointBz = pointB - new Vector3(0f, 0f, colliderSize.x);
        pointCz = pointC + new Vector3(0f, 0f, colliderSize.x);
        pointDz = pointD + new Vector3(0f, 0f, colliderSize.x);

        SpawnDoorsAndWalls();
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
        int doorIterator = 0;
        for (int i = 0; i < doorsArray.Length; i++)
        {
            var exclude = new HashSet<int>() { };
            for (int j = 0; j < doorsArray[i]; j++)
            {
                if(doorsArray[i] > 0 && doorsList.Count() == 0)
                {
                    continue;
                }
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;
                float positionInWall = j;

                if (!generateDoorRandomly)
                {
                    switch (i)
                    {
                        case 0:
                            pos = new Vector3(pointA.x / (doorsArray[0] + 1) * (j + 1), 0f, pointA.z);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                            positionInWall = pos.x;
                            break;
                        case 1:
                            pos = new Vector3(pointB.x, 0f, pointB.z / (doorsArray[1] + 1) * (j + 1));
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 90f));
                            positionInWall = pos.z;
                            break;
                        case 2:
                            pos = new Vector3(pointC.x / (doorsArray[2] + 1) * (j + 1), 0f, pointC.z);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 180f));
                            positionInWall = pos.x;
                            break;
                        case 3:
                            pos = new Vector3(pointD.x, 0f, pointD.z / (doorsArray[3] + 1) * (j + 1));
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

                            pos = new Vector3(positionInCurrentWall, 0f, pointA.z);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                            positionInWall = positionInCurrentWall;
                            break;
                        case 1:
                            positionInCurrentWall = GetAxisPos((int)pointCz.z, (int)pointBz.z, exclude);
                            exclude.Add(positionInCurrentWall);

                            pos = new Vector3(pointB.x, 0f, positionInCurrentWall);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 90f));
                            positionInWall = positionInCurrentWall;
                            break;
                        case 2:
                            positionInCurrentWall = GetAxisPos((int)pointCx.x, (int)pointDx.x, exclude);
                            exclude.Add(positionInCurrentWall);

                            pos = new Vector3(positionInCurrentWall, 0f, pointC.z);
                            rot = Quaternion.Euler(new Vector3(-90f, 0f, 180f));
                            positionInWall = positionInCurrentWall;
                            break;
                        case 3:
                            positionInCurrentWall = GetAxisPos((int)pointDz.z, (int)pointAz.z, exclude);
                            exclude.Add(positionInCurrentWall);

                            pos = new Vector3(pointD.x, 0f, positionInCurrentWall);
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

                GameObject door = Instantiate(doorPrefab);
                door.transform.SetParent(gameObject.transform);
                door.transform.SetLocalPositionAndRotation(pos, rot);
                Door doorScript = door.GetComponentInChildren<Door>();

                doorScript.SetDoor(doorsList[Random.Range(0, doorsList.Count)]);

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
            if (doorsArray[i] > 0)
            {
                MultipleDoors(doorsArray, i, currentWallDoors);
            }
        }
    }

    private void GenerateWall(Vector3 startPoint, Vector3 endPoint, Material wallMaterial)
    {
        if(doorCollider!= null)
        {
            //GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if(wallPrefab != null)
            {
                GameObject wall = Instantiate(wallPrefab);
                Vector3 distanceStartEnd = startPoint - endPoint;

                wall.transform.SetParent(gameObject.transform);
                wall.GetComponent<MeshRenderer>().material = wallMaterial;
                wall.tag = "Wall";
                wall.transform.localScale = new Vector3(colliderSize.y /* * doorPrefab.transform.lossyScale.x * 2*/, colliderSize.z / gameObject.transform.lossyScale.z * doorPrefab.transform.lossyScale.z, distanceStartEnd.magnitude);
                wall.transform.localPosition = (startPoint + endPoint) / 2f + new Vector3(0f, colliderSize.z / 2 / gameObject.transform.lossyScale.z * doorPrefab.transform.lossyScale.z, 0f);

                Vector3 lookAtThis = gameObject.transform.position + new Vector3(endPoint.x * gameObject.transform.lossyScale.x, colliderCenter.z * doorPrefab.transform.lossyScale.z, endPoint.z * gameObject.transform.lossyScale.z);
                wall.transform.LookAt(lookAtThis);
            }
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
        GenerateWall(startPoint, endPoint, roomSO.RoomWallMaterial());
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
                    doorPrefabOffset = new Vector3(colliderSize.x/2/gameObject.transform.lossyScale.x * doorPrefab.transform.lossyScale.x , 0f, 0f);
                    break;
                case 1:
                    startPoint = pointC;
                    doorPrefabOffset = new Vector3(0f, 0f, colliderSize.x / 2 / gameObject.transform.lossyScale.x * doorPrefab.transform.lossyScale.x);
                    break;
                case 2:
                    startPoint = pointD;
                    doorPrefabOffset = new Vector3(colliderSize.x / 2 / gameObject.transform.lossyScale.x * doorPrefab.transform.lossyScale.x, 0f, 0f);
                    break;
                case 3:
                    startPoint = pointD;
                    doorPrefabOffset = new Vector3(0f, 0f, colliderSize.x / 2 / gameObject.transform.lossyScale.x * doorPrefab.transform.lossyScale.x);
                    break;
                default:
                    startPoint = Vector3.zero;
                    break;
            }
            if (numberOfDoors >= 1)
            {
                if (numberOfDoors == doorsArray[wallIndex])
                {
                    endPoint = newList[j].DoorObject.transform.localPosition - doorPrefabOffset;
                    if (Vector3.Distance(startPoint, endPoint) >= colliderSize.x/2)
                    {
                        GenerateWall(startPoint, endPoint, roomSO.RoomWallMaterial());
                    }

                }
                else
                {
                    startPoint = newList[j - 1].DoorObject.transform.localPosition + doorPrefabOffset;
                    endPoint = newList[j].DoorObject.transform.localPosition - doorPrefabOffset;

                    float distance = Vector3.Distance(newList[j - 1].DoorObject.transform.localPosition, newList[j].DoorObject.transform.localPosition);
                    if (distance >= colliderSize.x / 2 / gameObject.transform.lossyScale.x * doorPrefab.transform.lossyScale.x)
                    {
                        GenerateWall(startPoint, endPoint, roomSO.RoomWallMaterial());
                    }
                }

                if (numberOfDoors == 1)
                {
                    startPoint = newList[j].DoorObject.transform.localPosition + doorPrefabOffset;
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
                        GenerateWall(startPoint, endPoint, roomSO.RoomWallMaterial());
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
