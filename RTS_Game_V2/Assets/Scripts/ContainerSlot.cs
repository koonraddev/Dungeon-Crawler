using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContainerSlot
{
    private int slotIndex;
    private bool empty;
    private Item item;
    private int amount;

    public Item Item
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

    public ContainerSlot(int slotIndex)
    {
        this.slotIndex = slotIndex;
        empty = true;
        item = null;
        amount = 0;
    }

    public ContainerSlot(ContainerSlot slot)
    {
        this.empty = slot.empty;
        this.slotIndex = slot.slotIndex;
        this.item = slot.item;
        this.amount = slot.amount;
    }

    public ContainerSlot(int slotIndex, Item item, int amount)
    {
        empty = false;
        this.slotIndex = slotIndex;
        this.item = item;
        this.amount = amount;
    }
}
