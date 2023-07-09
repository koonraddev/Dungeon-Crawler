using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "newChest", menuName = "Scriptable Objects/Chest", order = 1)]
public class ChestSO : ScriptableObject
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private int chestId;
    [SerializeField] List<TreasureSO> treasureList;

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

    public void RemoveTreasure(TreasureSO treasureToRemove)
    {
        treasureList.Remove(treasureToRemove);
        GameEvents.instance.ChestUpdate();
    }

    public void AddTreasure(TreasureSO treasureToAdd)
    {
        treasureList.Add(treasureToAdd);
        GameEvents.instance.ChestUpdate();
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
