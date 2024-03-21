using UnityEngine;

public class TreasureRoomSO : RoomSO
{
    public override PortalSO Portal => throw new System.NotImplementedException();

    public override GameObject Plane => throw new System.NotImplementedException();

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.TREASURE);
    }
}
