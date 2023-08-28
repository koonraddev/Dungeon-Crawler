using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

[CreateAssetMenu(fileName = "newStartRoom", menuName = "Scriptable Objects/Room/Start Room", order = 1)]
public class StartRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private Material roomWallMaterial;
    [SerializeField] private Material roomFloorMaterial;
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

    public override Material RoomFloorMaterial()
    {
        return roomFloorMaterial;
    }

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        roomGameObject.GetComponent<Renderer>().material = roomFloorMaterial;
        NavMeshSurface surface = roomPlane.GetComponent<NavMeshSurface>();
        NavigationController.instance.AddNavSurface(surface);

        if (isLastRoom)
        {
            GameEvents.instance.LastRoomReady();
        }
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
