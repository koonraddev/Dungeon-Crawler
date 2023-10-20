using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    private float timeleft;
    private float statValue;
    private StatisticType statType;
    public StatisticType StatType { get => statType; }
    public float StatValue { get => statValue; }
    public float TimeLeft { get => timeleft; }

    public void Update()
    {
        timeleft -= Time.deltaTime;
    }

    public Buff(StatisticType statType, float statValue, float duration)
    {
        timeleft = duration;
        this.statType = statType;
        this.statValue = statValue;
    }
}