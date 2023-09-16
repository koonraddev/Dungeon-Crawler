using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBoots", menuName = "Scriptable Objects/Treasure/Feets/Boots")]
public class BootsSO : TreasureSO, IEquipmentItem
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private Sprite bootsThumbnail;
    [SerializeField] private ItemStatisticsSO itemStatisticsSO;
    private ItemSlotType slotType = ItemSlotType.FEETS;

    public ItemSlotType ItemSlotType { get => slotType; }
    public string NameText { get => nameText; }
    public string Description { get => description; }
    public Sprite EquipmentThumbnail { get => bootsThumbnail; }
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
        return bootsThumbnail;
    }
}
