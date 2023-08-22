using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomSO: ScriptableObject
{
    public abstract int MaxDoorsInWall();
    public abstract List<DoorSO> RoomDoors();
    public abstract GameObject DoorPrefab();
    public abstract GameObject RoomPlane();
    public abstract Material RoomWallMaterial();
    public abstract Material RoomFloorMaterial();
    public abstract void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false);
}
