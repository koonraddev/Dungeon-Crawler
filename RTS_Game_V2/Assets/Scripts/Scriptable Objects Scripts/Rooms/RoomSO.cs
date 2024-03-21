using UnityEngine;

public abstract class RoomSO: ScriptableObject
{
    public abstract PortalSO Portal { get; }
    public abstract GameObject Plane { get; }
    public abstract void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false);
}
