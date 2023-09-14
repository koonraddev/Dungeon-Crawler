using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatisticType
{
    MaxHealth,
    MovementSpeed,
    HealthPointsRegeneration,
    HealthPercentageRegeneration,
    Armor,
    MagicResistance,
    PhysicalDamage,
    MagicDamage,
    AttackSpeed,
    AttackRange
}


public class PlayerStatistics : MonoBehaviour
{
    [SerializeField] private PlayerBasicStatisticsSO baseStats;
    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;
    private float health;
    private float healthRegeneration;// health(Points) per minute (totality)

    //BASE
    private float base_maxHealth;
    private float base_movementSpeed;
    private float base_armor;
    private float base_magicResistance;
    private float base_attackSpeed;
    private float base_attackRange;
    private float base_physicalDamage;
    private float base_magicDamage;
    private float base_healthPointsRegen;// health regeneration: POINTS (constant) per minute
    private float base_healthPercentsRegen;// health regeneration: PERCENTAGES (of max health) per minute

    //EQ
    private float eq_maxHealth;
    private float eq_movementSpeed;
    private float eq_armor;
    private float eq_magicResistance;
    private float eq_attackSpeed;
    private float eq_attackRange;
    private float eq_physicalDamage;
    private float eq_magicDamage;
    private float eq_healthPointsRegen;// health regeneration: POINTS (constant) per minute
    private float eq_healthPercentsRegen;// health regeneration: PERCENTAGES (of max health) per minute

    //TOTAL
    private float maxHealth;
    private float movementSpeed;
    private float armor;
    private float magicResistance;
    private float attackSpeed;
    private float attackRange;
    private float physicalDamage;
    private float magicDamage;
    private float healthPointsRegen;// health regeneration: POINTS (constant) per minute
    private float healthPercentsRegen;// health regeneration: PERCENTAGES (of max health) per minute

    public float MaxHealth { get => maxHealth; }
    public float MovementSpeed { get => movementSpeed; }
    public float Armor { get => armor; }
    public float MagicResistance { get => magicResistance; }
    public float AttackSpeed { get => attackSpeed; }
    public float AttackRange { get => attackRange; }
    public float PhysicalDamage { get => physicalDamage; }
    public float MagicDamage { get => magicDamage; }
    public float HealthPointsRegen { get => healthPointsRegen; }
    public float HealthPercentsRegen { get => healthPercentsRegen; }

    //[SerializeField] EquipmentManager playerEq;

    private float interval = 60f;
    private float timeLeft;

