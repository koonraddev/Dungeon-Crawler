using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentItem : Item , IStatisticItem
{
    [SerializeField] private EquipmentSlotType itemSlot;
    [SerializeField] private EquipmentItemSO equipmentItemSO;
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
    public EquipmentSlotType ItemSlot { get => itemSlot; }

    public Dictionary<StatisticType, float> Statistics => statistics;

    public EquipmentItem(EquipmentItemSO equipmentItemSO): base(equipmentItemSO.ItemInformations, equipmentItemSO.ItemID)
    {
        this.equipmentItemSO = equipmentItemSO;
        itemSlot = equipmentItemSO.ItemSlot;
        statistics = equipmentItemSO.ItemStatistics.Statistics;
        itemInfos = equipmentItemSO.ItemInformations;
        itemID = equipmentItemSO.ItemID;
        itemName = itemInfos.ItemName;
        itemDescription = itemInfos.ItemDescription;
        itemSprite = itemInfos.ItemSprite;
    }
}
