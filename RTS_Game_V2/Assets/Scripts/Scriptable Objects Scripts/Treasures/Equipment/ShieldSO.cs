using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newShield", menuName = "Scriptable Objects/Treasure/Left Hand/Shield ")]
public class ShieldSO : TreasureSO, IEquipmentItem
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private Sprite shieldThumbnail;
    [SerializeField] private ItemStatisticsSO itemStatisticsSO;
    private ItemSlotType slotType = ItemSlotType.LEFT_HAND;

    public ItemSlotType ItemSlotType { get => slotType; }
    public string NameText { get => nameText; }
    public string Description { get => description; }
    public Sprite EquipmentThumbnail { get => shieldThumbnail; }
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
        return shieldThumbnail;
    }
}
