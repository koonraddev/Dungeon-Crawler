using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChestRoom", menuName = "Scriptable Objects/Room/Chest Room", order = 5)]
public class ChestRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private Sprite roomWallTexture;
    [Header("Door section")]
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<DoorSO> doorsList;
    [Header("Chest section")]
    [SerializeField] private List<ChestSO> chestList;

    private int maxDoorsInWall = 0;
    public override GameObject DoorPrefab()
    {
        return doorPrefab;
    }

    public override int MaxDoorsInWall()
    {
        return maxDoorsInWall;
    }

    public override void RoomBehavoiur(GameObject gameObject)
    {
        Material objMaterial = gameObject.GetComponent<Renderer>().material;

        objMaterial.color = Color.cyan;
    }

    public override List<DoorSO> RoomDoors()
    {
        return doorsList;
    }

    public override GameObject RoomPlane()
    {
        return roomPlane;
    }

    public override Sprite RoomWallTexture()
    {
        return roomWallTexture;
    }
}
