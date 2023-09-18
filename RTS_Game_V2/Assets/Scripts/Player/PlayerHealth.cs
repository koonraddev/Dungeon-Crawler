using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth;
    private float health;
    private float armor;
    private float magicResistance;
    private float healthPointsRegen;
    private float healthPercentsRegen;
    private float healthRegeneration;

    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;

    private float interval = 60f;
    private float timeLeft;
    void Start()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0 && (health < maxHealth))
        {
            Heal(healthRegeneration);
            timeLeft = interval;
        }
        Mathf.Clamp(health, 0, maxHealth);
    }

    public void Damage(float physicalDamage, float magicDamage, float trueDamage)
    {
        float totalDamage = physicalDamage * physicalDamageMultiplier + magicDamage * magicDamageMultiplier + trueDamage;
        //Console Log
        Debug.Log("LOG. Attack damage gained: PH - " + physicalDamage * physicalDamageMultiplier + "; MA - " + magicDamage * magicDamageMultiplier + " TR - " + trueDamage);
        health -= totalDamage;
    }

    public void UpdateStats(StatisticType statisticType, float value)
    {
        switch (statisticType)
        {
            case StatisticType.MaxHealth:
                maxHealth = value;
                break;
            case StatisticType.Armor:
                armor = value;
                physicalDamageMultiplier = 100 / (100 - armor);
                break;
            case StatisticType.MagicResistance:
                magicResistance = value;
                magicDamageMultiplier = 100 / (100 - magicResistance);
                break;
            case StatisticType.HealthPercentageRegeneration:
                healthPercentsRegen = value;
                healthRegeneration = healthPointsRegen + (maxHealth * healthPercentsRegen);
                break;
            case StatisticType.HealthPointsRegeneration:
                healthPointsRegen = value;
                healthRegeneration = healthPointsRegen + (maxHealth * healthPercentsRegen);
                break;
            default:
                break;
        }
    }

    public void Damage(float healthPoints)
    {
        health -= healthPoints;
    }

    public void Heal(float healthPoints)
    {
        health += healthPoints;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
    }
}
