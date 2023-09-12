using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newInventory", menuName = "Scriptable Objects/Player/Inventory", order = 1)]
public class InventorySO : ScriptableObject
{
    [SerializeField] [HideInInspector] private InventorySlot[] itemSlots;
    private readonly int inventorySize = 25;
    [SerializeField] [HideInInspector] private int playerGold;

    public class InventorySlot
    {
        public string SlotName { get; set; }
        public IInventoryItem ItemInSlot { get; set; }
        public int ItemAmount { get; set; }
    }

    private void OnEnable()
    {
        if (itemSlots == null)
        {
            itemSlots = new InventorySlot[inventorySize];
            playerGold = 10;
        }
    }

    public int GetInventorySize()
    {
        return inventorySize;
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
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckEmptySlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool AddItem(IInventoryItem itemToAdd)
    {
        InventorySlot invSlot = new InventorySlot
        {
            SlotName = itemToAdd.NameText,
            ItemInSlot = itemToAdd
        };

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == null)
            {
                invSlot.ItemAmount = 1;
                itemSlots[i] = invSlot;
                GameEvents.instance.UpdateInventoryUI();
                return true;
            }
            else
            {
                if (itemSlots[i].ItemInSlot == itemToAdd && itemToAdd.IsStackable)
                {

                    invSlot.ItemAmount = invSlot.ItemAmount + itemSlots[i].ItemAmount + 1;
                    itemSlots[i] = invSlot;
                    GameEvents.instance.UpdateInventoryUI();
                    return true;

                }
            }
        }
        return false;
    }

    public bool AddItem(IInventoryItem itemToAdd, int slotIndex)
    {
        InventorySlot invSlot = new InventorySlot
        {
            SlotName = itemToAdd.NameText,
            ItemInSlot = itemToAdd
        };

        if (itemSlots[slotIndex] == null)
        {
            invSlot.ItemAmount = 1;
            itemSlots[slotIndex] = invSlot;
            GameEvents.instance.UpdateInventoryUI();
            return true;
        }

        return false;
    }

    public void MergeItems(int mergeFromSlotIndex, int mergeToSlotIndex)
    {
        InventorySlot mergingSlot = itemSlots[mergeFromSlotIndex];
        InventorySlot slotToMerge = itemSlots[mergeToSlotIndex];
        if (mergingSlot == null || slotToMerge == null)
        {
            return;
        }

        if (mergingSlot.ItemInSlot == slotToMerge.ItemInSlot)
        {
            if (mergingSlot.ItemInSlot.IsStackable)
            {
                slotToMerge.ItemAmount += mergingSlot.ItemAmount;
                RemoveItem(mergeFromSlotIndex, mergingSlot.ItemAmount);
            }
        }
    }

    public void GetOneItem(int fromSlotAIndex, int toSlotBIndex)
    {
        InventorySlot donorSlot = itemSlots[fromSlotAIndex];

        if (donorSlot != null)
        {
            if (donorSlot.ItemAmount <= 1)
            {
                SwapItems(fromSlotAIndex, toSlotBIndex);
            }
            else
            {
                if (AddItem(donorSlot.ItemInSlot, toSlotBIndex))
                {
                    donorSlot.ItemAmount -= 1;
                }
            }
        }
        GameEvents.instance.UpdateInventoryUI();
    }

    public void SwapItems(int requestingSlotIndex, int destinationSlotIndex)
    {
        InventorySlot requestingSlot = itemSlots[requestingSlotIndex];
        InventorySlot slotB = itemSlots[destinationSlotIndex];

        if (requestingSlot != null)
        {
            itemSlots[requestingSlotIndex] = slotB;
            itemSlots[destinationSlotIndex] = requestingSlot;
        }
        GameEvents.instance.UpdateInventoryUI();
    }

    public void RemoveItem(IInventoryItem itemToRemove)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                if (itemSlots[i].ItemInSlot == itemToRemove)
                {
                    if (itemSlots[i].ItemAmount == 1)
                    {
                        ClearSlot(i);
                    }
                    else
                    {
                        itemSlots[i].ItemAmount--;
                    }
                    GameEvents.instance.UpdateInventoryUI();
                    break;

                }
            }
        }
    }

    public void RemoveItem(int slotNumber, int amountToRemove = 1)
    {
        InventorySlot invSlot = itemSlots[slotNumber];

        if (slotNumber >= 0 && slotNumber <= inventorySize)
        {
            int amountCorrected = Mathf.Clamp(amountToRemove, 0, invSlot.ItemAmount);

            if (amountCorrected == invSlot.ItemAmount)
            {
                ClearSlot(slotNumber);
            }
            else
            {
                invSlot.ItemAmount--;
            }
            GameEvents.instance.UpdateInventoryUI();
        }
    }

    public void ClearSlot(int slotNumber)
    {
        itemSlots[slotNumber] = null;
        GameEvents.instance.UpdateInventoryUI();
    }

    public bool CheckItem(IInventoryItem itemToCheck)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                if (itemSlots[i].ItemInSlot == itemToCheck)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ClearInventory()
    {
        itemSlots = new InventorySlot[inventorySize];
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = null;
        }
    }

    public InventorySlot[] GetInventorySlots() 
    {
        Debug.Log("Get Inv slots in InventorySO");
        return itemSlots; 
    }
    public InventorySlot GetInventorySlot(int slotIndex) { return itemSlots[slotIndex]; }
}
