using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[CreateAssetMenu(fileName = "newChestRoom", menuName = "Scriptable Objects/Room/Chest Room")]
public class ChestRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private Material roomWallMaterial;
    [SerializeField] private Material roomFloorMaterial;
    [Header("Door section")]
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<DoorSO> doorsList;
    [Header("Chest section")]
    [SerializeField] private List<ChestSO> chestList;
    [SerializeField] private GameObject chestPrefab;

    private int maxDoorsInWall = 0;

    private MeshCollider mColl;
    private float spawnPlaneSizeX;
    private float spawnPlaneSizeZ;
    private Vector3 planePos;

    public override GameObject DoorPrefab()
    {
        return doorPrefab;
    }

    public override int MaxDoorsInWall()
    {
        return maxDoorsInWall;
    }

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        roomGameObject.GetComponent<Renderer>().material = roomFloorMaterial;

        mColl = roomGameObject.GetComponent<MeshCollider>();
        spawnPlaneSizeX = mColl.bounds.size.x;
        spawnPlaneSizeZ = mColl.bounds.size.z;
        planePos = roomGameObject.transform.position;

        foreach (var item in chestList)
        {
            GameObject newChest = Instantiate(chestPrefab);
            Chest chestScript = newChest.GetComponentInChildren<Chest>();
            var clone = Instantiate(item);
            chestScript.SetChest(clone);

            newChest.transform.SetPositionAndRotation(GetChestSpawnPosition(), GetChestSpawnRotation());
            newChest.transform.SetParent(roomGameObject.transform);
        }
        if (isLastRoom)
        {
            GameEvents.instance.LastRoomReady();
        }
    }

    private Vector3 GetChestSpawnPosition(int attempt = 1)
    {
        if (roomPlane != null)
        {
            float spawnPosX = Random.Range(1, spawnPlaneSizeX / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
            float spawnPosZ = Random.Range(1, spawnPlaneSizeZ / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
            Vector3 spawnPoint = new(spawnPosX, roomPlane.transform.position.y + 0.7f, spawnPosZ);
            spawnPoint += planePos;

            int thisAttempt = attempt;
            if (thisAttempt < 10)
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

    private Quaternion GetChestSpawnRotation()
    {
        return Quaternion.Euler(0f, Random.Range(0, 360), 0f);
    }

    public override List<DoorSO> RoomDoors()
    {
        return doorsList;
    }

    public override GameObject RoomPlane()
    {
        return roomPlane;
    }

    public override Material RoomWallMaterial()
    {
        return roomWallMaterial;
    }

    public override Material RoomFloorMaterial()
    {
        return roomFloorMaterial;
    }
}
