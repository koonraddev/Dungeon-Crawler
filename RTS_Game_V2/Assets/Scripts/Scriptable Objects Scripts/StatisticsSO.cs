using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newStatistics", menuName = "Scriptable Objects/Statistics")]
public class StatisticsSO : ScriptableObject, ISerializationCallbackReceiver
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
    [SerializeField] private float movementSpeed;

    [SerializeField] [HideInInspector] private List<StatisticType> statTypes;
    [SerializeField] [HideInInspector] private List<float> statValues;

    private Dictionary<StatisticType, float> statistics;
    public Dictionary<StatisticType, float> Statistics { get => statistics; }

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

    private void OnValidate()
    {
        if (statistics == null)
        {
            statistics = new Dictionary<StatisticType, float>();
        }

        if (!statistics.ContainsKey(StatisticType.PhysicalDamage)) statistics.Add(StatisticType.PhysicalDamage, physicalDamage);
        else statistics[StatisticType.PhysicalDamage] = physicalDamage;

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

        if (!statistics.ContainsKey(StatisticType.MovementSpeed)) statistics.Add(StatisticType.MovementSpeed, movementSpeed);
        else statistics[StatisticType.MovementSpeed] = movementSpeed;
    }

    public void OnBeforeSerialize()
    {
        if (statTypes == null)
        {
            statTypes = new();
        }

        if (statValues == null)
        {
            statValues = new();
        }

        if (statistics == null)
        {
            statistics = new();
        }

        statTypes.Clear();
        statValues.Clear();

        foreach (var item in statistics)
        {
            statTypes.Add(item.Key);
            statValues.Add(item.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        statistics = new Dictionary<StatisticType, float>();

        for (int i = 0; i < Mathf.Min(statTypes.Count, statValues.Count); i++)
        {
            statistics.Add(statTypes[i], statValues[i]);
        }
    }
}