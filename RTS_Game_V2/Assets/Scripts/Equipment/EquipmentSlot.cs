using UnityEngine;

[System.Serializable]
public class EquipmentSlot 
{
    [SerializeField] private EquipmentSlotType slotType;
    [SerializeField] private bool empty;
    [SerializeField] private EquipmentItem item;


    public EquipmentSlotType SlotType { get => slotType; }
    public EquipmentItem Item
    {
        get { return item; }
        set { item = value; }
    }

    public bool Empty
    {
        get { return empty; }
        set { empty = value; }
    }


    public EquipmentSlot(EquipmentSlotType slotType)
    {
        this.slotType = slotType;
        empty = true;
    }
}