using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : InventoryItem, IUsable
{
    [SerializeField] private UsableItemSO usableItemSO;
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

    public UsableItem(UsableItemSO usableItemSO) : base(usableItemSO.ItemInformations)
    {
        this.usableItemSO = usableItemSO;
        itemInfos = usableItemSO.ItemInformations;
        itemID = itemInfos.ItemID;
        itemName = itemInfos.ItemName;
        itemDescription = itemInfos.ItemDescription;
        itemSprite = itemInfos.ItemSprite;
    }

    public void Use()
    {
        usableItemSO.UseItem();
    }
}
