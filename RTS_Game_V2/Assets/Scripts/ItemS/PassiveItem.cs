using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : InventoryItem
{
    [SerializeField] private PassiveItemSO passiveItemSO;
    private ItemInformationsSO itemInfos;
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
        this.passiveItemSO = passiveItemSO;
        itemInfos = passiveItemSO.ItemInformations;
        itemID = passiveItemSO.ItemID;
        itemName = itemInfos.ItemName;
        itemDescription = itemInfos.ItemDescription;
        itemSprite = itemInfos.ItemSprite;
        isReusable = passiveItemSO.IsReusable;
    }
}
