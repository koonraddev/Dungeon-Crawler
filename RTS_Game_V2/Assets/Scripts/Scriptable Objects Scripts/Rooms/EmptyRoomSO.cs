using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEmptyRoom", menuName = "Scriptable Objects/Room/Empty Room")]
public class EmptyRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [Header("Door section")]
    [SerializeField] private PortalSO portalSO;

    public override PortalSO Portal => portalSO;

    public override GameObject Plane => roomPlane;

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        //roomGameObject.GetComponent<Renderer>().material = roomFloorMaterial;
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.EMPTY);
    }
}
