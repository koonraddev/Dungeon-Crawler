using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [SerializeField] private StatisticsSO baseStats;

    //BASE
    private float base_maxHealth;
    private float base_movementSpeed;
    private float base_armor;
    private float base_magicResistance;
    private float base_attackSpeed;
    private float base_attackRange;
    private float base_physicalDamage;
    private float base_magicDamage;
    private float base_trueDamage;
    private float base_healthPointsRegen;
    private float base_healthPercentsRegen;

    //EQ
    private float eq_maxHealth;
    private float eq_movementSpeed;
    private float eq_armor;
    private float eq_magicResistance;
    private float eq_attackSpeed;
    private float eq_attackRange;
    private float eq_physicalDamage;
    private float eq_magicDamage;
    private float eq_trueDamage;
    private float eq_healthPointsRegen;
    private float eq_healthPercentsRegen;

    //TOTAL
    private float maxHealth;
    private float movementSpeed;
    private float armor;
    private float magicResistance;
    private float attackSpeed;
    private float attackRange;
    private float physicalDamage;
    private float magicDamage;
    private float trueDamage;
    private float healthPointsRegen;// health regeneration: POINTS (constant) per minute
    private float healthPercentsRegen;// health regeneration: PERCENTAGES (of max health) per minute

    private float healthRegeneration;// health(Points) per minute (totality)

    //public float MaxHealth { get => maxHealth; }
    //public float MovementSpeed { get => movementSpeed; }
    //public float Armor { get => armor; }
    //public float MagicResistance { get => magicResistance; }
    //public float AttackSpeed { get => attackSpeed; }
    //public float AttackRange { get => attackRange; }
    //public float PhysicalDamage { get => physicalDamage; }
    //public float MagicDamage { get => magicDamage; }
    //public float TrueDamage { get => trueDamage; }
    //public float HealthRegeneration { get => healthRegeneration; }
    //public float HealthPointsRegen { get => healthPointsRegen; }
    //public float HealthPercentsRegen { get => healthPercentsRegen; }

    void Start()
    {
        SetBasicStatistics();
        SetStatisticsFromEquipment();

    }

    private void OnEnable()
    {
        GameEvents.instance.OnEquipmentUpdate += OnEquipmentUpdate;
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
            base_trueDamage = baseStats.TrueDamage;
            base_healthPointsRegen = baseStats.HealthPointsRegen;
            base_healthPercentsRegen = baseStats.HealthPercentsRegen;

            maxHealth = base_maxHealth;
            movementSpeed = base_movementSpeed;
            armor = base_armor;
            magicResistance = base_magicResistance;
            attackSpeed = base_attackSpeed;
            attackRange = base_attackRange;
            physicalDamage = base_physicalDamage;
            magicDamage = base_magicDamage;
            trueDamage = base_trueDamage;
            healthPointsRegen = base_healthPointsRegen;
            healthPercentsRegen = base_healthPercentsRegen;
        }

        GameEvents.instance.StatisticUpdate(StatisticType.MaxHealth, base_maxHealth);
        GameEvents.instance.StatisticUpdate(StatisticType.MovementSpeed, base_movementSpeed);
        GameEvents.instance.StatisticUpdate(StatisticType.Armor, armor);
        GameEvents.instance.StatisticUpdate(StatisticType.MagicResistance, base_magicResistance);
        GameEvents.instance.StatisticUpdate(StatisticType.AttackSpeed, base_attackSpeed);
        GameEvents.instance.StatisticUpdate(StatisticType.AttackRange, base_attackRange);
        GameEvents.instance.StatisticUpdate(StatisticType.PhysicalDamage, base_physicalDamage);
        GameEvents.instance.StatisticUpdate(StatisticType.MagicDamage, base_magicDamage);
        GameEvents.instance.StatisticUpdate(StatisticType.TrueDamage, base_trueDamage);
        GameEvents.instance.StatisticUpdate(StatisticType.HealthPointsRegeneration, base_healthPointsRegen);
        GameEvents.instance.StatisticUpdate(StatisticType.HealthPercentageRegeneration, base_healthPercentsRegen);
        GameEvents.instance.UpdateCurrentHP(base_maxHealth);
    }


    private void SetStatisticsFromEquipment()
    {
        float newMaxHealth = 0;
        float newMovementSpeed = 0;
        float newArmor = 0;
        float newMagicResist = 0;
        float newPhysicalDmg = 0;
        float newMagicDmg = 0;
        float newTrueDmg = 0;
        float newHealthPointsRegen = 0;
        float newHealtPercentageRegen = 0;
        float newAttackSpeed = 0;
        float newAttackRange = 0;

        foreach (EquipmentSlotType slotType in System.Enum.GetValues(typeof(EquipmentSlotType)))
        {
            Dictionary<StatisticType, float> stats = EquipmentManager.instance.GetStatistics(slotType);
            if(stats != null && stats.Count > 0)
            {
                foreach (KeyValuePair<StatisticType, float> oneStat in stats)
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
                        case StatisticType.TrueDamage:
                            newTrueDmg += oneStat.Value;
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
        }

        eq_maxHealth = newMaxHealth;
        eq_movementSpeed = newMovementSpeed;
        eq_armor = newArmor;
        eq_magicResistance = newMagicResist;
        eq_attackSpeed = newAttackSpeed;
        eq_attackRange = newAttackRange;
        eq_physicalDamage = newPhysicalDmg;
        eq_magicDamage = newMagicDmg;
        eq_trueDamage = newTrueDmg;
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
        if(maxHealth != base_maxHealth + eq_maxHealth)
        {
            Debug.Log("update maxHHealth");
            maxHealth = base_maxHealth + eq_maxHealth;
            GameEvents.instance.StatisticUpdate(StatisticType.MaxHealth, maxHealth);
        }
        if (movementSpeed != base_movementSpeed + eq_movementSpeed)
        {
            movementSpeed = base_movementSpeed + eq_movementSpeed;
            GameEvents.instance.StatisticUpdate(StatisticType.MovementSpeed, movementSpeed);
        }

        if (armor != base_armor + eq_armor)
        {
            Debug.Log("update armor");
            armor = base_armor + eq_armor;
            GameEvents.instance.StatisticUpdate(StatisticType.Armor, armor);
        }

        if (magicResistance != base_magicResistance + eq_magicResistance)
        {
            magicResistance = base_magicResistance + eq_magicResistance;
            GameEvents.instance.StatisticUpdate(StatisticType.MagicResistance, magicResistance);
        }
        if (attackSpeed != base_attackSpeed + eq_attackSpeed)
        {
            attackSpeed = base_attackSpeed + eq_attackSpeed;
            GameEvents.instance.StatisticUpdate(StatisticType.AttackSpeed, attackSpeed);
        }

        if (attackRange != base_attackRange + eq_attackRange)
        {
            attackRange = base_attackRange + eq_attackRange;
            GameEvents.instance.StatisticUpdate(StatisticType.AttackRange, attackRange);
        }

        if (physicalDamage != base_physicalDamage + eq_physicalDamage)
        {
            physicalDamage = base_physicalDamage + eq_physicalDamage;
            GameEvents.instance.StatisticUpdate(StatisticType.PhysicalDamage, physicalDamage);
        }

        if (magicDamage != base_magicDamage + eq_magicDamage)
        {
            magicDamage = base_magicDamage + eq_magicDamage;
            GameEvents.instance.StatisticUpdate(StatisticType.MagicDamage, magicDamage);
        }

        if (trueDamage != base_trueDamage + eq_trueDamage)
        {
            trueDamage = base_trueDamage + eq_trueDamage;
            GameEvents.instance.StatisticUpdate(StatisticType.TrueDamage, trueDamage);
        }

        if (healthPointsRegen != base_healthPointsRegen + eq_healthPointsRegen)
        {
            healthPointsRegen = base_healthPointsRegen + eq_healthPointsRegen;
            GameEvents.instance.StatisticUpdate(StatisticType.HealthPointsRegeneration, healthPointsRegen);
        }

        if (healthPercentsRegen != base_healthPercentsRegen + eq_healthPercentsRegen)
        {
            healthPercentsRegen = base_healthPercentsRegen + eq_healthPercentsRegen;
            GameEvents.instance.StatisticUpdate(StatisticType.HealthPercentageRegeneration, healthPercentsRegen);
        }

        //if (healthRegeneration != healthPointsRegen + (healthPercentsRegen * maxHealth))
        //{
        //    healthRegeneration = healthPointsRegen + (healthPercentsRegen * maxHealth);
        //    //GameEvents.instance.StatisticUpdate(StatisticType.HealthPointsRegeneration, healthRegeneration);
        //}
    }
    void OnDisable()
    {
        GameEvents.instance.OnEquipmentUpdate -= OnEquipmentUpdate;
    }
}