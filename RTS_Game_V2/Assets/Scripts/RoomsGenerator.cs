using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomsGenerator : MonoBehaviour
{
    public static RoomsGenerator instance;
    System.Random rand;
    [Min(2)]
    [SerializeField] private int roomsToGenerate;
    private bool firstRoom = true;
    private void Awake()
    {
        rand = new System.Random();
        instance = this;
    }

    [SerializeField] private List<RoomSO> roomsList;
    [SerializeField] private RoomSO startRoom;
    [SerializeField] private RoomSO bossRoom;

    public RoomSO GetRoom()
    {
        RoomSO roomSo;
        if (firstRoom)
        {
            roomSo = startRoom;
            firstRoom = false;
        }
        else
        {
            if(roomsToGenerate == 1)
            {
                roomSo = bossRoom;
            }
            else
            {
                roomSo = roomsList[rand.Next(0, roomsList.Count)];
            }
        }
        roomsToGenerate--;
        return roomSo;
    }


    public int GetRoomsLeft()
    {
        return roomsToGenerate;
    }
}
