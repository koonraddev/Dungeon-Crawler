using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLoot", menuName = "Scriptable Objects/Items/Loot")]
public class LootSO : ScriptableObject
{
    [SerializeField] List<LootItem> lootItems;
    List<Item> finalLoot = new List<Item>();
    public List<Item> LootLogic(bool oneLoot = false)
    {
        float randomValue = Random.Range(0f, 100f);
        if (oneLoot)
        {
            foreach (LootItem lootItem in lootItems)
            {

                if (randomValue <= lootItem.lootChancePercentage)
                {
                    AddLoot(lootItem.item);
                }
            }
        }
        else
        {
            lootItems.Sort((a, b) => b.lootChancePercentage.CompareTo(a.lootChancePercentage));

            foreach (LootItem lootItem in lootItems)
            {
                if (randomValue <= lootItem.lootChancePercentage)
                {
                    AddLoot(lootItem.item);
                    break;
                }
            }
        }
        return finalLoot;
    }

    private void AddLoot(ScriptableObject item)
    {
        if(item is EquipmentItemSO)
        {
            Item newItem = new EquipmentItem(item as EquipmentItemSO);
            finalLoot.Add(newItem);
            return;
        }

        if (item is UnknownItemSO)
        {
            Item newItem = new UnknownItem(item as UnknownItemSO);
            finalLoot.Add(newItem);
            return;
        }

        if (item is UsableItemSO)
        {
            Item newItem = new UsableItem(item as UsableItemSO);
            finalLoot.Add(newItem);
            return;
        }

        if(item is PassiveItemSO)
        {
            Item newItem = new PassiveItem(item as PassiveItemSO);
            finalLoot.Add(newItem);
            return;
        }
    }
}
