using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UsableItem : InventoryItem, IUsable, IStatisticItem, ISerializationCallbackReceiver
{
    [SerializeField] private UsableItemSO usableItemSO;
    private Dictionary<StatisticType, float> statistics;
    [SerializeField] private List<StatisticType> statTypes;
    [SerializeField] private List<float> statValues;
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
        itemID = usableItemSO.ItemID;
        itemName = usableItemSO.ItemInformations.ItemName;
        itemDescription = usableItemSO.ItemInformations.ItemDescription;
        itemSprite = usableItemSO.ItemInformations.ItemSprite;
        statistics = usableItemSO.ItemStatistics.Statistics;
        this.usableItemSO = usableItemSO;
        itemDescription += "\n Cooldown: " + usableItemSO.Cooldown;
    }

    public bool Use()
    {
        return usableItemSO.UseItem();
    }

    public void OnBeforeSerialize()
    {
        if (statTypes == null)
        {
            statTypes = new();
        }

        if (statValues == null)
        {
            statValues = new();
        }

        if (statistics == null)
        {
            statistics = new();
        }

        statTypes.Clear();
        statValues.Clear();

        foreach (var item in statistics)
        {
            statTypes.Add(item.Key);
            statValues.Add(item.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        statistics = new Dictionary<StatisticType, float>();

        for (int i = 0; i < Mathf.Min(statTypes.Count, statValues.Count); i++)
        {
            statistics.Add(statTypes[i], statValues[i]);
        }
    }
}
