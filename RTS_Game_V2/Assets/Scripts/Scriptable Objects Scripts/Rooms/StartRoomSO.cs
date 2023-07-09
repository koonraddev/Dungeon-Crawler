using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStartRoom", menuName = "Scriptable Objects/Room/Start Room", order = 1)]
public class StartRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private Sprite roomWallTexture;
    [Header("Door section")]
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<DoorSO> doorsList;

    private int maxDoorsInWall = 1;

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

        objMaterial.color = Color.yellow;
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
