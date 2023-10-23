using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour 
{
    public static InventoryManager instance;

    [SerializeField] private Inventory playerInventory;

    public Inventory Inventory 
    {
        get { return playerInventory; }
        set 
        { 
            playerInventory = value;
            GameEvents.instance.InventoryUpdate();
        } 
    }
    public List<InventorySlot> Slots { get => playerInventory.Slots; }

    private void Awake() 
    {
        instance = this;
        playerInventory = new();
    }

    public bool AddItem(InventoryItem itemToAdd, int amount = 1)
    {
        return playerInventory.AddItem(itemToAdd, amount);
    }

    public bool AddItem(InventoryItem itemToAdd, int slotIndex, int amount = 1)
    {
        return playerInventory.AddItem(itemToAdd, slotIndex, amount);
    }

    public InventoryItem RemoveItem(int slotIndex, int amount = 1)
    {
        return playerInventory.RemoveItem(slotIndex, amount);
    }

    public bool RemoveItem(InventoryItem itemToRemove)
    {
        return playerInventory.RemoveItem(itemToRemove);
    }

    public bool ChargeGold(int gold)
    {
        return playerInventory.ChargeGold(gold);
    }

    public void MoveOnePiece(int fromSlotAIndex, int toSlotBIndex)
    {
        playerInventory.MoveOnePiece(fromSlotAIndex, toSlotBIndex);
    }

    public void SwapItems(int fromSlotAIndex, int toSlotBIndex)
    {
        playerInventory.SwapItems(fromSlotAIndex, toSlotBIndex);
    }

    public void MergeItems(int mergeFromSlotIndex, int mergeToSlotIndex)
    {
        playerInventory.MergeItems(mergeFromSlotIndex, mergeToSlotIndex);
    }

    public bool CheckItem(InventoryItem itemToCheck)
    {
        return playerInventory.CheckItem(itemToCheck);
    }

    public void ClearSlot(int slotIndex)
    {
        playerInventory.ClearSlot(slotIndex);
    }

}