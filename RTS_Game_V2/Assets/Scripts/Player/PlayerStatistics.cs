using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    private float maxHealth;
    private float health;
    private float armor;
    private float magicResistance;
    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;

    //Health Regeneration;
    private float healthRegeneration;// health(Points) per minute (totality)
    private float healthPointsRegen;// health regeneration: POINTS (constant) per minute
    private float healthPercentsRegen;// health regeneration: PERCENTAGES (of max health) per minute


    //[SerializeField] EquipmentManager playerEq;

    private float interval = 60f;
    private float timeLeft;
    void Start()
    {
        //GameEvents.instance.OnEquipmentUpdate += UpdateStats;
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