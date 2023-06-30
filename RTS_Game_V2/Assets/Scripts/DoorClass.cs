using UnityEngine;

public class DoorClass
{
    public DoorClass(int wallIndex, float positionInWall, GameObject doorObject)
    {
        WallIndex = wallIndex;
        PositionInWall = positionInWall;
        DoorObject = doorObject;
    }
    public int WallIndex { get; }
    public GameObject DoorObject { get; }
    public float PositionInWall { get; }
}