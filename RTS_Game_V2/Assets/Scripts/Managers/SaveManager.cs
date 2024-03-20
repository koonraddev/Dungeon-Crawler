using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class PlayerData
{
    public Equipment equipment;
    public Inventory inventory;
    public List<Buff> activeBuffsList;
    public StatisticsSet playerBasicStatistics;
    public float playerHP;
    public string characterName;
    public int levelCompleted;
    public string dateTime;
}


public class SaveManager : MonoBehaviour
{
    private int chosenSlotIndex;
    public int ChosenSlotIndex { get => chosenSlotIndex; set => chosenSlotIndex = value; }

    public static SaveManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        GameEvents.instance.OnLoadNextLevel += SaveEquipment;
        GameEvents.instance.OnLoadLevel += LoadSave;
    }

    public void SaveEquipment()
    {
        PlayerData playerData = new();

        playerData.equipment = EquipmentManager.instance.Equipment;
        playerData.inventory = InventoryManager.instance.Inventory;
        playerData.activeBuffsList = BuffManager.instance.Buffs;
        playerData.playerBasicStatistics = BuffManager.instance.PlayerBasicStatistics;
        playerData.playerHP = BuffManager.instance.PlayerHP;
        playerData.levelCompleted = LevelManager.instance.Level;
        playerData.dateTime = DateTime.Now.ToString();

        string allData = JsonUtility.ToJson(playerData);
        string path = GetPath(chosenSlotIndex);
        File.WriteAllText(path, allData);
        GameEvents.instance.PlayerDataSaved();
    }

    public void CreateSave(int slot, string characterName, StatisticsSet playerBasicStatisitcs)
    {
        PlayerData playerData = new();

        playerData.equipment = new();
        playerData.inventory = new();
        playerData.activeBuffsList = new();
        playerData.playerBasicStatistics = playerBasicStatisitcs;
        playerData.playerHP = playerBasicStatisitcs.maxHealth;
        playerData.characterName = characterName;
        playerData.levelCompleted = 0;
        playerData.dateTime = DateTime.Now.ToString();

        string path = GetPath(slot);
        string allData = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, allData);

    }
    public void LoadSave()
    {
        if (GetPlayerData(chosenSlotIndex, out PlayerData loadedData))
        {
            BuffManager.instance.PlayerBasicStatistics = loadedData.playerBasicStatistics;
            EquipmentManager.instance.Equipment = loadedData.equipment;
            InventoryManager.instance.Inventory = loadedData.inventory;
            BuffManager.instance.Buffs = loadedData.activeBuffsList;
            LevelManager.instance.Level = loadedData.levelCompleted;
            BuffManager.instance.LoadedPlayerHP = loadedData.playerHP;

            GameEvents.instance.PlayerDataLoaded();
        }
        else
        {
            GameEvents.instance.ExitToMenu();
        }
    }

    private string GetPath(int slot)
    {
        string path = "C:/saves";
        switch (slot)
        {
            case 1:
                path += "/saveslot1.json";
                break;
            case 2:
                path += "/saveslot2.json";
                break;
            case 3:
                path += "/saveslot3.json";
                break;
            default:
                break;
        }
        return path;
    }
    public bool GetPlayerData(int slot, out PlayerData playerData)
    {
        playerData = null;
        if(GetFile(slot, out string save))
        {
            try
            {
                playerData = JsonUtility.FromJson<PlayerData>(save);
            }
            catch (System.Exception)
            {
                return false;
            }
            playerData = JsonUtility.FromJson<PlayerData>(save);
            return true;
        }
        return false;
    }

    private bool GetFile(int slot, out string save)
    {
        save = "";
        string path = GetPath(slot);
        if (File.Exists(path))
        {
            save = File.ReadAllText(path);
            return true;
        }
        return false;
    }


    public void DeleteSave(int slotIndex)
    {
        if (GetFile(slotIndex, out string save))
        {
            File.Delete(GetPath(slotIndex));
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLoadNextLevel -= SaveEquipment;
        GameEvents.instance.OnLoadLevel -= LoadSave;
    }
}
