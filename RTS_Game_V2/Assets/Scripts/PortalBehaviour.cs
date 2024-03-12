using UnityEngine;

[System.Serializable]
public class PortalBehaviour
{
    private PlayerMovement playerObject;

    private GameObject parentRoom, destinationRoom;
    private Transform  teleportDestination;

    public PlayerMovement PlayerMovement { set => playerObject = value; }
    public PortalBehaviour(GameObject parentRoom)
    {
        this.parentRoom = parentRoom;
    }


    public void SetDestinationStuff(GameObject destinationRoom, Transform destinationTeleportTransform)
    {
        this.destinationRoom = destinationRoom;
        teleportDestination = destinationTeleportTransform;
    }


    public void Teleport()
    {
        destinationRoom.SetActive(true);
        MapManager.instance.ActiveRoom(destinationRoom);
        playerObject.TeleportTo(teleportDestination.position);

        MapManager.instance.DeactiveRoom(parentRoom);
        parentRoom.SetActive(false);
    }
}
