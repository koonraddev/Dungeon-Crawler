using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    public static RoomsGenerator instance;
    System.Random rand;

    public int RoomsToGenerate 
    { 
        get=>roomsToGenerate; 
        set 
        {
            int range = (value - 1) / 5;
            roomsToGenerate = (range + 1) * 5;
        } 
    }
    private int roomsToGenerate;
    private bool firstRoom = true;
    private List<RoomSO> roomsList;
    private RoomSO startRoom;
    private RoomSO bossRoom;
    private RoomsSetSO roomsSetSO;
    [SerializeField] private RoomsSetSO roomSet;


    private void Awake()
    {
        rand = new System.Random();
        instance = this;

        roomsSetSO = Instantiate(roomSet);
        startRoom = roomsSetSO.StartRoom;
        bossRoom = roomsSetSO.BossRoom;
        roomsList = roomsSetSO.RoomsList;
        roomsToGenerate = roomsSetSO.RoomsAmount;
    }

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
            if (roomsToGenerate == 1)
            {
                roomSo = bossRoom;
            }
            else
            {
                int index = rand.Next(0, roomsList.Count);
                roomSo = roomsList[index];
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
