using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRoomsSet", menuName = "Scriptable Objects/Rooms/Rooms Set")]
public class RoomsSetSO : ScriptableObject
{
    [SerializeField] private RoomSO startRoom;
    [SerializeField] private RoomSO bossRoom;
    [SerializeField] private List<RoomSO> roomsList;

    public RoomSO StartRoom { get => startRoom; }
    public RoomSO BossRoom { get => bossRoom; }
    public List<RoomSO> RoomsList { get => roomsList; }

    private void Awake()
    {
        roomsList = new();
        roomsList.Add(null);
    }

    private void OnValidate()
    {
        if(RoomsList.Count == 0)
        {
            roomsList.Add(null);
        }
    }
}
