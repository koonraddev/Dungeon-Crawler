using System.Collections.Generic;

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

    public bool Empty
    {
        get
        {
            return contSlots.Count > 0;
        }
    }

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
        ContainerSlot tmp = new(contSlots[slotIndexA]);

        contSlots[slotIndexA].Empty = contSlots[slotIndexB].Empty;
        contSlots[slotIndexA].Item = contSlots[slotIndexB].Item;
        contSlots[slotIndexA].Amount = contSlots[slotIndexB].Amount;


        contSlots[slotIndexB].Empty = tmp.Empty;
        contSlots[slotIndexB].Item = tmp.Item;
        contSlots[slotIndexB].Amount = tmp.Amount;
        GameEvents.instance.ContainerUpdate();
    }
}
