using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PortalSpawner : MonoBehaviour
{
    private List<DoorClass> doorClassList = new();
    private List<DoorSO> doorsList;
    private GameObject doorPrefab;
    private BoxCollider doorCollider;


    private Vector3 colliderSize;
    private Vector3 colliderCenter;
    private RoomSO roomSO;

    private int[] doorsArray;

    private Vector3 pointA, pointAx, pointAz;
    private Vector3 pointB, pointBx, pointBz;
    private Vector3 pointC, pointCx, pointCz;
    private Vector3 pointD, pointDx, pointDz;

    bool firstRoom;
    private void Awake()
    {
        doorPrefab = Resources.Load("Teleport") as GameObject;
    }
    public void SetEssentials(RoomSO roomSO, int[] doorsArray, bool firstRoom)
    {
        this.doorsArray = doorsArray;
        this.doorsList = roomSO.RoomDoors;
        this.roomSO = roomSO;
        this.firstRoom = firstRoom;

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

        SpawnPortals();
    }
    void Start()
    {
        
    }

    void SpawnPortals()
    {
        int doorIterator = 0;
        for (int i = 0; i < doorsArray.Length; i++)
        {
            var exclude = new HashSet<int>() { };
            for (int j = 0; j < doorsArray[i]; j++)
            {
                if (doorsArray[i] > 0 && doorsList.Count() == 0)
                {
                    continue;
                }
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;
                float positionInWall = j;

                int positionInCurrentWall = 0;
                switch (i)
                {
                    case 0:
                        positionInCurrentWall = GetAxisPos((int)pointAx.x, (int)pointBx.x, exclude);
                        exclude.Add(positionInCurrentWall);
                
                        pos = new Vector3(positionInCurrentWall, 0f, pointA.z);
                        rot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        positionInWall = positionInCurrentWall;
                        break;
                    case 1:
                        positionInCurrentWall = GetAxisPos((int)pointCz.z, (int)pointBz.z, exclude);
                        exclude.Add(positionInCurrentWall);
                
                        pos = new Vector3(pointB.x, 0f, positionInCurrentWall);
                        rot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                        positionInWall = positionInCurrentWall;
                        break;
                    case 2:
                        positionInCurrentWall = GetAxisPos((int)pointCx.x, (int)pointDx.x, exclude);
                        exclude.Add(positionInCurrentWall);
                
                        pos = new Vector3(positionInCurrentWall, 0f, pointC.z);
                        rot = Quaternion.Euler(new Vector3(0f, 180, 0f));
                        positionInWall = positionInCurrentWall;
                        break;
                    case 3:
                        positionInCurrentWall = GetAxisPos((int)pointDz.z, (int)pointAz.z, exclude);
                        exclude.Add(positionInCurrentWall);
                
                        pos = new Vector3(pointD.x, 0f, positionInCurrentWall);
                        rot = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                        positionInWall = positionInCurrentWall;
                        break;
                    default:
                        positionInWall = j;
                        pos = new Vector3(0f, 0f, 0f);
                        rot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        break;
                }
                

                GameObject door = Instantiate(doorPrefab);
                door.transform.SetParent(gameObject.transform);
                door.transform.SetLocalPositionAndRotation(pos, rot);

                door.transform.SetParent(null);
                Door doorScript = door.GetComponentInChildren<Door>();
                RoomFader roomFade = door.GetComponent<RoomFader>();
                roomFade.ParentObject = gameObject;
                roomFade.FirstRoom = firstRoom;
                doorScript.SetDoor(doorsList[Random.Range(0, doorsList.Count)]);

                GameController.instance.spawnedPortals.Add(doorScript.GameObject);
                DoorClass doorClass = new DoorClass(i, positionInWall, door);
                doorClassList.Add(doorClass);

                doorIterator++;

            }
        }
    }

    private int GetAxisPos(int minValue, int maxValue, HashSet<int> excludeHashSet = null, int attempt = 1)
    {
        if (maxValue <= minValue)
        {
            return 0;
        }
        int count = Mathf.Abs(minValue - maxValue + 1);

        var exclude = excludeHashSet;
        var rand = new System.Random();
        var range = Enumerable.Range(minValue, count).Where(i => !exclude.Contains(i));
        int index = rand.Next(0, range.Count() - exclude.Count());

        int newValue = range.ElementAt(index);

        if (attempt <= 10)
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

}
