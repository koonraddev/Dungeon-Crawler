using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawning Section")]
    [Tooltip("This represents a list of doors(Scriptable Objects) that are required to appear in game. " +
        "which are spawned automaticaly in area (Spawn Plane). ")]
    [SerializeField] private List<DoorSO> doorslist;
    [Tooltip("This represents a list of chests(Scriptable Objects) that are required to appear in game. " +
        "which are spawned automaticaly in area (Spawn Plane). ")]
    [SerializeField] private List<ChestSO> chestlist;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private GameObject spawnPlane;

    void Start()
    {
        SpawnDoorsAndChests();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    private void SpawnDoorsAndChests()
    {
        int i = chestlist.Count;
        int d = doorslist.Count;
        if (chestPrefab != null)
        {
            foreach (ChestSO chest in chestlist)
            {
                if (chest != null)
                {
                    GameObject newChest = Instantiate(chestPrefab) as GameObject;
                    newChest.transform.SetPositionAndRotation(GetChestSpawnPosition(), GetChestRotation());
                    Chest chestScript = newChest.GetComponentInChildren<Chest>();
                    chestScript.SetChest(chest);
                }
                i--;
            }
        }

        if (doorPrefab != null)
        {
            foreach (DoorSO door in doorslist)
            {
                if (door != null)
                {
                    GameObject newDoor = Instantiate(doorPrefab) as GameObject;
                    newDoor.transform.position = new Vector3(-d, 1, i);
                    Door doorScript = newDoor.GetComponentInChildren<Door>();
                    doorScript.SetDoor(door);
                }
                d--;
            }
        }
    }

    private Vector3 GetChestSpawnPosition()
    {
        
        if (spawnPlane != null)
        {
            MeshCollider mColl = spawnPlane.GetComponent<MeshCollider>();
            float spawnPlaneSizeX = mColl.bounds.size.x;
            float spawnPlaneSizeZ = mColl.bounds.size.z;

            float spawnPosX = Random.Range(2, spawnPlaneSizeX / 2 - 2) * (Random.Range(0, 2) * 2 - 1);
            float spawnPosZ = Random.Range(2, spawnPlaneSizeZ / 2 - 2) * (Random.Range(0, 2) * 2 - 1);

            int layerMask = chestPrefab.layer;
            LayerMask mask = 1 << layerMask;
            float chestCollRadius = chestPrefab.GetComponentInChildren<MeshCollider>().bounds.max.x;
            Vector3 spawnPoint = new(spawnPosX, 0.7f, spawnPosZ);
            //Debug.Log("Spawn: " + spawnPoint);
            //while (Physics.CheckSphere(spawnPoint, chestCollRadius, mask)) // do poprawy
            //{
            //    int czas = 3;
            //    //spawnPosX = Random.Range(2, spawnPlaneSizeX / 2 - 2) * (Random.Range(0, 2) * 2 - 1);
            //    //spawnPosZ = Random.Range(2, spawnPlaneSizeZ / 2 - 2) * (Random.Range(0, 2) * 2 - 1);
            //    //spawnPoint = new Vector3(spawnPosX, 0.7f, spawnPosZ);
            //    Debug.Log("Nowy spawn: " + spawnPoint);
            //}

            while (Physics.OverlapSphere(spawnPoint, chestCollRadius/2, mask).Length > 0) //nie dziala jak trzeba
            {
                //Debug.Log("koliduje");
                spawnPosX = Random.Range(1, spawnPlaneSizeX / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
                spawnPosZ = Random.Range(1, spawnPlaneSizeZ / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
                spawnPoint = new Vector3(spawnPosX, 0.7f, spawnPosZ);
                //Debug.Log("Nowy spawn: " + spawnPoint);

            }
            //Debug.Log("Finalny spawn: " + spawnPoint);
            return spawnPoint;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Quaternion GetChestRotation()
    {
        float spawnRotY = Random.Range(0, 360);
        return Quaternion.Euler(0f, spawnRotY, 0f);
    }

    private void Respawn()
    {
        DeleteOldChests();
        DeleteOldDoors();
        SpawnDoorsAndChests();
    }

    private void DeleteOldDoors()
    {
        GameObject[] oldDoors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in oldDoors)
        {
            Destroy(door);
        }
    }
    private void DeleteOldChests()
    {
        GameObject[] oldChests = GameObject.FindGameObjectsWithTag("Chest");

        foreach(GameObject chest in oldChests)
        {
            Destroy(chest);
        }
    }
}
