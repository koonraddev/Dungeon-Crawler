using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEquipmentItem", menuName = "Scriptable Objects/Items/Equipment Item")]
public class EquipmentItemSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private EquipmentSlotType itemSlot;
    [SerializeField] private ItemInformationsSO itemInfos;
    [SerializeField] private StatisticsSO itemStatistics;

    public int ItemID { get => itemID; }
    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public StatisticsSO ItemStatistics { get => itemStatistics; }
    public EquipmentSlotType ItemSlot { get => itemSlot; }
}
