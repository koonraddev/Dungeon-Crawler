using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    private StatisticsSet baseStatsSet = new();
    private StatisticsSet eqStatsSet = new();
    private StatisticsSet buffStatsSet = new();
    private StatisticsSet totalStatsSet = new();


    void Start()
    {
        ResetStatisticsAndSetBasics();
        SetStatisticsFromEquipment();
    }

    private void OnEnable()
    {
        GameEvents.instance.OnEquipmentUpdate += SetStatisticsFromEquipment;
        GameEvents.instance.OnBuffActivate += ActivateBuff;
        GameEvents.instance.OnBuffDeactivate += DeactivateBuff;
        GameEvents.instance.OnLoadedPlayerData += ResetStatisticsAndSetBasics;

    }

    private void ResetStatisticsAndSetBasics()
    {
        baseStatsSet.Reset();
        StatisticsSet playerBaseStats = BuffManager.instance.PlayerBasicStatistics;
        baseStatsSet = StatisticsSet.SummarizeSets(playerBaseStats);
        UpdateStats();
    }

    private void SetStatisticsFromEquipment()
    {
        eqStatsSet.Reset();

        foreach (EquipmentSlotType slotType in System.Enum.GetValues(typeof(EquipmentSlotType)))
        {
            Dictionary<StatisticType, float> stats = EquipmentManager.instance.GetStatistics(slotType);
            if(stats != null && stats.Count > 0)
            {
                foreach (KeyValuePair<StatisticType, float> oneStat in stats)
                {
                    eqStatsSet.IncreaseStatisticValue(oneStat.Key, oneStat.Value);
                }
            }   
        }

        UpdateStats();
    }

    public void ActivateBuff(StatisticType buffType, float buffValue, float duration)
    {
        buffStatsSet.IncreaseStatisticValue(buffType, buffValue);
        UpdateStats();
    }

    public void DeactivateBuff(StatisticType buffType, float value)
    {
        buffStatsSet.DecreaseStatisticValue(buffType, value);
        UpdateStats();
    }

    public void UpdateStats()
    {
        StatisticsSet summarizedSet = StatisticsSet.SummarizeSets(baseStatsSet, eqStatsSet, buffStatsSet);
        foreach (StatisticType statType in System.Enum.GetValues(typeof(StatisticType)))
        {
            float totalValue = totalStatsSet.GetValue(statType);
            float summarizeValue = summarizedSet.GetValue(statType);

            if(totalValue != summarizeValue)
            {
                GameEvents.instance.StatisticUpdate(statType, summarizeValue);
                totalStatsSet.SetStatisticValue(statType, summarizeValue);
            }
        }
    }

    void OnDisable()
    {
        GameEvents.instance.OnEquipmentUpdate -= SetStatisticsFromEquipment;
        GameEvents.instance.OnBuffActivate -= ActivateBuff;
        GameEvents.instance.OnBuffDeactivate -= DeactivateBuff;
        GameEvents.instance.OnLoadedPlayerData -= ResetStatisticsAndSetBasics;
    }
}