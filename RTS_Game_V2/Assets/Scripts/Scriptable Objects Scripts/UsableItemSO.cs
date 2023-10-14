using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUsableItem", menuName = "Scriptable Objects/Items/Usable Item")]
public class UsableItemSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private ItemInformationsSO itemInfos;
    [SerializeField] private StatisticsSO itemStatistics;
    [SerializeField] private float cooldown;
    [SerializeField] private bool durationMode;
    [SerializeField] private float duration;
    public int ItemID { get => itemID; }
    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public StatisticsSO ItemStatistics { get => itemStatistics; }

    public bool DurationMode { get => durationMode; }
    public float Duration { get => duration; }
    public float Cooldown { get => cooldown; }
    public bool UseItem()
    {
        return BuffManager.instance.Buff(itemStatistics.Statistics, Duration);
    }
}
