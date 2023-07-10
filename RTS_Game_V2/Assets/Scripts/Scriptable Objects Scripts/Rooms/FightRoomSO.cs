using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newFightRoom", menuName = "Scriptable Objects/Room/Fight Room", order = 4)]
public class FightRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private Material roomWallMaterial;
    [SerializeField] private Material roomFloorMaterial;
    [Header("Door section")]
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<DoorSO> doorsList;
    [SerializeField] private int maxDoorsInWall;
    [Header("Enemy section")]
    public string enemyDescription;
    //do uzupelnienia


    public override GameObject DoorPrefab()
    {
        return doorPrefab;
    }

    public override int MaxDoorsInWall()
    {
        return maxDoorsInWall;
    }

    public override Material RoomFloorMaterial()
    {
        return roomFloorMaterial;
    }

    public override void RoomBehavoiur(GameObject gameObject)
    {
        gameObject.GetComponent<Renderer>().material = roomFloorMaterial;

        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newCube.transform.position = gameObject.transform.position;
        newCube.name = enemyDescription;
        newCube.transform.SetParent(gameObject.transform);
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
}
