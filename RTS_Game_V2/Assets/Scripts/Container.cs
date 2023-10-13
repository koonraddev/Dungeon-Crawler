using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Container
{
    private List<ContainerSlot> contSlots;
    private string contName;
    private string contDesc;
    private bool canAddTreasures;

    public string Name { get => contName;}
    public string Description { get => contDesc; }
    public List<ContainerSlot> Slots { get => contSlots;}

    public Container(List<ContainerSlot> containerSlots, string containerName, string containerDescription, bool canAddTreasures = true)
    {
        contSlots = containerSlots;
        contName = containerName;
        contDesc = containerDescription;
        this.canAddTreasures = canAddTreasures;
    }

    public void RemoveTreasure(int slotIndex)
    {
        contSlots[slotIndex].Item = null;
        contSlots[slotIndex].Amount = 0;
        contSlots[slotIndex].Empty = true;
        GameEvents.instance.ContainerUpdate();
    }

    public bool AddTreasure(int slotIndex, Item treasureToAdd, int amount = 1)
    {
        if(contSlots[slotIndex].Empty && canAddTreasures)
        {
            contSlots[slotIndex].Item = treasureToAdd;
            contSlots[slotIndex].Amount = amount;
            contSlots[slotIndex].Empty = false;
            GameEvents.instance.ContainerUpdate();
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
        GameEvents.instance.ContainerUpdate();
    }
}
