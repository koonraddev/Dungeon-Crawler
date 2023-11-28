using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

[CreateAssetMenu(fileName = "newStartRoom", menuName = "Scriptable Objects/Room/Start Room")]
public class StartRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [Header("Door section")]
    [SerializeField] private List<DoorSO> doorsList;

    private int maxDoorsInWall = 1;

    public override int MaxDoorsInWall => maxDoorsInWall;

    public override List<DoorSO> RoomDoors => doorsList;

    public override GameObject RoomPlane => roomPlane;

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        //roomGameObject.GetComponent<Renderer>().material = roomFloorMaterial;
        NavMeshSurface surface = roomPlane.GetComponent<NavMeshSurface>();
        NavigationController.instance.AddNavSurface(surface);
    }
}
