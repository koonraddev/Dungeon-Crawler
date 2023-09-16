using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGreaves", menuName = "Scriptable Objects/Treasure/Legs/Greaves ")]
public class GreavesSO : TreasureSO, IEquipmentItem
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private Sprite greavesThumbnail;
    [SerializeField] private ItemStatisticsSO itemStatisticsSO;
    private ItemSlotType slotType = ItemSlotType.LEGS;

    public ItemSlotType ItemSlotType { get => slotType; }
    public string NameText { get => nameText; }
    public string Description { get => description; }
    public Sprite EquipmentThumbnail { get => greavesThumbnail; }
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
        return greavesThumbnail;
    }
}
