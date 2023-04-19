using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
    private static Inventory _instance;
    [SerializeField] private List<ItemSO> items = new();

    public static Inventory Instance { get { return _instance; } }

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
        foreach (ItemSO invItem in items)
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