    private void Awake()
    {
        SetBasicStatistics();
        SetStatisticsFromEquipment();
        UpdateStats();
    }
    void Start()
    {
        GameEvents.instance.OnEquipmentUpdate += OnEquipmentUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            Heal(healthRegeneration);
            timeLeft = interval;
        }
    }

    private void SetBasicStatistics()
    {
        if (baseStats != null)
        {
            base_maxHealth = baseStats.MaxHealth;
            base_movementSpeed = baseStats.MovementSpeed;
            base_armor = baseStats.Armor;
            base_magicResistance = baseStats.MagicResistance;
            base_attackSpeed = baseStats.AttackSpeed;
            base_attackRange = baseStats.AttackRange;
            base_physicalDamage = baseStats.PhysicalDamage;
            base_magicDamage = baseStats.MagicDamage;
            base_healthPointsRegen = baseStats.HealthPointsRegen;
            base_healthPercentsRegen = baseStats.HealthPercentsRegen;
        }
    }


    private void SetStatisticsFromEquipment()
    {
        float newMaxHealth = 0;
        float newMovementSpeed = 0;
        float newArmor = 0;
        float newMagicResist = 0;
        float newPhysicalDmg = 0;
        float newMagicDmg = 0;
        float newHealthPointsRegen = 0;
        float newHealtPercentageRegen = 0;
        float newAttackSpeed = 0;
        float newAttackRange = 0;

        foreach (ItemSlotType slotType in System.Enum.GetValues(typeof(ItemSlotType)))
        {
            IDictionary stats = Equipment.Instance.GetStatistics(slotType);
            foreach (KeyValuePair<StatisticType,float> oneStat in stats)
            {
                switch (oneStat.Key)
                {
                    case StatisticType.MaxHealth:
                        newMaxHealth += oneStat.Value;
                        break;
                    case StatisticType.MovementSpeed:
                        newMovementSpeed += oneStat.Value;
                        break;
                    case StatisticType.HealthPointsRegeneration:
                        newHealthPointsRegen += oneStat.Value;
                        break;
                    case StatisticType.HealthPercentageRegeneration:
                        newHealtPercentageRegen += oneStat.Value;
                        break;
                    case StatisticType.Armor:
                        newArmor += oneStat.Value;
                        break;
                    case StatisticType.MagicResistance:
                        newMagicResist += oneStat.Value;
                        break;
                    case StatisticType.PhysicalDamage:
                        newPhysicalDmg += oneStat.Value;
                        break;
                    case StatisticType.MagicDamage:
                        newMagicDmg += oneStat.Value;
                        break;
                    case StatisticType.AttackSpeed:
                        newAttackSpeed += oneStat.Value;
                        break;
                    case StatisticType.AttackRange:
                        newAttackRange += oneStat.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        eq_maxHealth = newMaxHealth;
        eq_movementSpeed = newMovementSpeed;
        eq_armor = newArmor;
        eq_magicResistance = newMagicResist;
        eq_attackSpeed = newAttackSpeed;
        eq_attackRange = newAttackRange;
        eq_physicalDamage = newPhysicalDmg;
        eq_magicDamage = newMagicDmg;
        eq_healthPointsRegen = newHealthPointsRegen;
        eq_healthPercentsRegen = newHealtPercentageRegen;
    }
    public void OnEquipmentUpdate()
    {
        SetStatisticsFromEquipment();
        UpdateStats();
    }

    public void UpdateStats()
    {
        maxHealth = base_maxHealth + eq_maxHealth;
        movementSpeed = base_movementSpeed + eq_movementSpeed;
        armor = base_armor + eq_armor;
        magicResistance = base_magicResistance + eq_magicResistance;
        attackSpeed = base_attackSpeed + eq_attackSpeed;
        attackRange = base_attackRange + eq_attackRange;
        physicalDamage = base_physicalDamage + eq_physicalDamage;
        magicDamage = base_magicDamage + eq_magicDamage;
        healthPointsRegen = base_healthPointsRegen + eq_healthPointsRegen;
        healthPercentsRegen = base_healthPercentsRegen + eq_healthPercentsRegen;

        physicalDamageMultiplier = 100 / (100 - armor);
        magicDamageMultiplier = 100 / (100 - magicResistance);
        healthRegeneration = healthPointsRegen + (healthPercentsRegen * maxHealth);

        if(health > maxHealth)
        {
            Mathf.Clamp(health, 0, maxHealth);
        }

        GameEvents.instance.StatisticsUpdate();
    }

    public void Damage(float physicalDamage, float magicDamage, float trueDamage)
    {
        float totalDamage = physicalDamage * physicalDamageMultiplier + magicDamage * magicDamageMultiplier + trueDamage;
        Debug.Log("LOG. Attack damage gained: PH - " + physicalDamage * physicalDamageMultiplier + "; MA - " + magicDamage * magicDamageMultiplier + " TR - " + trueDamage);
        health -= totalDamage;
    }

    public void Damage(float healthPoints)
    {
        health -= healthPoints;
    }

    public void Heal(float healthPoints)
    {
        health += healthPoints;
        Mathf.Clamp(health, 0, maxHealth);
    }

    void OnDisable()
    {
        //GameEvents.instance.OnEquipmentUpdate -= UpdateStats;
    }
}