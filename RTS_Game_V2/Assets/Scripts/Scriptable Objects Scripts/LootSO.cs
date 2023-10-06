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

    public List<ContainerSlot> GetLoot(bool oneLoot = false)
    {
        containerSlots = new();
        i = 0;
        float randomValue = Random.Range(0f, 100f);
        if (oneLoot)
        {
            Debug.Log("one Loot!");
            foreach (LootSlot ltSlot in lootSlots)
            {

                if (randomValue <= ltSlot.lootChancePercentage)
                {
                    AddLoot(ltSlot);
                }
            }
        }
        else
        {
            Debug.Log("many Loot!");
            lootSlots.Sort((a, b) => b.lootChancePercentage.CompareTo(a.lootChancePercentage));
            foreach (LootSlot ltSlot in lootSlots)
            {
                if (randomValue <= ltSlot.lootChancePercentage)
                {
                    AddLoot(ltSlot);
                    break;
                }
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
