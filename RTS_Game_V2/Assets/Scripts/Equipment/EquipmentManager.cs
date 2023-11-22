using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    [SerializeReference] private Equipment playerEquipment;

    public Equipment Equipment 
    {
        get { return playerEquipment; } 
        set 
        { 
            playerEquipment = value;
            GameEvents.instance.EquipmentUpdate();
        }
    }
    public List<EquipmentSlot> Slots { get => playerEquipment.Slots; }
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

    public EquipmentSlot GetEquipmentSlot(EquipmentSlotType slotType)
    {
        return playerEquipment.GetEquipmentSlot(slotType);
    }
    public Dictionary<StatisticType, float> GetStatistics(EquipmentSlotType itemSlotType)
    {
        return playerEquipment.GetStatistics(itemSlotType);
    }
}