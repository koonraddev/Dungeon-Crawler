using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public Equipment equipment;
    public Inventory inventory;
    //public string jsonBasicStatistics;
}

public class SaveManager : MonoBehaviour
{
    public string savePath;
    public EquipmentItemSO eqItemSO;
    public PassiveItemSO passiveItemSO;
    public UsableItemSO usableSO;
    void Start()
    {
        //GameEvents.instance.onSave += SaveEquipment;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveEquipment(savePath);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            EquipmentItem item = new(eqItemSO);

            EquipmentManager.instance.AddItem(item);

            InventoryItem invItem = new PassiveItem(passiveItemSO);
            InventoryItem invItem2 = new UsableItem(usableSO);
            InventoryManager.instance.AddItem(invItem, slotIndex: 5);
            InventoryManager.instance.AddItem(invItem2, amount: 3);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            List<EquipmentSlot> slots = EquipmentManager.instance.GetSlots();


            foreach (var eqSlot in slots)
            {
                Debug.Log("----------------------");
                Debug.Log(eqSlot.SlotType);
                if(!eqSlot.Empty)
                {
                    Debug.Log(eqSlot.Item.Name);
                }
            }

            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            List<InventorySlot> invSlots = InventoryManager.instance.GetInventorySlots();
            foreach (var invSlot in invSlots)
            {
                Debug.Log("----------------------");
                Debug.Log(invSlot.Item);
                if (invSlot.Item != null)
                {
                    Debug.Log(invSlot.Item.Name);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadEquipment(savePath);
        }
    }

    public void SaveEquipment(string savePath)
    {
        Debug.Log(savePath);

        PlayerData data = new PlayerData();
        data.equipment = EquipmentManager.instance.GetEquipment();
        data.inventory = InventoryManager.instance.GetInventory();
        Debug.Log(Application.persistentDataPath);

        string allData = JsonUtility.ToJson(data);
        
        File.WriteAllText(savePath + "/sejw.json", allData);




        //
    }


    public void LoadEquipment(string savePath)
    {
        if (File.Exists(savePath + "/sejw.json"))
        {
            Debug.Log("istnieje");
            string save = File.ReadAllText(savePath + "/sejw.json");

            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(save);

            Equipment loadedEq = loadedData.equipment;
            EquipmentManager.instance.LoadEquipment(loadedEq);
            Inventory loadedInv = loadedData.inventory;
            InventoryManager.instance.LoadInventory(loadedInv);




            List<EquipmentSlot> eqSlots = EquipmentManager.instance.GetSlots();
            List<InventorySlot> invSlots = InventoryManager.instance.GetInventorySlots();

            foreach (var item in eqSlots)
            {
                Debug.Log("----------------------");
                Debug.Log(item.SlotType);
                Debug.Log(item.Item);
                if (item.Item != null)
                {
                    Debug.Log(item.Item.Name);
                }
            }

            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            foreach (var item in invSlots)
            {
                Debug.Log("----------------------");
                Debug.Log(item.Item);
                if (item.Item != null)
                {
                    Debug.Log(item.Item.Name);
                }
            }
        }   
    }
}
