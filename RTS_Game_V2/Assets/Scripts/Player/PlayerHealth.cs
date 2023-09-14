using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStatistics playerStats;

    private float maxHealth;
    private float health;
    private float armor;
    private float magicResistance;
    private float healthRegeneration;

    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;

    private float interval = 60f;
    private float timeLeft;
    void Start()
    {
        UpdateStats();
        health = maxHealth;

        GameEvents.instance.OnStatsUpdate += UpdateStats;
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

    public void UpdateStats()
    {
        maxHealth = playerStats.MaxHealth;
        armor = playerStats.Armor;
        magicResistance = playerStats.MagicResistance;
        healthRegeneration = playerStats.HealthRegeneration;

        physicalDamageMultiplier = 100 / (100 - armor);
        magicDamageMultiplier = 100 / (100 - magicResistance);
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
        GameEvents.instance.OnStatsUpdate -= UpdateStats;
    }
}
