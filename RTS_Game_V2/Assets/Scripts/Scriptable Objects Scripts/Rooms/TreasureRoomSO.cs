using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoomSO : RoomSO
{
    public override int MaxDoorsInWall => throw new System.NotImplementedException();

    public override List<DoorSO> RoomDoors => throw new System.NotImplementedException();

    public override GameObject RoomPlane => throw new System.NotImplementedException();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.TREASURE);
    }
}
