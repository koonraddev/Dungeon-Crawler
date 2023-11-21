using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    public enum LevelRange
    {
        RANGE_1_5 = 5,
        RANGE_6_10 = 10,
        RANGE_11_15 = 15,
        RANGE_16_20 = 20,
        RANGE_21_25 = 25,
        RANGE_26_30 = 30,
        RANGE_31_35 = 35,
        RANGE_36_40 = 40,
        RANGE_41_45 = 45,
        RANGE_46_50 = 50,
        RANGE_51_55 = 55,
        RANGE_56_60 = 60,
        RANGE_61_65 = 65,
        RANGE_66_70 = 70,
        RANGE_71_75 = 75,
        RANGE_76_80 = 80,
        RANGE_81_85 = 85,
        RANGE_86_90 = 90,
        RANGE_91_95 = 95,
        RANGE_96_100 = 100
    }

    [System.Serializable]
    public class LevelsRangeSetiings
    {
        public LevelRange levelRange;
        public RoomsSetSO roomsSet;
    }

    [System.Serializable]
    public class LevelException
    {
        public int level;
        public RoomsSetSO roomsSet;
        public int roomsAmount;
    }

    public static RoomsGenerator instance;
    System.Random rand;
    Array levelRangeValues = Enum.GetValues(typeof(LevelRange));
    int arrayLength;

    private int roomsToGenerate;
    private bool firstRoom;
    private List<RoomSO> roomsList;
    private RoomSO startRoom;
    private RoomSO bossRoom;
    private RoomsSetSO roomsSetSO;
    [SerializeField] private RoomsSetSO defaultRoomsSet;
    [SerializeField] private int defaultRoomsAmount;
    [SerializeField] private List<LevelsRangeSetiings> levelRangeSetsList;
    [SerializeField] private List<LevelException> levelExcSetsList;
    public int RoomsToGenerate { get => roomsToGenerate; }

    private void Awake()
    {
        rand = new System.Random();
        instance = this;
        arrayLength = levelRangeValues.Length;
        //SetRoomsGenerator(1);
    }


    public void SetRoomsGenerator(int level)
    {
        firstRoom = true;
        int range = (level - 1) / 5;
        range = Mathf.Clamp(range, 0, arrayLength - 1);
        LevelRange lr = (LevelRange)levelRangeValues.GetValue(range);
        RoomsSetSO tmpSet = null;

        foreach (var item in levelExcSetsList)
        {
            if(item.level == level)
            {
                tmpSet = item.roomsSet;
                roomsToGenerate = item.roomsAmount;
                break;
            }
        }

        if(tmpSet == null)
        {
            foreach (var item in levelRangeSetsList)
            {
                if (item.levelRange == lr)
                {
                    tmpSet = item.roomsSet;
                    roomsToGenerate = (int)item.levelRange;
                    break;
                }
            }
        }

        if (tmpSet == null)
        {
            tmpSet = defaultRoomsSet;
            roomsToGenerate = defaultRoomsAmount;
        }

        roomsSetSO = Instantiate(tmpSet);

        startRoom = roomsSetSO.StartRoom;
        bossRoom = roomsSetSO.BossRoom;
        roomsList = roomsSetSO.RoomsList;
    }


    public RoomSO GetRoom()
    {
        //Debug.Log("ROOMS LEFT  = " + roomsToGenerate);
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
}
