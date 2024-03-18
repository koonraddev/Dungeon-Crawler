using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRoomsSet", menuName = "Scriptable Objects/Room/Rooms Set")]
public class RoomsSetSO : ScriptableObject
{
    [SerializeField] private StartRoomSO startRoom;
    [SerializeField] private BossRoomSO bossRoom;
    [SerializeField] private List<RoomSO> roomsList;
    public RoomSO StartRoom { get => startRoom; }
    public RoomSO BossRoom { get => bossRoom; }
    public List<RoomSO> RoomsList { get => roomsList; }
}
