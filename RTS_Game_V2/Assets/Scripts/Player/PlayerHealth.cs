using System.Collections;
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

    private const float interval = 5f;
    private float timeLeft;

    [SerializeField] PlayerAnimation playerAnim;

    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
        GameEvents.instance.OnLoadedPlayerData += OnplayerDataLoaded;

    }

    private void Start()
    {
        OnplayerDataLoaded();
    }

    private void OnplayerDataLoaded()
    {
        health = BuffManager.instance.LoadedPlayerHP;
        GameEvents.instance.UpdateCurrentHP(health);
        GameEvents.instance.PlayerStateEvent(PlayerStateEvent.NONE);
    }


    void Update()
    {
        if(GameController.GameStatus == GameStatus.ON)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0 && (health < maxHealth))
            {
                Heal(healthRegeneration / 12);
                timeLeft = interval;
            }

            if(health <= 0)
            {
                Die();
            }
        }
    }
    public void Damage(string enemyName,float physicalDamage, float magicDamage, float trueDamage)
    {
        float totalDamage = Mathf.RoundToInt(physicalDamage * physicalDamageMultiplier + magicDamage * magicDamageMultiplier + trueDamage);
        ConsolePanel.instance.PlayerTakeDamage(enemyName, totalDamage);
        health -= totalDamage;
        playerAnim.HitAnimation();
        GameEvents.instance.UpdateCurrentHP(health);
    }

    public void StunEffect(float stunDuration)
    {
        StopCoroutine(StunCooldown(stunDuration));
        StartCoroutine(StunCooldown(stunDuration));
        GameEvents.instance.PlayerStateEvent(PlayerStateEvent.STUN);
    }

    IEnumerator StunCooldown(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        GameEvents.instance.PlayerStateEvent(PlayerStateEvent.NONE);
    }

    private void UpdateStats(StatisticType statisticType, float value)
    {
        switch (statisticType)
        {
            case StatisticType.MaxHealth:
                float diff = value - maxHealth;
                maxHealth = value;
                health += diff;

                if(health <= 0)
                {
                    if(GameController.GameStatus == GameStatus.ON)
                    {
                        Die();  
                    }
                }
                GameEvents.instance.UpdateCurrentHP(health);
                break;
            case StatisticType.Armor:
                armor = value;
                physicalDamageMultiplier = StatisticalUtility.DamageMultiplier(armor);
                break;
            case StatisticType.MagicResistance:
                magicResistance = value;
                magicDamageMultiplier = StatisticalUtility.DamageMultiplier(magicResistance);
                break;
            case StatisticType.HealthPercentageRegeneration:
                healthPercentsRegen = value;
                healthRegeneration = StatisticalUtility.HealthRegeneration(maxHealth, healthPointsRegen, healthPercentsRegen);
                break;
            case StatisticType.HealthPointsRegeneration:
                healthPointsRegen = value;
                healthRegeneration = StatisticalUtility.HealthRegeneration(maxHealth, healthPointsRegen, healthPercentsRegen);
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

    private void Die()
    {
        GameEvents.instance.PlayerStateEvent(PlayerStateEvent.DEATH);
        GameEvents.instance.GameOver();
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
        GameEvents.instance.OnLoadedPlayerData -= OnplayerDataLoaded;
    }
}
