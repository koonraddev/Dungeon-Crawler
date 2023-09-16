using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newArmor", menuName = "Scriptable Objects/Treasure/Body/Armor")]
public class ArmorSO : TreasureSO, IEquipmentItem
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private Sprite armorThumbnail;
    [SerializeField] private ItemStatisticsSO itemStatisticsSO;
    private ItemSlotType slotType = ItemSlotType.BODY;

    public ItemSlotType ItemSlotType { get => slotType; }
    public string NameText { get => nameText; }
    public string Description { get => description; }
    public Sprite EquipmentThumbnail { get => armorThumbnail; }
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
        return armorThumbnail;
    }
}
