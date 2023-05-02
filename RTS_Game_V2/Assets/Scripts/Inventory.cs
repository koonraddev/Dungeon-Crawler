using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;
    [SerializeField] public InventorySlot[] itemSlots;

    private int inventorySize;
    public static Inventory Instance { get { return _instance; } }
    public int index;
    public int InventorySize
    {
        get { return inventorySize; }
        set
        {
            inventorySize = Mathf.Clamp(value, 1, 10);
        }
    }
    [System.Serializable]
    public class InventorySlot
    {
        public string SlotName { get; set; }
        public IInventoryItem ItemInSlot { get; set; }
        public int itemAmount { get; set; }
    }
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

    private void Start()
    {
        ClearInventory();
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

    public void AddItem(IInventoryItem itemToAdd)
    {
        InventorySlot invSlot = new InventorySlot
        {
            SlotName = itemToAdd.NameText,
            ItemInSlot = itemToAdd
        };

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i] == null)
            {
                invSlot.itemAmount = 1;
                itemSlots[i] = invSlot;
                GameEvents.instance.UpdateInventoryUI();
                break;
            }
            else
            {
                if (itemSlots[i].ItemInSlot == itemToAdd && itemToAdd.IsStackable)
                {

                    invSlot.itemAmount = invSlot.itemAmount + itemSlots[i].itemAmount + 1;
                    itemSlots[i] = invSlot;
                    GameEvents.instance.UpdateInventoryUI();
                    break;

                }
            }
        }
    }



    public void RemoveItem(IInventoryItem itemToRemove)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                if (itemSlots[i].ItemInSlot == itemToRemove)
                {
                    if (itemSlots[i].itemAmount == 1)
                    {
                        itemSlots[i] = null;
                    }
                    else
                    {
                        itemSlots[i].itemAmount--;
                    }
                    GameEvents.instance.UpdateInventoryUI();
                    break;

                }
            }

        }
    }

    public void RemoveItem(int slotNumber)
    {
        if (slotNumber >= 0 && slotNumber <= inventorySize)
        {
            itemSlots[slotNumber] = null;
            GameEvents.instance.UpdateInventoryUI();
        }
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
        return itemSlots;
    }
}
