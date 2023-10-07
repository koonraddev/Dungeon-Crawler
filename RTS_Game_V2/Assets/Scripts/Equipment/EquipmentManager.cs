using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    [SerializeField] private Equipment playerEquipment;

    private void Awake() {
        instance = this;
        playerEquipment = new();
    }

    public bool AddItem(EquipmentItem itemToAdd)
    {
        return playerEquipment.AddItem(itemToAdd);
    }

    public EquipmentItem RemoveItem(EquipmentItem itemToRemove)
    {
        return playerEquipment.RemoveItem(itemToRemove);
    }

    public void LoadEquipment(Equipment eqtoLoad)
    {
        playerEquipment = eqtoLoad;
    }

    public Equipment GetEquipment()
    {
        return playerEquipment;
    }

    public List<EquipmentSlot> GetSlots()
    {
        return playerEquipment.GetEquipmentSlots();
    }

    public EquipmentSlot GetEquipmentSlot(EquipmentSlotType slotType)
    {
        return playerEquipment.GetEquipmentSlot(slotType);
    }
    public Dictionary<StatisticType, float> GetStatistics(EquipmentSlotType itemSlotType)
    {
        return playerEquipment.GetStatistics(itemSlotType);
    }

    void Test()
    {
        List<Item> items = new();

        //Item itA = new InventoryItem();
        //Item itB = new PassiveItem();
        //Item itC = new UsableItem();
        //Item itD = new UnknownItem();

    }

}