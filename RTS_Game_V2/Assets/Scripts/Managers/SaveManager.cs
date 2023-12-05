using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public Equipment equipment;
    public Inventory inventory;
    public List<Buff> activeBuffsList;
    public PlayerBasicStatistics playerBasicStatistics;
    public float playerHP;
    public string characterName;
    public int levelCompleted;
}

public class SaveManager : MonoBehaviour
{
    private int chosenSlotIndex;
    public int ChosenSlotIndex { get => chosenSlotIndex; set => chosenSlotIndex = value; }

    private void OnEnable()
    {
        GameEvents.instance.OnLoadNextLevel += SaveEquipment;
        GameEvents.instance.OnLoadLevel += LoadSave;
    }

    public void SaveEquipment()
    {
        PlayerData data = new();
        data.equipment = EquipmentManager.instance.Equipment;
        data.inventory = InventoryManager.instance.Inventory;
        data.activeBuffsList = BuffManager.instance.Buffs;
        data.playerBasicStatistics = BuffManager.instance.PlayerBasicStatistics;
        data.playerHP = BuffManager.instance.PlayerHP;
        data.levelCompleted = LevelManager.instance.Level;

        string allData = JsonUtility.ToJson(data);
        string path = GetPath(chosenSlotIndex);
        File.WriteAllText(path, allData);
        GameEvents.instance.PlayerDataSaved();
    }

    public void CreateSave(int slot, PlayerData playerData)
    {
        string path = GetPath(slot);
        string allData = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, allData);
    }

    private string GetPath(int slot)
    {
        string path = "C:/saves";
        switch (slot)
        {
            case 1:
                path += "saveslot1.json";
                break;
            case 2:
                path += "saveslot2.json";
                break;
            case 3:
                path += "saveslot3.json";
                break;
            default:
                break;
        }
        return path;
    }
    public bool GetPlayerData(int slot, out PlayerData playerData)
    {
        string path = GetPath(slot);
        playerData = null;
        if (File.Exists(path))
        {
            string save = File.ReadAllText(path);

            playerData = JsonUtility.FromJson<PlayerData>(save);
            return true;
        }
        return false;
    }

    public void LoadSave()
    {
        if (GetPlayerData(chosenSlotIndex,out PlayerData loadedData))
        {
            BuffManager.instance.PlayerBasicStatistics = loadedData.playerBasicStatistics;
            EquipmentManager.instance.Equipment = loadedData.equipment;
            InventoryManager.instance.Inventory = loadedData.inventory;
            BuffManager.instance.Buffs = loadedData.activeBuffsList;
            LevelManager.instance.Level = loadedData.levelCompleted;
            BuffManager.instance.PlayerHP = loadedData.playerHP;

            GameEvents.instance.PlayerDataLoaded();
        } 
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLoadNextLevel -= SaveEquipment;
        GameEvents.instance.OnLoadLevel -= LoadSave;
    }
}
