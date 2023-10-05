using UnityEngine;

[System.Serializable]
public class EquipmentItem : Item
{
    [SerializeField] private EquipmentSlotType itemSlot;
    [SerializeField] private EquipmentItemSO equipmentItemSO;
    private StatisticsSO itemStatistics;
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
    public EquipmentSlotType ItemSlot { get => itemSlot; }
    public StatisticsSO ItemStatistics { get => itemStatistics; }
    public EquipmentItem(EquipmentItemSO equipmentItemSO): base(equipmentItemSO.ItemInformations)
    {
        this.equipmentItemSO = equipmentItemSO;
        itemSlot = equipmentItemSO.ItemSlot;
        itemStatistics = equipmentItemSO.ItemStatistics;
        itemInfos = equipmentItemSO.ItemInformations;
        itemID = itemInfos.ItemID;
        itemName = itemInfos.ItemName;
        itemDescription = itemInfos.ItemDescription;
        itemSprite = itemInfos.ItemSprite;
    }
}
