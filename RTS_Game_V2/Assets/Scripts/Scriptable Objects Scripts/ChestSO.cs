using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChestSlot
{
    public ScriptableObject treasure;
    public int amount;
}

[CreateAssetMenu(fileName = "newChest", menuName = "Scriptable Objects/Chest")]
public class ChestSO : ScriptableObject
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private int chestId;
    [SerializeField] List<ChestSlot> treasureList = new(25);
    private List<ContainerSlot> contSlots = new(25);

    private void Awake()
    {
        if (treasureList == null)
        {
            treasureList = new();
            {
                for (int i = 0; i < 25; i++)
                {
                    treasureList.Add(new ChestSlot());
                }
            }
        }
    }
    private void OnValidate()
    {
        if (treasureList.Count > 25)
        {
            int slotsToCreate = treasureList.Count - 25;
            for (int i = 0; i < slotsToCreate; i++)
            {
                treasureList.RemoveAt(treasureList.Count - 1);
            }
        }
        else if (treasureList.Count < 25)
        {
            int slotsToCreate = 25 - treasureList.Count;
            for (int i = 0; i < slotsToCreate; i++)
            {
                treasureList.Add(null);
            }
        }
    }

    public Container Container
    {
        get
        {
            contSlots = new();
            for (int i = 0; i < treasureList.Count; i++)
            {
                ChestSlot itemSO = treasureList[i];
                if (itemSO.treasure == null)
                {
                    ContainerSlot newSlot = new(i);
                    contSlots.Add(newSlot);
                }
                else
                {
                    if (itemSO.treasure is EquipmentItemSO)
                    {
                        Item newItem = new EquipmentItem(itemSO.treasure as EquipmentItemSO);
                        ContainerSlot newSlot = new(i, newItem, itemSO.amount);
                        contSlots.Add(newSlot);
                        continue;
                    }

                    if (itemSO.treasure is UnknownItemSO)
                    {
                        Item newItem = new UnknownItem(itemSO.treasure as UnknownItemSO);
                        ContainerSlot newSlot = new(i, newItem, itemSO.amount);
                        contSlots.Add(newSlot);
                        continue;
                    }

                    if (itemSO.treasure is UsableItemSO)
                    {
                        Item newItem = new UsableItem(itemSO.treasure as UsableItemSO);
                        ContainerSlot newSlot = new(i, newItem, itemSO.amount);
                        contSlots.Add(newSlot);
                        continue;
                    }

                    if (itemSO.treasure is PassiveItemSO)
                    {
                        Item newItem = new PassiveItem(itemSO.treasure as PassiveItemSO);
                        ContainerSlot newSlot = new(i, newItem, itemSO.amount);
                        contSlots.Add(newSlot);
                        continue;
                    }
                }
            }
            Container container = new( nameText, description, contSlots);
            return container;
        }
    }
}
