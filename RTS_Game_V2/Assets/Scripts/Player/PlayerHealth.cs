using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 100;
    private float health = 100;
    private float armor;
    private float magicResistance;
    private float healthPointsRegen;
    private float healthPercentsRegen;
    private float healthRegeneration;

    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;

    private const float interval = 5f;
    private float timeLeft;


    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
    }

    private void Start()
    {
        health = BuffManager.instance.PlayerHP;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0 && (health < maxHealth))
        {
            Heal(healthRegeneration/12);
            timeLeft = interval;
        }
    }
    public void Damage(string enemyName,float physicalDamage, float magicDamage, float trueDamage)
    {
        float totalDamage = Mathf.RoundToInt(physicalDamage * physicalDamageMultiplier + magicDamage * magicDamageMultiplier + trueDamage);
        //Console Log
        ConsolePanel.instance.PlayerTakeDamage(enemyName, totalDamage);
        health -= totalDamage;
        GameEvents.instance.UpdateCurrentHP(health);
    }


    public void UpdateStats(StatisticType statisticType, float value)
    {
        switch (statisticType)
        {
            case StatisticType.MaxHealth:
                maxHealth = value;
                health = Mathf.Clamp(health, 0, maxHealth);
                GameEvents.instance.UpdateCurrentHP(health);
                break;
            case StatisticType.Armor:
                armor = value;
                physicalDamageMultiplier = 100 / (100 + armor);
                break;
            case StatisticType.MagicResistance:
                magicResistance = value;
                magicDamageMultiplier = 100 / (100 + magicResistance);
                break;
            case StatisticType.HealthPercentageRegeneration:
                healthPercentsRegen = value;
                healthRegeneration = healthPointsRegen + (maxHealth * (healthPercentsRegen / 100));
                break;
            case StatisticType.HealthPointsRegeneration:
                healthPointsRegen = value;
                healthRegeneration = healthPointsRegen + (maxHealth * (healthPercentsRegen / 100));
                break;
            default:
                break;
        }
    }

    public void Damage(float healthPoints)
    {
        health -= healthPoints;
        GameEvents.instance.UpdateCurrentHP(health);
    }

    public void Heal(float healthPoints, bool consoleLog = false)
    {
        float beforeHeal = health;
        health += healthPoints;
        health = Mathf.Clamp(health, 0, maxHealth);
        float afterHeal = health;
        float diff = afterHeal - beforeHeal;
        GameEvents.instance.UpdateCurrentHP(health);
        if (consoleLog)
        {
            ConsolePanel.instance.HealLog(diff);
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
    }
}
