using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{
    [SerializeField] private List<EquipmentSlot> slotsList;

    public List<EquipmentSlot> Slots { get => slotsList; }
    public Equipment()
    {
        slotsList = new();
        foreach (EquipmentSlotType slotType in System.Enum.GetValues(typeof(EquipmentSlotType)))
        {
            EquipmentSlot eqSlot = new EquipmentSlot(slotType);
            slotsList.Add(eqSlot);
        }
    }

    public bool AddItem(EquipmentItem itemToAdd)
    {
        EquipmentSlot eqSlotToCheck = GetEquipmentSlot(itemToAdd.ItemSlot);
        if (eqSlotToCheck.Empty)
        {
            switch (eqSlotToCheck.SlotType)
            {
                case EquipmentSlotType.LEFT_ARM:
                    EquipmentSlot rightArmSLot = GetEquipmentSlot(EquipmentSlotType.RIGHT_ARM);
                    if (!rightArmSLot.Empty && rightArmSLot.Item.IsWeapon)
                    {
                        return false;
                    }
                    break;
                case EquipmentSlotType.RIGHT_ARM:
                    EquipmentSlot leftArmSLot = GetEquipmentSlot(EquipmentSlotType.LEFT_ARM);
                    if (!leftArmSLot.Empty && leftArmSLot.Item.IsWeapon)
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }

            eqSlotToCheck.Item = itemToAdd;
            eqSlotToCheck.Empty = false;
            GameEvents.instance.EquipmentUpdate();
            return true;
        }
        return false;
    }

    public EquipmentItem RemoveItem(EquipmentItem itemToRemove)
    {
        foreach(EquipmentSlot eqSlot in slotsList)
        {
            if(eqSlot.SlotType == itemToRemove.ItemSlot)
            {
                EquipmentItem eqitem = eqSlot.Item;
                eqSlot.Item = null;
                eqSlot.Empty = true;
                GameEvents.instance.EquipmentUpdate();
                return eqitem;
            }
        }
        return null;
    }

    public EquipmentSlot GetEquipmentSlot(EquipmentSlotType slotType)
    {
        foreach (EquipmentSlot eqSlot in slotsList)
        {
            if(eqSlot.SlotType == slotType)
            {
                return eqSlot;
            }
        }
        return null;
    }

    public Dictionary<StatisticType, float> GetStatistics(EquipmentSlotType itemSlotType)
    {
        EquipmentSlot eqSlot = slotsList.Find(sl => sl.SlotType == itemSlotType);
        EquipmentItem eqItem = eqSlot.Item;

        if (eqItem != null)
        {
            return eqItem.Statistics;
        }
        return null;
    }
}