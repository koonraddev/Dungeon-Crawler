using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootSlot
{
    public ScriptableObject treasure;
    public int amount;
    [Range(0, 100)]
    public float lootChancePercentage;
}

[CreateAssetMenu(fileName = "newLoot", menuName = "Scriptable Objects/Loot")]
public class LootSO : ScriptableObject
{
    [SerializeField] List<LootSlot> lootSlots = new(10);
    [SerializeField] private int lootTimeExisting;
    public int LootTimeExisting { get => lootTimeExisting; }
    private List<ContainerSlot> containerSlots;
    int i;
    private void OnValidate()
    {
        if (lootSlots.Count > 10)
        {
            int slotsToCreate = lootSlots.Count - 10;
            for (int i = 0; i < slotsToCreate; i++)
            {
                lootSlots.RemoveAt(lootSlots.Count - 1);
            }
        }
        else if (lootSlots.Count < 10)
        {
            int slotsToCreate = 10 - lootSlots.Count;
            for (int i = 0; i < slotsToCreate; i++)
            {
                lootSlots.Add(null);
            }
        }
    }

    public List<ContainerSlot> GetLoot()
    {
        containerSlots = new();
        i = 0;
        //lootSlots.Sort((a, b) => b.lootChancePercentage.CompareTo(a.lootChancePercentage));
        foreach (LootSlot ltSlot in lootSlots)
        {
            float randomLootChance = Random.Range(0f, 101f);
            if (randomLootChance <= ltSlot.lootChancePercentage)
            {
                AddLoot(ltSlot);
            }
        }
        return containerSlots;
    }

    private void AddLoot(LootSlot ltSlot)
    {
        if(ltSlot.treasure is EquipmentItemSO)
        {
            Item newItem = new EquipmentItem(ltSlot.treasure as EquipmentItemSO);
            ContainerSlot newSlot = new(i, newItem, ltSlot.amount);
            containerSlots.Add(newSlot);
            i++;
            return;
        }

        if (ltSlot.treasure is UnknownItemSO)
        {
            Item newItem = new UnknownItem(ltSlot.treasure as UnknownItemSO);
            ContainerSlot newSlot = new(i, newItem, ltSlot.amount);
            containerSlots.Add(newSlot);
            i++;
            return;
        }

        if (ltSlot.treasure is UsableItemSO)
        {
            Item newItem = new UsableItem(ltSlot.treasure as UsableItemSO);
            ContainerSlot newSlot = new(i, newItem, ltSlot.amount);
            containerSlots.Add(newSlot);
            i++;
            return;
        }

        if(ltSlot.treasure is PassiveItemSO)
        {
            Item newItem = new PassiveItem(ltSlot.treasure as PassiveItemSO);
            ContainerSlot newSlot = new(i, newItem, ltSlot.amount);
            containerSlots.Add(newSlot);
            i++;
            return;
        }
    }
}
