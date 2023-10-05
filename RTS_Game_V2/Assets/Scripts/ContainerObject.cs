using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContainerObject
{
    public List<ContainerSlot> contSlots;
    public string contName;
    public string contDesc;

    public ContainerObject(List<ContainerSlot> containerSlots, string containerName, string containerDescription)
    {
        contSlots = containerSlots;
        contName = containerName;
        contDesc = containerDescription;
    }

    public void RemoveTreasure(int slotIndex)
    {
        contSlots[slotIndex].Item = null;
        contSlots[slotIndex].Amount = 0;
        contSlots[slotIndex].Empty = true;
        GameEvents.instance.ChestUpdate();
    }

    public bool AddTreasure(int slotIndex, Item treasureToAdd, int amount = 1)
    {
        if(contSlots[slotIndex].Empty)
        {
            contSlots[slotIndex].Item = treasureToAdd;
            contSlots[slotIndex].Amount = amount;
            contSlots[slotIndex].Empty = false;
            GameEvents.instance.ChestUpdate();
            return true;
        }
        return false;
    }

    public void SwapItems(int slotIndexA, int slotIndexB)
    {    
        Debug.Log("Swap1");
        ContainerSlot tmp = new(contSlots[slotIndexA]);

        //Debug.Log(tmp.Empty);
        //Debug.Log(tmp.Item);
        //Debug.Log(tmp.Amount);

        //Debug.Log("-----From-------");
        //Debug.Log(contSlots[slotIndexA].Empty);
        //Debug.Log(contSlots[slotIndexA].Item);
        //Debug.Log(contSlots[slotIndexA].Amount);
        //Debug.Log("-----TO-------");
        //Debug.Log(contSlots[slotIndexB].Empty);
        //Debug.Log(contSlots[slotIndexB].Item);
        //Debug.Log(contSlots[slotIndexB].Amount);

        contSlots[slotIndexA].Empty = contSlots[slotIndexB].Empty;
        contSlots[slotIndexA].Item = contSlots[slotIndexB].Item;
        contSlots[slotIndexA].Amount = contSlots[slotIndexB].Amount;


        contSlots[slotIndexB].Empty = tmp.Empty;
        contSlots[slotIndexB].Item = tmp.Item;
        contSlots[slotIndexB].Amount = tmp.Amount;


        //Debug.Log("---------@@@@@@@@@@@@@@@@@-------AFTER");

        //Debug.Log(contSlots[slotIndexA].Empty);
        //Debug.Log(contSlots[slotIndexA].Item);
        //Debug.Log(contSlots[slotIndexA].Amount);
        //Debug.Log("-----TO-------");
        //Debug.Log(contSlots[slotIndexB].Empty);
        //Debug.Log(contSlots[slotIndexB].Item);
        //Debug.Log(contSlots[slotIndexB].Amount);
        GameEvents.instance.ChestUpdate();
    }
}
