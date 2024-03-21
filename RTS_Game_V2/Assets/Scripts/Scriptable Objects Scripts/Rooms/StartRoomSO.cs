using UnityEngine;
using Unity.AI.Navigation;

[CreateAssetMenu(fileName = "newStartRoom", menuName = "Scriptable Objects/Room/Start Room")]
public class StartRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [Header("Door section")]
    [SerializeField] private PortalSO portalSO;

    public override PortalSO Portal => portalSO;

    public override GameObject Plane => roomPlane;

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        NavMeshSurface surface = roomPlane.GetComponent<NavMeshSurface>();
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.START);
    }
}
