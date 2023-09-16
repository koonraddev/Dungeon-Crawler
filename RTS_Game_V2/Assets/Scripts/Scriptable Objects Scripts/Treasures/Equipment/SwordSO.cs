using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSword", menuName = "Scriptable Objects/Treasure/Right Hand/Sword")]
public class SwordSO : TreasureSO, IEquipmentItem
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private Sprite swordThumbnail;
    [SerializeField] private ItemStatisticsSO itemStatisticsSO;
    private ItemSlotType slotType = ItemSlotType.RIGHT_HAND;

    public ItemSlotType ItemSlotType { get => slotType; }
    public string NameText { get => nameText; }
    public string Description { get => description; }
    public Sprite EquipmentThumbnail { get => swordThumbnail; }
    public Dictionary<StatisticType, float> Statistics { get => itemStatisticsSO.Statistics; }

    public override string GetName()
    {
        return nameText;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override Sprite GetThumbnail()
    {
        return swordThumbnail;
    }
}
