using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveItem : InventoryItem
{
    private bool isReusable;
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

    public bool IsReusable
    {
        get { return isReusable; }
        set { isReusable = value; }
    }

    public PassiveItem(PassiveItemSO passiveItemSO) : base(passiveItemSO.ItemInformations, passiveItemSO.ItemID)
    {
        itemID = passiveItemSO.ItemID;
        itemName = passiveItemSO.ItemInformations.ItemName;
        itemDescription = passiveItemSO.ItemInformations.ItemDescription;
        itemSprite = passiveItemSO.ItemInformations.ItemSprite;
        isReusable = passiveItemSO.IsReusable;
    }
}
