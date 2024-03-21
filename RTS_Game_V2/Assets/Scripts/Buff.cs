using UnityEngine;

[System.Serializable]
public class Buff
{
    [SerializeField] private float timeleft;
    [SerializeField] private float statValue;
    [SerializeField] private StatisticType statType;
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