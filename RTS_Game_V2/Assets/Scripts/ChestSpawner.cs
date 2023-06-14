using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [Header("Spawning Section")]
    [Tooltip("This represents a list of chests(Scriptable Objects) that are required to appear in game. " +
        "which are spawned automaticaly in area (Spawn Plane). ")]
    [SerializeField] private List<ChestSO> chestlist;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private GameObject spawnPlane;

    private MeshCollider mColl;
    private List<ChestClass> chestClassList = new();
    public class ChestClass
    {
        public ChestClass(int chestIndex, GameObject chestObject)
        {
            ChestIndex = chestIndex;
            ChestObject = chestObject;
        }
        public int ChestIndex { get; }
        public GameObject ChestObject { get; }
    }
    void Start()
    {
        mColl = spawnPlane.GetComponent<MeshCollider>();
        GameEvents.instance.OnPrepareGame += Respawn;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnPrepareGame -= Respawn;
    }

    private void SpawnChests()
    {
        if (chestPrefab != null)
        {
            for (int i = 0; i < chestlist.Count; i++)
            {
                GameObject newChest = Instantiate(chestPrefab);

                Chest chestScript = newChest.GetComponentInChildren<Chest>();
                ChestSO chest = chestlist[i];
                chestScript.SetChest(chest);

                Quaternion newRot = GetChestRotation();
                newChest.transform.rotation = newRot;
                
                Vector3 newPos = GetChestSpawnPosition();

                newChest.transform.position = newPos;

                ChestClass chestClass = new ChestClass(i, newChest);
                chestClassList.Add(chestClass);
            }
        }
    }

    private Vector3 GetChestSpawnPosition(int attempt = 1)
    { 
        if (spawnPlane != null)
        {
            float spawnPlaneSizeX = mColl.bounds.size.x;
            float spawnPlaneSizeZ = mColl.bounds.size.z;

            float spawnPosX = Random.Range(2, spawnPlaneSizeX / 2 - 2) * (Random.Range(0, 2) * 2 - 1);
            float spawnPosZ = Random.Range(2, spawnPlaneSizeZ / 2 - 2) * (Random.Range(0, 2) * 2 - 1);
            Vector3 spawnPoint = new(spawnPosX, spawnPlane.transform.position.y + 0.7f, spawnPosZ);

            int thisAttempt = attempt;
            if(thisAttempt < 10)
            {
                Collider[] colliders = Physics.OverlapSphere(spawnPoint, 0f);

                if (colliders.Length > 0)
                {
                    spawnPoint = GetChestSpawnPosition(thisAttempt + 1);
                }
                return spawnPoint;
            }
            return Vector3.zero;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Quaternion GetChestRotation()
    {
        return Quaternion.Euler(0f, Random.Range(0, 360), 0f);
    }                                                                                                                                                                                                                                                                                                                                                           

    private void Respawn()
    {
        DeleteOldChests();
        SpawnChests();
    }

    private void DeleteOldChests()
    {
        foreach (var item in chestClassList)
        {
            Destroy(item.ChestObject);
        }
        chestClassList.Clear();
    }
}
