using UnityEngine;
using System.Collections.Generic;

public interface IEquipmentItem
{
    public ItemSlotType ItemSlotType { get; }
    public string NameText { get;}
    public string Description { get; }
    public Sprite EquipmentThumbnail { get; }
    public Dictionary<StatisticType, float> Statistics { get; }
}