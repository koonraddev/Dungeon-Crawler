using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemStatistics", menuName = "Scriptable Objects/Item Statistics")]
public class ItemStatisticsSO : ScriptableObject
{
    [Header("Attack Section")]
    [SerializeField] private float physicalDamage;
    [SerializeField] private float trueDamage;
    [SerializeField] private float magicDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;

    [Header("Defence Section")]
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;

    [Header("Health Section")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthPointsRegeneration;
    [SerializeField] private float healthPercentageRegeneration;

    [Header("Movement Section")]
    [SerializeField] private float MovementSpeed;
    
    private Dictionary<StatisticType, float> statistics;
    public Dictionary<StatisticType, float> Statistics { get => statistics; }

    private void OnValidate()
    {
        if (statistics == null)
        {
            statistics = new Dictionary<StatisticType, float>();
        }

        if (!statistics.ContainsKey(StatisticType.TrueDamage)) statistics.Add(StatisticType.TrueDamage, trueDamage);
        else statistics[StatisticType.TrueDamage] = trueDamage;

        if (!statistics.ContainsKey(StatisticType.MagicDamage)) statistics.Add(StatisticType.MagicDamage, magicDamage);
        else statistics[StatisticType.MagicDamage] = magicDamage;

        if (!statistics.ContainsKey(StatisticType.AttackSpeed)) statistics.Add(StatisticType.AttackSpeed, attackSpeed);
        else statistics[StatisticType.AttackSpeed] = attackSpeed;

        if (!statistics.ContainsKey(StatisticType.AttackRange)) statistics.Add(StatisticType.AttackRange, attackRange);
        else statistics[StatisticType.AttackRange] = attackRange;

        if (!statistics.ContainsKey(StatisticType.Armor)) statistics.Add(StatisticType.Armor, armor);
        else statistics[StatisticType.Armor] = armor;

        if (!statistics.ContainsKey(StatisticType.MagicResistance)) statistics.Add(StatisticType.MagicResistance, magicResistance);
        else statistics[StatisticType.MagicResistance] = magicResistance;

        if (!statistics.ContainsKey(StatisticType.MaxHealth)) statistics.Add(StatisticType.MaxHealth, maxHealth);
        else statistics[StatisticType.MaxHealth] = maxHealth;

        if (!statistics.ContainsKey(StatisticType.HealthPointsRegeneration)) statistics.Add(StatisticType.HealthPointsRegeneration, healthPointsRegeneration);
        else statistics[StatisticType.HealthPointsRegeneration] = healthPointsRegeneration;

        if (!statistics.ContainsKey(StatisticType.HealthPercentageRegeneration)) statistics.Add(StatisticType.HealthPercentageRegeneration, healthPercentageRegeneration);
        else statistics[StatisticType.HealthPercentageRegeneration] = healthPercentageRegeneration;
    }
}
