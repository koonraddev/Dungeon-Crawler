using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEmptyRoom", menuName = "Scriptable Objects/Room/Empty Room")]
public class EmptyRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [Header("Door section")]
    [SerializeField] private List<DoorSO> doorsList;
    [SerializeField] private int maxDoorsInWall;

    public override int MaxDoorsInWall => maxDoorsInWall;

    public override List<DoorSO> RoomDoors => doorsList;

    public override GameObject RoomPlane => roomPlane;

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        //roomGameObject.GetComponent<Renderer>().material = roomFloorMaterial;
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.EMPTY);
    }
}
