using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "newChest", menuName = "Scriptable Objects/Chest", order = 1)]
public class ChestSO : ScriptableObject
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private int chestId;
    [SerializeField] List<TreasureSO> treasureList;

    private void OnValidate()
    {
        if(treasureList.Count > 25)
        {
            int slotsToCreate = treasureList.Count - 25;
            for (int i = 0; i < slotsToCreate; i++)
            {
                treasureList.RemoveAt(treasureList.Count - 1);
            }
        }
        else if(treasureList.Count < 25)
        {
            int slotsToCreate = 25 - treasureList.Count;
            for (int i = 0; i < slotsToCreate; i++)
            {
                treasureList.Add(null);
            }
        }

    }

    private void Awake()
    {
        treasureList = new();
        {
            for (int i = 0; i < 25; i++)
            {
                treasureList.Add(null);
            }
        }
    }
    
    public int GetId()
    {
        return chestId;
    }
    public  string GetNameText()
    {
        return nameText;
    }
    public string GetDescription()
    {
        return description;
    }
    public  List<TreasureSO> GetTreasure()
    {
        return treasureList;
    }

    public void RemoveTreasure(int slotIndex)
    {
        treasureList[slotIndex] = null;
        GameEvents.instance.ChestUpdate();
    }

    public bool AddTreasure(int slotIndex, TreasureSO treasureToAdd)
    {
        if(treasureList[slotIndex] == null)
        {
            treasureList[slotIndex] = treasureToAdd;
            GameEvents.instance.ChestUpdate();
            return true;
        }
        return false;
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        TreasureSO requestingSlot = treasureList[fromIndex];
        TreasureSO slotB = treasureList[toIndex];

        if (requestingSlot != null)
        {
            treasureList[fromIndex] = slotB;
            treasureList[toIndex] = requestingSlot;
        }
        GameEvents.instance.ChestUpdate();
    }
}
