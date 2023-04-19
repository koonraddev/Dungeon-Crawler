using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newInventory", menuName = "Scriptable Objects/Inventory", order = 1)]
public class InventorySO : ScriptableObject
{
    public List<ItemSO> items = new();
    public void AddItem(ItemSO itemToAdd)
    {
        items.Add(itemToAdd);
    }
    public void RemoveItem(ItemSO itemToRemove)
    {
        items.Remove(itemToRemove);
    }

    public bool CheckItem(ItemSO itemToCheck)
    {
        foreach(ItemSO invItem in items)
        {
            if (invItem.Id == itemToCheck.Id)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearInventory()
    {
        items = new();
    }

    public List<ItemSO> GetInventory()
    {
        return items;
    }
}
