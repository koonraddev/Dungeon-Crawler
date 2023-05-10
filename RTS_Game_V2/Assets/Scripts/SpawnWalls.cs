using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnWalls : MonoBehaviour
{
    private List<DoorClass> doorClassList = new();

    [SerializeField] private List<DoorSO> doorslist;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private Collider doorCollider;
    [SerializeField] private Mesh doorMesh;
    [SerializeField] private GameObject plane;

    private Vector3 planeSize;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 pointC;
    private Vector3 pointD;

    [SerializeField] private int wallA;
    [SerializeField] private int wallB;
    [SerializeField] private int wallC;
    [SerializeField] private int wallD;
    public class DoorClass
    {
        public DoorClass(int wallIndex, GameObject doorObject)
        {
            this.WallIndex = wallIndex;
            this.DoorObject = doorObject;
        }
        public int WallIndex { get; }
        public GameObject DoorObject { get; }
    }
    

    void Start()
    {
        planeSize = plane.GetComponent<Collider>().bounds.size;

        pointA = new Vector3(-planeSize.x / 2, 0f, planeSize.z / 2);
        pointB = new Vector3(planeSize.x / 2, 0f, planeSize.z / 2);
        pointC = new Vector3(planeSize.x / 2, 0f, -planeSize.z / 2);
        pointD = new Vector3(-planeSize.x / 2, 0f, -planeSize.z / 2);

        GameObject cornerA = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        GameObject cornerB = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        GameObject cornerC = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        GameObject cornerD = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        cornerA.GetComponent<MeshRenderer>().material.color = Color.red;
        cornerB.GetComponent<MeshRenderer>().material.color = Color.blue;
        cornerC.GetComponent<MeshRenderer>().material.color = Color.green;
        cornerD.GetComponent<MeshRenderer>().material.color = Color.yellow;

        cornerA.transform.position = pointA;
        cornerB.transform.position = pointB;
        cornerC.transform.position = pointC;
        cornerD.transform.position = pointD;

        int[] doorsArray = { wallA, wallB, wallC, wallD };

        for (int i = 0; i < doorsArray.Length; i++)
        {
            for (int j = 0; j < doorsArray[i]; j++)
            {
                float spawnPosZ = Random.Range(2, planeSize.z / 2 - 2) * (Random.Range(0, 2) * 2 - 1);
                float spawnPosX = Random.Range(2, planeSize.x / 2 - 2) * (Random.Range(0, 2) * 2 - 1);
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;
                switch (i)
                {
                    case 0:
                        pos = new Vector3(pointA.x + planeSize.x / (wallA+1)*(j+1) , 0f, planeSize.z / 2);
                        rot = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                        break;
                    case 1:
                        pos = new Vector3(planeSize.x / 2, 0f, pointB.z - planeSize.z / (wallB + 1) * (j + 1));
                        rot = Quaternion.Euler(new Vector3(-90f, 0f, 90f));
                        break;
                    case 2:
                        pos = new Vector3(pointC.x - planeSize.x / (wallC + 1) * (j + 1), -planeSize.z / 2);
                        rot = Quaternion.Euler(new Vector3(-90f, 0f, 180f));
                        break;
                    case 3:
                        pos = new Vector3(-planeSize.x / 2, 0f, pointD.z + planeSize.z / (wallD + 1) * (j + 1));
                        rot = Quaternion.Euler(new Vector3(-90f, 0f, 270f));
                        break;
                    default:
                        break;
                }

                GameObject door = Instantiate(doorPrefab, pos, rot);
                Door doorScript = door.GetComponentInChildren<Door>();

                doorScript.SetDoor(doorslist[Random.Range(0, doorslist.Count)]);
                DoorClass doorClass = new DoorClass(i, door);
                doorClassList.Add(doorClass);
            }
        }

        for (int i = 0; i < doorsArray.Length; i++)
        {
            List<DoorClass> newList = doorClassList.Where(door => door.WallIndex == i).ToList();
            int numberOfDoors = doorsArray[i];

            switch (numberOfDoors)
            {
                case 0:
                    ZeroDors(i);
                    break;
                case 1:
                    DoorClass oneDoor = newList[0];
                    OneDoor(i, oneDoor.DoorObject);
                    break;
                default:
                    MultipleDoors(doorsArray, i, numberOfDoors, newList);
                    break;
            }
        }
    }


    private void ZeroDors(int wallIndex)
    {
        Vector3 startPoint = Vector3.zero;
        Vector3 endPoint = Vector3.zero;
        Vector3 distanceStartEnd = Vector3.zero;
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
                break;

        }
        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        distanceStartEnd = startPoint - endPoint;
        cube1.GetComponent<MeshRenderer>().material.color = Color.yellow;
        cube1.transform.localScale = new Vector3(0.5f, doorMesh.bounds.size.y * 2, distanceStartEnd.magnitude);
        cube1.transform.position = (startPoint + endPoint) / 2f + new Vector3(0f, doorMesh.bounds.center.y, 0f);
        cube1.transform.LookAt(endPoint + new Vector3(0f, doorMesh.bounds.center.y, 0f));
    }

    private void OneDoor(int wallIndex, GameObject oneDoor)
    {
        Vector3 endPoint = Vector3.zero;
        Vector3 distanceStartEnd = Vector3.zero;
        Vector3 doorPrefabOffset = Vector3.zero;
        Vector3 startPoint = Vector3.zero;
        switch (wallIndex)
        {
            case 0:
                startPoint = pointA;
                endPoint = pointB;
                doorPrefabOffset = new Vector3(doorMesh.bounds.size.x, 0f, 0f);
                break;
            case 1:
                startPoint = pointB;
                endPoint = pointC;
                doorPrefabOffset = new Vector3(0f, 0f, doorMesh.bounds.size.x);
                break;
            case 2:
                startPoint = pointC;
                endPoint = pointD;
                doorPrefabOffset = new Vector3(doorMesh.bounds.size.x, 0f, 0f);
                break;
            case 3:
                startPoint = pointD;
                endPoint = pointA;
                doorPrefabOffset = new Vector3(0f, 0f, doorMesh.bounds.size.x);
                break;
            default:
                break;

        }
        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        endPoint = oneDoor.transform.position + doorPrefabOffset;
        distanceStartEnd = startPoint - endPoint;

        cube1.GetComponent<MeshRenderer>().material.color = Color.red;
        cube1.transform.localScale = new Vector3(0.5f, doorMesh.bounds.size.y*2, distanceStartEnd.magnitude);
        cube1.transform.position = (startPoint + endPoint) / 2f + new Vector3(0f, doorMesh.bounds.center.y,0f);
        cube1.transform.LookAt(endPoint + new Vector3(0f, doorMesh.bounds.center.y, 0f));

        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startPoint = oneDoor.transform.position - doorPrefabOffset;
        switch (wallIndex)
        {
            case 0:
                endPoint = pointB;
                break;
            case 1:
                endPoint = pointC;
                break;
            case 2:
                endPoint = pointD;
                break;
            case 3:
                endPoint = pointA;
                break;
            default:
                break;
        }
        distanceStartEnd = startPoint - endPoint;

        cube2.GetComponent<MeshRenderer>().material.color = Color.magenta;
        cube2.transform.localScale = new Vector3(0.5f, doorMesh.bounds.size.y * 2, distanceStartEnd.magnitude);
        cube2.transform.position = (startPoint + endPoint) / 2f + new Vector3(0f, doorMesh.bounds.center.y, 0f);
        cube2.transform.LookAt(endPoint + new Vector3(0f, doorMesh.bounds.center.y, 0f));
    }

    private void MultipleDoors(int[] doorsArray, int wallIndex, int numberOfDoors, List<DoorClass> newList)
    {
        for (int j = 0; j < doorsArray[wallIndex]; j++)
        {
            GameObject cube1A = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 endPoint = Vector3.zero;
            Vector3 distanceStartEnd = Vector3.zero;
            Vector3 doorPrefabOffset = Vector3.zero;
            Vector3 startPoint = Vector3.zero;
            switch (wallIndex)
            {
                case 0:
                    startPoint = pointA;
                    endPoint = pointB;
                    doorPrefabOffset = new Vector3(doorMesh.bounds.size.x, 0f, 0f);
                    break;
                case 1:
                    startPoint = pointB;
                    endPoint = pointC;
                    doorPrefabOffset = new Vector3(0f, 0f, doorMesh.bounds.size.x);
                    break;
                case 2:
                    startPoint = pointC;
                    endPoint = pointD;
                    doorPrefabOffset = new Vector3(doorMesh.bounds.size.x, 0f, 0f);
                    break;
                case 3:
                    startPoint = pointD;
                    endPoint = pointA;
                    doorPrefabOffset = new Vector3(0f, 0f, doorMesh.bounds.size.x);
                    break;
                default:
                    break;
            }
            if (numberOfDoors >= 1)
            {

                if (numberOfDoors == doorsArray[wallIndex])
                {
                    endPoint = newList[j].DoorObject.transform.position - doorPrefabOffset;
                    distanceStartEnd = startPoint - endPoint;
                    cube1A.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    startPoint = newList[j - 1].DoorObject.transform.position + doorPrefabOffset;
                    endPoint = newList[j].DoorObject.transform.position - doorPrefabOffset;
                    distanceStartEnd = startPoint - endPoint;
                    cube1A.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                cube1A.transform.localScale = new Vector3(0.5f, doorMesh.bounds.size.y * 2, distanceStartEnd.magnitude);
                cube1A.transform.position = (startPoint + endPoint) / 2f + new Vector3(0f, doorMesh.bounds.center.y, 0f);
                cube1A.transform.LookAt(endPoint + new Vector3(0f, doorMesh.bounds.center.y, 0f));

                if (numberOfDoors == 1)
                {
                    GameObject cube1B = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    startPoint = newList[j].DoorObject.transform.position + doorPrefabOffset;
                    switch (wallIndex)
                    {
                        case 0:
                            endPoint = pointB;
                            break;
                        case 1:
                            endPoint = pointC;
                            break;
                        case 2:
                            endPoint = pointD;
                            break;
                        case 3:
                            endPoint = pointA;
                            break;
                        default:
                            break;
                    }
                    distanceStartEnd = startPoint - endPoint;
                    cube1B.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    cube1B.transform.localScale = new Vector3(0.5f, doorMesh.bounds.size.y * 2, distanceStartEnd.magnitude);
                    cube1B.transform.position = (startPoint + endPoint) / 2f + new Vector3(0f, doorMesh.bounds.center.y, 0f);
                    cube1B.transform.LookAt(endPoint + new Vector3(0f, doorMesh.bounds.center.y, 0f));
                }
                numberOfDoors--;
            }
        }
    }
}
