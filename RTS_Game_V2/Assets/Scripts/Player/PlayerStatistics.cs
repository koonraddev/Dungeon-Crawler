using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    private PlayerBasicStatistics baseStats;

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
    
    //BUFF
    private float buff_maxHealth;
    private float buff_movementSpeed;
    private float buff_armor;
    private float buff_magicResistance;
    private float buff_attackSpeed;
    private float buff_attackRange;
    private float buff_physicalDamage;
    private float buff_magicDamage;
    private float buff_trueDamage;
    private float buff_healthPointsRegen;
    private float buff_healthPercentsRegen;

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
    private float healthPointsRegen;
    private float healthPercentsRegen;


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
        maxHealth = 0;
        movementSpeed = 0;
        armor = 0;
        magicResistance = 0;
        attackSpeed = 0;
        attackRange = 0;
        physicalDamage = 0;
        magicDamage = 0;
        trueDamage = 0;
        healthPointsRegen = 0;
        healthPercentsRegen = 0;


        baseStats = BuffManager.instance.PlayerBasicStatistics;
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
        }
        UpdateStats();
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
        UpdateStats();
    }

    public void ActivateBuff(StatisticType buffType, float buffValue, float duration)
    {
        switch (buffType)
        {
            case StatisticType.MaxHealth:
                buff_maxHealth += buffValue;
                break;
            case StatisticType.MovementSpeed:
                buff_movementSpeed += buffValue;
                break;
            case StatisticType.HealthPointsRegeneration:
                buff_healthPointsRegen += buffValue;
                break;
            case StatisticType.HealthPercentageRegeneration:
                buff_healthPercentsRegen += buffValue;
                break;
            case StatisticType.Armor:
                buff_armor += buffValue;
                break;
            case StatisticType.MagicResistance:
                buff_magicResistance += buffValue;
                break;
            case StatisticType.PhysicalDamage:
                buff_physicalDamage += buffValue;
                break;
            case StatisticType.MagicDamage:
                buff_magicDamage += buffValue;
                break;
            case StatisticType.TrueDamage:
                buff_trueDamage += buffValue;
                break;
            case StatisticType.AttackSpeed:
                buff_attackSpeed += buffValue;
                break;
            case StatisticType.AttackRange:
                buff_attackRange += buffValue;
                break;
            default:
                break;
        }

        UpdateStats();
    }

    public void DeactivateBuff(StatisticType buffType, float value)
    {
        switch (buffType)
        {
            case StatisticType.MaxHealth:
                buff_maxHealth -= value;
                break;
            case StatisticType.MovementSpeed:
                buff_movementSpeed -= value;
                break;
            case StatisticType.HealthPointsRegeneration:
                buff_healthPointsRegen -= value;
                break;
            case StatisticType.HealthPercentageRegeneration:
                buff_healthPercentsRegen -= value;
                break;
            case StatisticType.Armor:
                buff_armor -= value;
                break;
            case StatisticType.MagicResistance:
                buff_magicResistance -= value;
                break;
            case StatisticType.PhysicalDamage:
                buff_physicalDamage -= value;
                break;
            case StatisticType.MagicDamage:
                buff_magicDamage -= value;
                break;
            case StatisticType.TrueDamage:
                buff_trueDamage -= value;
                break;
            case StatisticType.AttackSpeed:
                buff_attackSpeed -= value;
                break;
            case StatisticType.AttackRange:
                buff_attackRange -= value;
                break;
            default:
                break;
        }

        UpdateStats();
    }

    public void UpdateStats()
    {
        if (maxHealth != (base_maxHealth + eq_maxHealth + buff_maxHealth))
        {
            maxHealth = (base_maxHealth + eq_maxHealth + buff_maxHealth);
            GameEvents.instance.StatisticUpdate(StatisticType.MaxHealth, maxHealth);
        }
        if (movementSpeed != (base_movementSpeed + eq_movementSpeed + buff_movementSpeed))
        {
            movementSpeed = (base_movementSpeed + eq_movementSpeed + buff_movementSpeed);
            GameEvents.instance.StatisticUpdate(StatisticType.MovementSpeed, movementSpeed);
        }

        if (armor != (base_armor + eq_armor + buff_armor))
        {
            armor = (base_armor + eq_armor + buff_armor);
            GameEvents.instance.StatisticUpdate(StatisticType.Armor, armor);
        }

        if (magicResistance != (base_magicResistance + eq_magicResistance + buff_magicResistance))
        {
            magicResistance = (base_magicResistance + eq_magicResistance + buff_magicResistance);
            GameEvents.instance.StatisticUpdate(StatisticType.MagicResistance, magicResistance);
        }
        if (attackSpeed != (base_attackSpeed + eq_attackSpeed + buff_attackSpeed))
        {
            attackSpeed = (base_attackSpeed + eq_attackSpeed + buff_attackSpeed);
            GameEvents.instance.StatisticUpdate(StatisticType.AttackSpeed, attackSpeed);
        }

        if (attackRange != (base_attackRange + eq_attackRange + buff_attackRange))
        {
            attackRange = (base_attackRange + eq_attackRange + buff_attackRange);
            GameEvents.instance.StatisticUpdate(StatisticType.AttackRange, attackRange);
        }

        if (physicalDamage != (base_physicalDamage + eq_physicalDamage + buff_physicalDamage))
        {
            physicalDamage = (base_physicalDamage + eq_physicalDamage + buff_physicalDamage);
            GameEvents.instance.StatisticUpdate(StatisticType.PhysicalDamage, physicalDamage);
        }

        if (magicDamage != (base_magicDamage + eq_magicDamage + buff_magicDamage))
        {
            magicDamage = (base_magicDamage + eq_magicDamage + buff_magicDamage);
            GameEvents.instance.StatisticUpdate(StatisticType.MagicDamage, magicDamage);
        }

        if (trueDamage != (base_trueDamage + eq_trueDamage + buff_trueDamage))
        {
            trueDamage = (base_trueDamage + eq_trueDamage + buff_trueDamage);
            GameEvents.instance.StatisticUpdate(StatisticType.TrueDamage, trueDamage);
        }

        if (healthPointsRegen != (base_healthPointsRegen + eq_healthPointsRegen + buff_healthPointsRegen))
        {
            healthPointsRegen = (base_healthPointsRegen + eq_healthPointsRegen + buff_healthPointsRegen);
            GameEvents.instance.StatisticUpdate(StatisticType.HealthPointsRegeneration, healthPointsRegen);
        }

        if (healthPercentsRegen != (base_healthPercentsRegen + eq_healthPercentsRegen + buff_healthPercentsRegen))
        {
            healthPercentsRegen = (base_healthPercentsRegen + eq_healthPercentsRegen + buff_healthPercentsRegen);
            GameEvents.instance.StatisticUpdate(StatisticType.HealthPercentageRegeneration, healthPercentsRegen);
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