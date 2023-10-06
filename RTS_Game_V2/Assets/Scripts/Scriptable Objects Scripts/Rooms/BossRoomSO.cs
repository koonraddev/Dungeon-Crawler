using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBossRoom", menuName = "Scriptable Objects/Room/Boss Room")]
public class BossRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private Material roomWallMaterial;
    [SerializeField] private Material roomFloorMaterial;
    [Header("Door section")]
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<DoorSO> doorsList;

    private int maxDoorsInWall = 0;
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

        if (isLastRoom)
        {
            GameEvents.instance.LastRoomReady();
        }
    }

    public override List<DoorSO> RoomDoors()
    {
        return doorsList;
    }

    public override Material RoomFloorMaterial()
    {
        return roomFloorMaterial;
    }

    public override GameObject RoomPlane()
    {
        return roomPlane;
    }

    public override Material RoomWallMaterial()
    {
        return roomWallMaterial;
    }
}
