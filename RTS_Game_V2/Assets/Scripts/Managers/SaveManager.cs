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
    //temporary
    public int levelCompleted;
}

public class SaveManager : MonoBehaviour
{
    public string savePath;
    [SerializeField] private StatisticsSO playerBaseStats;
    void Start()
    {
        //GameEvents.instance.onSave += SaveEquipment;
    }

    private void OnEnable()
    {
        GameEvents.instance.OnLoadNextLevel += SaveEquipment;
        GameEvents.instance.OnLoadLevel += LoadEquipment;
    }

    private void Awake()
    {
        BuffManager.instance.PlayerBasicStatistics = new(playerBaseStats);   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SaveEquipment();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadEquipment();
        }
    }

    public void SaveEquipment()
    {
        Debug.Log(savePath);

        PlayerData data = new();
        data.equipment = EquipmentManager.instance.Equipment;
        data.inventory = InventoryManager.instance.Inventory;
        data.activeBuffsList = BuffManager.instance.Buffs;
        data.playerBasicStatistics = BuffManager.instance.PlayerBasicStatistics;
        data.playerHP = BuffManager.instance.PlayerHP;
        data.levelCompleted = LevelManager.instance.Level;

        Debug.Log(Application.persistentDataPath);

        string allData = JsonUtility.ToJson(data);
        
        File.WriteAllText(savePath + "/sejw.json", allData);
        GameEvents.instance.PlayerDataSaved();
        Debug.LogWarning("GAME SAVED");
    }


    public void LoadEquipment()
    {
        if (File.Exists(savePath + "/sejw.json"))
        {
            string save = File.ReadAllText(savePath + "/sejw.json");

            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(save);

            Equipment loadedEq = loadedData.equipment;
            Inventory loadedInv = loadedData.inventory;
            List<Buff> loadedActiveBuffsList = loadedData.activeBuffsList;
            PlayerBasicStatistics loadedPlayerBaseStatistics = loadedData.playerBasicStatistics;
            int loadedCompletedLevel = loadedData.levelCompleted;
            float loadedPlayerHP = loadedData.playerHP;

            BuffManager.instance.PlayerBasicStatistics = loadedPlayerBaseStatistics;
            EquipmentManager.instance.Equipment = loadedEq;
            InventoryManager.instance.Inventory = loadedInv;
            BuffManager.instance.Buffs = loadedActiveBuffsList;
            LevelManager.instance.Level = loadedCompletedLevel;
            //GameEvents.instance.UpdateCurrentHP(loadedPlayerHP);
            BuffManager.instance.PlayerHP = loadedPlayerHP;


            GameEvents.instance.PlayerDataLoaded();
            //List<EquipmentSlot> eqSlots = EquipmentManager.instance.Slots;
            //List<InventorySlot> invSlots = InventoryManager.instance.Slots;

            //foreach (var item in eqSlots)
            //{
            //    Debug.Log("----------------------");
            //    Debug.Log(item.SlotType);
            //    Debug.Log(item.Item);
            //    if (item.Item != null)
            //    {
            //        Debug.Log(item.Item.Name); 
            //    }
            //}

            //Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            //foreach (var item in invSlots)
            //{
            //    Debug.Log("----------------------");
            //    Debug.Log(item.Item);
            //    if (item.Item != null)
            //    {
            //        Debug.Log(item.Item.Name);
            //    }
            //}
        }   
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLoadNextLevel -= SaveEquipment;
        GameEvents.instance.OnLoadLevel -= LoadEquipment;
    }
}
