using UnityEngine;

[System.Serializable]
public struct StatisticsSet
{
    [Header("Health section")]
    [Tooltip("Maximum health value that an object can have")]
    public float maxHealth;
    [Tooltip("Health points object heals per minute (VALUE/min) ")]
    public float healthPointsRegeneration;
    [Tooltip("Health points percentages object heals per minute (maxHealth * VALUE/min")]
    public float healthPercentagesRegeneration;

    [Space(10)]
    public float movementSpeed;

    [Header("Deff section")]
    [Tooltip("Armor reduce physical damages acording: Damage Multiplier = 100/(100 + Armor)")]
    public float armor;
    [Tooltip("Magic Resistance reduce magic damages acording: Damage Multiplier = 100/(100 + Magic Resistance)")]
    public float magicResistance;

    [Header("Attack section")]
    [Tooltip("Attacks per minute  (VALUE/min)")]
    public float attackSpeed;
    [Tooltip("Minimum Distance between target to attack")]
    public float attackRange;
    [Tooltip("Physical damage value is multiplied by Physical Damage multiplier ( value * pDM)")]
    public float physicalDamage;
    [Tooltip("Magic damage value is multiplied by Magic Damage multiplier ( value * mDM)")]
    public float magicDamage;
    [Tooltip("True damage ignores armor and magic resistance and deal pure damage to opponent")]
    public float trueDamage;


    public float GetValue(StatisticType statisticType)
    {
        return statisticType switch
        {
            StatisticType.MaxHealth => maxHealth,
            StatisticType.MovementSpeed => movementSpeed,
            StatisticType.HealthPointsRegeneration => healthPointsRegeneration,
            StatisticType.HealthPercentageRegeneration => healthPercentagesRegeneration,
            StatisticType.Armor => armor,
            StatisticType.MagicResistance => magicResistance,
            StatisticType.PhysicalDamage => physicalDamage,
            StatisticType.MagicDamage => magicDamage,
            StatisticType.TrueDamage => trueDamage,
            StatisticType.AttackSpeed => attackSpeed,
            StatisticType.AttackRange => attackRange,
            _ => 0,
        };
    }

    public void SetStatisticValue(StatisticType statisticType, float value)
    {
        DecreaseStatisticValue(statisticType, GetValue(statisticType));
        IncreaseStatisticValue(statisticType, value);
    }

    public void DecreaseStatisticValue(StatisticType statisticType, float value)
    {
        switch (statisticType)
        {
            case StatisticType.MaxHealth:
                maxHealth -= value;
                break;
            case StatisticType.MovementSpeed:
                movementSpeed -= value;
                break;
            case StatisticType.HealthPointsRegeneration:
                healthPointsRegeneration -= value;
                break;
            case StatisticType.HealthPercentageRegeneration:
                healthPercentagesRegeneration -= value;
                break;
            case StatisticType.Armor:
                armor -= value;
                break;
            case StatisticType.MagicResistance:
                magicResistance -= value;
                break;
            case StatisticType.PhysicalDamage:
                physicalDamage -= value;
                break;
            case StatisticType.MagicDamage:
                magicDamage -= value;
                break;
            case StatisticType.TrueDamage:
                trueDamage -= value;
                break;
            case StatisticType.AttackSpeed:
                attackSpeed -= value;
                break;
            case StatisticType.AttackRange:
                attackRange -= value;
                break;
            default:
                break;
        }
    }

    public void IncreaseStatisticValue(StatisticType statisticType, float value)
    {
        switch (statisticType)
        {
            case StatisticType.MaxHealth:
                maxHealth += value;
                break;
            case StatisticType.MovementSpeed:
                movementSpeed += value;
                break;
            case StatisticType.HealthPointsRegeneration:
                healthPointsRegeneration += value;
                break;
            case StatisticType.HealthPercentageRegeneration:
                healthPercentagesRegeneration += value;
                break;
            case StatisticType.Armor:
                armor += value;
                break;
            case StatisticType.MagicResistance:
                magicResistance += value;
                break;
            case StatisticType.PhysicalDamage:
                physicalDamage += value;
                break;
            case StatisticType.MagicDamage:
                magicDamage += value;
                break;
            case StatisticType.TrueDamage:
                trueDamage += value;
                break;
            case StatisticType.AttackSpeed:
                attackSpeed += value;
                break;
            case StatisticType.AttackRange:
                attackRange += value;
                break;
            default:
                break;
        }
    }


    public void Reset()
    {
        maxHealth = 0;
        movementSpeed = 0;
        armor = 0;
        magicResistance = 0;
        attackSpeed = 0;
        attackRange = 0;
        physicalDamage = 0;
        magicDamage = 0;
        trueDamage = 0;
        healthPointsRegeneration = 0;
        healthPercentagesRegeneration = 0;
    }

    public static StatisticsSet SummarizeSets(params StatisticsSet[] sets)
    {
        StatisticsSet newSet = new();

        foreach (var set in sets)
        {
            newSet.maxHealth += set.maxHealth;
            newSet.movementSpeed += set.movementSpeed;
            newSet.healthPointsRegeneration += set.healthPointsRegeneration;
            newSet.healthPercentagesRegeneration += set.healthPercentagesRegeneration;
            newSet.armor += set.armor;
            newSet.magicResistance += set.magicResistance;
            newSet.physicalDamage += set.physicalDamage;
            newSet.magicDamage += set.magicDamage;
            newSet.trueDamage += set.trueDamage;
            newSet.attackSpeed += set.attackSpeed;
            newSet.attackRange += set.attackRange;
        }

        return newSet;
    }

}
