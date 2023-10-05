using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private int slotIndex;
    [SerializeField] private bool empty;
    [SerializeField] private InventoryItem item;
    [SerializeField] private int amount;

    public InventoryItem Item
    {
        get { return item; }
        set { item = value; }
    }

    public bool Empty
    {
        get { return empty; }
        set { empty = value; }
    }

    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public InventorySlot(int slotIndex)
    {
        this.slotIndex = slotIndex;
        empty = true;
        item = null;
        amount = 0;
    }

    public InventorySlot(InventorySlot slot)
    {
        slotIndex = slot.slotIndex;
        empty = slot.Empty;
        item = slot.Item;
        amount = slot.amount;
    }
}