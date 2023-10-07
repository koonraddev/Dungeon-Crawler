using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : Item
{
    public override int ID
    {
        get { return itemID; }
        set { itemID = value; }
    }

    public override string Name
    {
        get { return itemName; }
        set { itemName = value; }
    }

    public override string Description
    {
        get { return itemDescription; }
        set { itemDescription = value; }
    }
    public override Sprite Sprite
    {
        get { return itemSprite; }
        set { itemSprite = value; }
    }

    public InventoryItem(ItemInformationsSO itemInformationsSO, int id) : base(itemInformationsSO, id)
    {
        itemName = itemInformationsSO.ItemName;
        itemDescription = itemInformationsSO.ItemDescription;
        itemSprite = itemInformationsSO.ItemSprite;
    }

}
