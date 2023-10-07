using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : InventoryItem, IUsable, IStatisticItem
{
    [SerializeField] private UsableItemSO usableItemSO;
    private ItemInformationsSO itemInfos;
    private Dictionary<StatisticType, float> statistics;
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

    public Dictionary<StatisticType, float> Statistics => statistics;

    public UsableItem(UsableItemSO usableItemSO) : base(usableItemSO.ItemInformations, usableItemSO.ItemID)
    {
        this.usableItemSO = usableItemSO;
        itemInfos = usableItemSO.ItemInformations;
        statistics = usableItemSO.ItemStatistics.Statistics;
        itemID = usableItemSO.ItemID;
        itemName = itemInfos.ItemName;
        itemDescription = itemInfos.ItemDescription;

        if (usableItemSO.DurationMode)
        {
            itemDescription += "\n Duration: " + usableItemSO.Duration;
        }
        else
        {
            itemDescription += "\n Cooldown: " + usableItemSO.Cooldown;
        }

        itemSprite = itemInfos.ItemSprite;
    }

    public void Use()
    {
        usableItemSO.UseItem();
    }
}
