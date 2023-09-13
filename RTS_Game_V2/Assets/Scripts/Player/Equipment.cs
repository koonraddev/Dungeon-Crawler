using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private static Equipment _instance;

    public static Equipment Instance { get { return _instance; } }

    public EquipmentSO EquipmentSO { get; set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public bool AddItem(IEquipmentItem itemToAdd)
    {
        //if (EquipmentSO.AddItem(itemToAdd))
        //{
        //    GameEvents.instance.UpdateEquipmentUI();
        //    return true;
        //}
        //return false;
        return EquipmentSO.AddItem(itemToAdd);
    }

    public void RemoveItem(IEquipmentItem itemToRemove)
    {
        EquipmentSO.RemoveItem(itemToRemove);
    }

    public void RemoveItem(ItemSlotType slotToClear)
    {
        EquipmentSO.RemoveItem(slotToClear);
    }

    public List<EquipmentSlot> GetEquipmentSlots()
    {
        return EquipmentSO.GetEquipmentSlots();
    }

    public EquipmentSlot GetEquipmentSlot(ItemSlotType itemSlotType)
    {
        return EquipmentSO.GetEquipmentSlot(itemSlotType);
    }

    public Dictionary<StatisticType, float> GetStatistics(ItemSlotType itemSlotType)
    {
        return EquipmentSO.GetStatistics(itemSlotType);
    }
}
