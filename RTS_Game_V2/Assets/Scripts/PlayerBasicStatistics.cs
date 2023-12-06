using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBasicStatistics
{
    [SerializeField] private float physicalDamage;
    [SerializeField] private float trueDamage;
    [SerializeField] private float magicDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthPointsRegeneration;
    [SerializeField] private float healthPercentageRegeneration;
    [SerializeField] private float movementSpeed;

    public float MaxHealth { get => maxHealth; }
    public float Armor { get => armor; }
    public float MagicResistance { get => magicResistance; }
    public float AttackSpeed { get => attackSpeed; }
    public float AttackRange { get => attackRange; }
    public float PhysicalDamage { get => physicalDamage; }
    public float MagicDamage { get => magicDamage; }
    public float TrueDamage { get => trueDamage; }
    public float HealthPointsRegen { get => healthPointsRegeneration; }
    public float HealthPercentsRegen { get => healthPercentageRegeneration; }
    public float MovementSpeed { get => movementSpeed; }
    public Dictionary<StatisticType, float> Statistics 
    {
        get
        {
            Dictionary<StatisticType, float> statistics;
            statistics = new();
            statistics.Add(StatisticType.PhysicalDamage, physicalDamage);
            statistics.Add(StatisticType.TrueDamage, trueDamage);
            statistics.Add(StatisticType.MagicDamage, magicDamage);
            statistics.Add(StatisticType.AttackSpeed, attackSpeed);
            statistics.Add(StatisticType.AttackRange, attackRange);
            statistics.Add(StatisticType.Armor, armor);
            statistics.Add(StatisticType.MagicResistance, magicResistance);
            statistics.Add(StatisticType.MaxHealth, maxHealth);
            statistics.Add(StatisticType.HealthPointsRegeneration, healthPointsRegeneration);
            statistics.Add(StatisticType.HealthPercentageRegeneration, healthPercentageRegeneration);
            statistics.Add(StatisticType.MovementSpeed, movementSpeed);
            return statistics;
        }
    }

    public PlayerBasicStatistics(StatisticsSO statistics)
    {
        physicalDamage = statistics.PhysicalDamage;
        trueDamage = statistics.TrueDamage;
        magicDamage = statistics.MagicDamage;
        attackSpeed = statistics.AttackSpeed;
        attackRange = statistics.AttackRange;
        armor = statistics.Armor;
        magicResistance = statistics.MagicResistance;
        maxHealth = statistics.MaxHealth;
        healthPointsRegeneration = statistics.HealthPointsRegen;
        healthPercentageRegeneration = statistics.HealthPercentsRegen;
        movementSpeed = statistics.MovementSpeed;
    }

    public PlayerBasicStatistics(float physicalDamage, float trueDamage, float magicDamage, float attackSpeed, float attackRange, float armor, float magicResistance, float maxHealth, float healthPointsRegeneration, float healthPercentageRegeneration, float movementSpeed)
    {
        this.physicalDamage = physicalDamage;
        this.trueDamage = trueDamage;
        this.magicDamage = magicDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.armor = armor;
        this.magicResistance = magicResistance;
        this.maxHealth = maxHealth;
        this.healthPointsRegeneration = healthPointsRegeneration;
        this.healthPercentageRegeneration = healthPercentageRegeneration;
        this.movementSpeed = movementSpeed;
    }

    public PlayerBasicStatistics(List<StatisticCreator> statCreatorsList)
    {
        foreach (var item in statCreatorsList)
        {
            switch (item.StatisticType)
            {
                case StatisticType.MaxHealth:
                    maxHealth = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.MovementSpeed:
                    movementSpeed = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.HealthPointsRegeneration:
                    healthPointsRegeneration = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.HealthPercentageRegeneration:
                    healthPercentageRegeneration = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.Armor:
                    armor = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.MagicResistance:
                    magicResistance = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.PhysicalDamage:
                    physicalDamage = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.MagicDamage:
                    magicDamage = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.TrueDamage:
                    trueDamage = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.AttackSpeed:
                    attackSpeed = item.BaseStatisticValue + item.AddedValue;
                    break;
                case StatisticType.AttackRange:
                    attackRange = item.BaseStatisticValue + item.AddedValue;
                    break;
                default:
                    break;
            }
        }
    }
}
