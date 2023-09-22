using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;

    public static Inventory Instance { get { return _instance; } }

    public InventorySO InventorySO { get; set; }

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


    public int GetInventorySize()
    {
        return InventorySO.GetInventorySize();
    }

    public void AddGold(int gold)
    {
        InventorySO.AddGold(gold);
    }

    public bool ChargeGold(int gold)
    {
        return InventorySO.ChargeGold(gold);
    }

    public bool CheckEmptySlot()
    {
        return InventorySO.CheckEmptySlot();
    }

    public bool AddItem(IInventoryItem itemToAdd)
    {
        return InventorySO.AddItem(itemToAdd);
    }

    public bool AddItem(IInventoryItem itemToAdd, int slotIndex)
    {
        return InventorySO.AddItem(itemToAdd, slotIndex);
    }

    public void MergeItems(int mergeFromSlotIndex, int mergeToSlotIndex)
    {
        InventorySO.MergeItems(mergeFromSlotIndex, mergeToSlotIndex);
    }

    public void GetOneItem(int fromSlotAIndex,int toSlotBIndex)
    {
        InventorySO.GetOneItem(fromSlotAIndex, toSlotBIndex);
    }

    public void SwapItems(int requestingSlotIndex, int destinationSlotIndex)
    {
        InventorySO.SwapItems(requestingSlotIndex, destinationSlotIndex);
    }

    public void RemoveItem(IInventoryItem itemToRemove)
    {
        InventorySO.RemoveItem(itemToRemove);
    }

    public void RemoveItem(int slotNumber, int amountToRemove = 1)
    {
        InventorySO.RemoveItem(slotNumber, amountToRemove);
    }

    public void ClearSlot(int slotNumber)
    {
        InventorySO.ClearSlot(slotNumber);
    }

    public bool CheckItem(IInventoryItem itemToCheck)
    {
        return InventorySO.CheckItem(itemToCheck);
    }

    public void ClearInventory()
    {
        InventorySO.ClearInventory();
    }

    public InventorySO.InventorySlot[] GetInventorySlots() 
    {
        Debug.Log("Get Inv slots in Inventory");
        return InventorySO.GetInventorySlots();
    }
    public InventorySO.InventorySlot GetInventorySlot(int slotIndex) 
    { 
        return InventorySO.GetInventorySlot(slotIndex); 
    }
}
