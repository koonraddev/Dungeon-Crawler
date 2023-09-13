using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatisticType
{
    MaxHealth,
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
    private float maxHealth;
    private float health;
    private float armor;
    private float magicResistance;
    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;
    private float attackSpeed;
    private float attackRange;

    //Health Regeneration;
    private float healthRegeneration;// health(Points) per minute (totality)
    private float healthPointsRegen;// health regeneration: POINTS (constant) per minute
    private float healthPercentsRegen;// health regeneration: PERCENTAGES (of max health) per minute


    //[SerializeField] EquipmentManager playerEq;

    private float interval = 60f;
    private float timeLeft;
    void Start()
    {
        GameEvents.instance.OnEquipmentUpdate += UpdateStats;
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


    private void SetAllStatistics(float armor, float magicResistance, float health, float maxHealth)
    {
        SetArmor(armor);
        SetMagicResistance(magicResistance);
        this.health = health;
        SetMaxHealth(maxHealth);
    }

    private void SetAllStatistics()
    {
        float newMaxHealth = 0;
        float newArmor = 0;
        float newMagicResist = 0;
        float newPhysicalDmg = 0;
        float newMagicDmg = 0;
        float newhealthPointsRegen = 0;
        float newhealtPercentageRegen = 0;
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
                    case StatisticType.HealthPointsRegeneration:
                        newhealthPointsRegen += oneStat.Value;
                        break;
                    case StatisticType.HealthPercentageRegeneration:
                        newhealtPercentageRegen += oneStat.Value;
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


            maxHealth = newMaxHealth;
            healthPointsRegen = newhealthPointsRegen;
            healthPercentsRegen = newhealtPercentageRegen;
            armor = newArmor;
            magicResistance = newMagicResist;
            attackSpeed = newAttackSpeed;
            attackRange = newAttackRange;

        }

        //Equipment.Instance.Ge
        SetArmor(armor);
        SetMagicResistance(magicResistance);
        SetMaxHealth(maxHealth);
    }



    public void UpdateStats()
    {
        //SetAllStatistics();
    }

    public void SetArmor(float armor)
    {
        this.armor = armor;
        physicalDamageMultiplier = 100 / (100 - armor);
    }

    public void SetMagicResistance(float magicResistance)
    {
        this.magicResistance = magicResistance;
        magicDamageMultiplier = 100 / (100 - magicResistance);
    }


    public void SetRegeneration()
    {

    }

    public void SetMaxHealth(float maxHealth)
    {
        float healthRatio = health / this.maxHealth;
        this.maxHealth = maxHealth;
        health = healthRatio * this.maxHealth;
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