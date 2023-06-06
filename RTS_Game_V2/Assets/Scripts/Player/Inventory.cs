using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;
    [SerializeField] public InventorySlot[] itemSlots;

    private int inventorySize;
    public static Inventory Instance { get { return _instance; } }

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
                invSlot.itemAmount = 1;
                itemSlots[i] = invSlot;
                GameEvents.instance.UpdateInventoryUI();
                return true;
            }
            else
            {
                if (itemSlots[i].ItemInSlot == itemToAdd && itemToAdd.IsStackable)
                {

                    invSlot.itemAmount = invSlot.itemAmount + itemSlots[i].itemAmount + 1;
                    itemSlots[i] = invSlot;
                    GameEvents.instance.UpdateInventoryUI();
                    return true;

                }
            }
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

        if(mergingSlot.ItemInSlot == slotToMerge.ItemInSlot)
        {
            if (mergingSlot.ItemInSlot.IsStackable)
            {
                slotToMerge.itemAmount += mergingSlot.itemAmount;
                RemoveItem(mergeFromSlotIndex, mergingSlot.itemAmount);
            }
        }
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
            invSlot.itemAmount = 1;
            itemSlots[slotIndex] = invSlot;
            GameEvents.instance.UpdateInventoryUI();
            return true;
        }

        return false;
    }


    public void GetOneItem(int fromSlotAIndex,int toSlotBIndex)
    {
        InventorySlot donorSlot  = itemSlots[fromSlotAIndex];

        if(donorSlot != null)
        {
            if (donorSlot.itemAmount <= 1)
            {
                SwapItems(fromSlotAIndex, toSlotBIndex);
            }
            else
            {
                if (AddItem(donorSlot.ItemInSlot, toSlotBIndex))
                {
                    donorSlot.itemAmount -= 1;
                }
            }
        }
        GameEvents.instance.UpdateInventoryUI();
    }

    public void SwapItems(int requestingSlotIndex, int destinationSlotIndex)
    {
        InventorySlot requestingSlot = itemSlots[requestingSlotIndex];
        InventorySlot slotB = itemSlots[destinationSlotIndex];

        if(requestingSlot != null)
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
                    if (itemSlots[i].itemAmount == 1)
                    {
                        ClearSlot(i);
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

    public void RemoveItem(int slotNumber, int amountToRemove = 1)
    {
        InventorySlot invSlot = itemSlots[slotNumber];

        if (slotNumber >= 0 && slotNumber <= inventorySize)
        {
            int amountCorrected = Mathf.Clamp(amountToRemove, 0, invSlot.itemAmount);

            if (amountCorrected == invSlot.itemAmount)
            {
                ClearSlot(slotNumber);
            }
            else
            {
                invSlot.itemAmount--;
            }
            GameEvents.instance.UpdateInventoryUI();
        }
    }

    private void ClearSlot(int slotNumber)
    {
        itemSlots[slotNumber] = null;
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

    public InventorySlot[] GetInventorySlots() { return itemSlots; }
}
