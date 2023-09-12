using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentSlot
{
    private ItemSlotType itemSlotType;
    private IEquipmentItem eqItem;
    [SerializeField]
    public EquipmentSlot(ItemSlotType itemSlotType)
    {
        this.itemSlotType = itemSlotType;
        eqItem = null;
    }

    public ItemSlotType ItemSlotType { get => itemSlotType; }
    public IEquipmentItem ItemInSlot { get => eqItem; set { eqItem = value; } }
}

[CreateAssetMenu(fileName = "newEquipment", menuName = "Scriptable Objects/Player/Equipment", order = 2)]
public class EquipmentSO : ScriptableObject
{
    [SerializeField] private List<EquipmentSlot> slotsList;
    private void Awake()
    {
        slotsList = new List<EquipmentSlot>();
        foreach (ItemSlotType val in System.Enum.GetValues(typeof(ItemSlotType)))
        {
            EquipmentSlot eqSlot = new EquipmentSlot(val);
            slotsList.Add(eqSlot);
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool AddItem(IEquipmentItem itemToAdd)
    {
        foreach (var item in slotsList)
        {
            if((item.ItemSlotType == itemToAdd.ItemSlotType) && item.ItemInSlot == null)
            {
                item.ItemInSlot = itemToAdd;
                GameEvents.instance.UpdateEquipmentUI();
                return true;
            }
        }
        return false;

    }

    public void RemoveItem(IEquipmentItem itemToRemove)
    {
        foreach (var item in slotsList)
        {
            if (item.ItemInSlot == itemToRemove)
            {
                GameEvents.instance.UpdateEquipmentUI();
                item.ItemInSlot = null;
            }
        }
    }

    public void RemoveItem(ItemSlotType slotToClear)
    {
        foreach (var item in slotsList)
        {
            if (item.ItemSlotType == slotToClear)
            {
                GameEvents.instance.UpdateEquipmentUI();
                item.ItemInSlot = null;
            }
        }
    }

    public List<EquipmentSlot> GetEquipmentSlots()
    {
        return slotsList;
    }

    public EquipmentSlot GetEquipmentSlot(ItemSlotType itemSlotType)
    {
        foreach (EquipmentSlot eqSlot in slotsList)
        {
            if(eqSlot.ItemSlotType == itemSlotType)
            {
                return eqSlot;
            }
        }
        return null;
    }
}
