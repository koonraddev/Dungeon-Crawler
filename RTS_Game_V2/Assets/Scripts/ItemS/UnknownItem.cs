using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownItem : Item, IUsable
{
    [SerializeField] private UnknownItemSO unknownItemSO;
    private ItemInformationsSO itemInfos;
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

    public UnknownItem(UnknownItemSO unknownItemSO) : base(unknownItemSO.ItemInformations, unknownItemSO.ItemID)
    {
        this.unknownItemSO = unknownItemSO;
        itemInfos = unknownItemSO.ItemInformations;
        itemID = unknownItemSO.ItemID;
        itemName = itemInfos.ItemName;
        itemDescription = itemInfos.ItemDescription;
        itemSprite = itemInfos.ItemSprite;
    }

    public bool Use()
    {
        return unknownItemSO.InspectItem();
    }
}
