using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    public static RoomsGenerator instance;
    System.Random rand;

    private void Awake()
    {
        rand = new System.Random();
        instance = this;
    }

    private int roomsToGenerate;
    private bool firstRoom = true;
    private List<RoomSO> roomsList;
    private RoomSO startRoom;
    private RoomSO bossRoom;
    private RoomsSetSO roomsSetSO;
    
    public void SetRoomsGenerator(RoomsSetSO roomsSetSO)
    {
        this.roomsSetSO = Instantiate(roomsSetSO);
        startRoom = this.roomsSetSO.StartRoom;
        bossRoom = this.roomsSetSO.BossRoom;
        roomsList = this.roomsSetSO.RoomsList;
        roomsToGenerate = this.roomsSetSO.RoomsAmount;
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
