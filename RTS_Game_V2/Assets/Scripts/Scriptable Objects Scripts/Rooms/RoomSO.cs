using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomSO: ScriptableObject
{
    public abstract int MaxDoorsInWall { get; }
    public abstract List<DoorSO> RoomDoors { get; }
    public abstract GameObject RoomPlane { get; }
    public abstract void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false);
}
