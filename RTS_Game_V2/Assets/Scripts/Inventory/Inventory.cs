using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<InventorySlot> slotsList;
    [SerializeField] private int playerGold;
    public List<InventorySlot> Slots { get => slotsList; }
    public Inventory()
    {
        slotsList = new();

        for (int i = 0; i < 25; i++)
        {
            InventorySlot slot = new(i);
            slotsList.Add(slot);
        }

        playerGold = 0;
    }
    public bool AddItem(InventoryItem itemToAdd, int amount = 1)
    {
        bool emptySlotFound = false;
        int emptySlotIndex = 0;
        for (int i = 0; i < slotsList.Count; i++)
        {
            if (slotsList[i].Empty)
            {
                if (!emptySlotFound)
                {
                    emptySlotIndex = i;
                    emptySlotFound = true;
                }
            }
            else
            {
                if (slotsList[i].Item.Equals(itemToAdd))
                {
                    slotsList[i].Amount += amount;
                    GameEvents.instance.InventoryUpdate();
                    return true;
                }
            }
        }

        if (emptySlotFound)
        {
            slotsList[emptySlotIndex].Item = itemToAdd;
            slotsList[emptySlotIndex].Amount = amount;
            slotsList[emptySlotIndex].Empty = false;
            GameEvents.instance.InventoryUpdate();
            return true;
        }
        return false;
    }

    public bool AddItem(InventoryItem itemToAdd, int slotIndex, int amount = 1)
    {
        InventorySlot invSlot = slotsList[slotIndex];

        if (invSlot.Empty)
        {
            invSlot.Item = itemToAdd;
            invSlot.Amount = amount;
            invSlot.Empty = false;
            GameEvents.instance.InventoryUpdate();
            return true;
        }else
        {
            if(invSlot.Item.Equals(itemToAdd))
            {
                invSlot.Amount += amount;
                GameEvents.instance.InventoryUpdate();
                return true;
            }
        }
        return false;
    }



    public InventoryItem RemoveItem(int slotIndex, int amount = 1)
    {
        InventorySlot invSlot = slotsList[slotIndex];
        InventoryItem invItem = invSlot.Item;
        invSlot.Amount -= amount;
        if(invSlot.Amount <= 0)
        {
            invSlot.Item = null;
            invSlot.Empty = true;
        }
        GameEvents.instance.InventoryUpdate();
        return invItem;
    }

    public bool RemoveItem(InventoryItem itemToRemove)
    {
        foreach (InventorySlot invSlot in slotsList)
        {
            if(invSlot.Item == itemToRemove)
            {
                invSlot.Amount -= 1;
                if (invSlot.Amount == 0)
                {
                    invSlot.Item = null;
                    invSlot.Empty = true;
                }
                GameEvents.instance.InventoryUpdate();
                return true;
            }
        }
        return false;
    }

    public void AddGold(int gold)
    {
        playerGold += gold;
    }

    public bool ChargeGold(int gold)
    {
        if (gold <= playerGold)
        {
            playerGold -= gold;
            GameEvents.instance.InventoryUpdate();
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool CheckItem(InventoryItem invToCheck, int amount = 1)
    {
        foreach (InventorySlot invSlot in slotsList)
        {
            if(invSlot.Item == invToCheck && invSlot.Amount >= amount)
            {
                return true;
            }
        }

        return false;
    }

    public void MoveOnePiece(int fromSlotAIndex, int toSlotBIndex)
    {
        InventorySlot donorSlot = slotsList[fromSlotAIndex];

        if(!donorSlot.Empty)
        {

            if (AddItem(donorSlot.Item, slotIndex: toSlotBIndex))
            {
                donorSlot.Amount -= 1;
            }
            
            if (donorSlot.Amount <= 0)
            {
                RemoveItem(fromSlotAIndex);
            }

        }

        GameEvents.instance.InventoryUpdate();
    }

    public void SwapItems(int fromSlotAIndex, int toSlotBIndex)
    {
        InventorySlot tmp = new(slotsList[fromSlotAIndex]);

        if (tmp.Empty)
        {
            return;
        }

        slotsList[fromSlotAIndex].Empty = slotsList[toSlotBIndex].Empty;
        slotsList[fromSlotAIndex].Item = slotsList[toSlotBIndex].Item;
        slotsList[fromSlotAIndex].Amount = slotsList[toSlotBIndex].Amount;

        slotsList[toSlotBIndex].Empty = tmp.Empty;
        slotsList[toSlotBIndex].Item = tmp.Item;
        slotsList[toSlotBIndex].Amount = tmp.Amount;
        
        GameEvents.instance.InventoryUpdate();
    }

    public void MergeItems(int mergeFromSlotIndex, int mergeToSlotIndex)
    {
        InventorySlot mergingSlot = slotsList[mergeFromSlotIndex];
        InventorySlot slotToMerge = slotsList[mergeToSlotIndex];
        if (mergingSlot.Empty)
        {
            return;
        }

        if (slotToMerge.Empty)
        {
            SwapItems(mergeFromSlotIndex, mergeToSlotIndex);
        }
        else
        {
            if (mergingSlot.Item.Equals(slotToMerge.Item))
            {
                slotToMerge.Amount += mergingSlot.Amount;
                RemoveItem(mergeFromSlotIndex, mergingSlot.Amount);
            }
        }

        GameEvents.instance.InventoryUpdate();
    }

    public void ClearSlot(int slotIndex)
    {
        InventorySlot slotToClear = slotsList[slotIndex];

        slotToClear.Item = null;
        slotToClear.Amount = 0;
        slotToClear.Empty = true;
        GameEvents.instance.InventoryUpdate();
    }
}
