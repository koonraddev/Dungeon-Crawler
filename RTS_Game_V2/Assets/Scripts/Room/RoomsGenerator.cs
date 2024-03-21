using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    [Serializable]
    private class LevelsRangeSetiings
    {
        public LevelRange levelRange;
        public RoomsSetSO roomsSet;
    }

    [Serializable]
    private class LevelException
    {
        public int level;
        public RoomsSetSO roomsSet;
        public int roomsAmount;
    }

    public static RoomsGenerator instance;
    System.Random rand;
    Array levelRangeValues = Enum.GetValues(typeof(LevelRange));
    int arrayLength;


    private bool firstRoom;
    private List<RoomSO> roomsList;
    private RoomSO startRoom;
    private RoomSO bossRoom;
    private RoomsSetSO roomsSetSO;
    [SerializeField] private RoomsSetSO defaultRoomsSet;
    [SerializeField] private int defaultRoomsAmount;
    [SerializeField] private List<LevelsRangeSetiings> levelRangeSetsList;
    [SerializeField] private List<LevelException> levelExcSetsList;
    private List<SpawnPoint> spawnPoints = new();
    private int roomsLeft;
    public int RoomsLeft { get => roomsLeft; }
    private int roomsToGenerate;
    public int RoomsToGenerate { get => roomsToGenerate; }

    private int roomsLeftToGenerate;
    private void Awake()
    {
        rand = new System.Random();
        instance = this;
        arrayLength = levelRangeValues.Length;
    }

    public void ResetRooms()
    {
        roomsLeft = roomsToGenerate;
        roomsLeftToGenerate = roomsToGenerate;
        firstRoom = true;
    }

    public void AddSpawnPoint(SpawnPoint spawnPoint)
    {
        spawnPoints.Add(spawnPoint);
        roomsLeft--;
    }

    public void RunNextSpawner()
    {
        if (spawnPoints.Count > 0)
        {
            SpawnPoint sp = spawnPoints[0];
            if (!sp.EnableSpawn())
            {
                roomsLeft++;
                StartCoroutine(SP());
            }
            spawnPoints.Remove(sp);
        }
        else
        {
            GameEvents.instance.LastRoomReady();
        }

    }

    private IEnumerator SP()
    {
        yield return new WaitForEndOfFrame();
        RunNextSpawner();

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
        ResetRooms();
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
            if (roomsLeftToGenerate == 1)
            {
                roomSo = bossRoom;
            }
            else
            {
                int index = rand.Next(0, roomsList.Count);
                roomSo = roomsList[index];
            }
        }
        roomsLeftToGenerate--;
        return roomSo;
    }
}
