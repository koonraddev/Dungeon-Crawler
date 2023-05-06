using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newInventory", menuName = "Scriptable Objects/Inventory", order = 1)]
public class InventorySO : ScriptableObject
{
    public List<KeySO> items = new();
    public void AddItem(KeySO itemToAdd)
    {
        items.Add(itemToAdd);
    }
    public void RemoveItem(KeySO itemToRemove)
    {
        items.Remove(itemToRemove);
    }

    public bool CheckItem(KeySO itemToCheck)
    {
        foreach(KeySO invItem in items)
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

    public List<KeySO> GetInventory()
    {
        return items;
    }
}
