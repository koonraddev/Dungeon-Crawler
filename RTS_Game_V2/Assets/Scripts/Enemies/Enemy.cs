using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfigurationSO enemyConfig;
    [SerializeField] LootSO lootSO;

    private GameObject parentRoom;

    private string enemyName;
    private float maxHealth;
    private float health;
    private float armor;
    private float magicResistance;

    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;

    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] EnemyAttack enemyAttack;

    private void Awake()
    {
        maxHealth = enemyConfig.Health;
        armor = enemyConfig.Armor;
        magicResistance = enemyConfig.MagicResistance;
        enemyName = enemyConfig.EnemyName;

        physicalDamageMultiplier = 100 / (100 - armor);
        magicDamageMultiplier = 100 / (100 - magicResistance);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        health = maxHealth;
    }


    void Start()
    {

    }

    public void SetEnemy(GameObject parentRoom)
    {
        this.parentRoom = parentRoom;
        gameObject.transform.SetParent(parentRoom.transform);

        if (enemyMovement != null)
        {
            enemyMovement.SetEnemyMovement(enemyConfig.MovementSpeed, enemyConfig.MinMoveInterval, enemyConfig.MaxMoveInterval, parentRoom);
        }
        if(enemyAttack != null)
        {
            enemyAttack.SetEnemyAttack(enemyConfig.EnemyName,enemyConfig.AttackSpeed, enemyConfig.AttackRange, enemyConfig.TriggerRange, enemyConfig.PhysicalDamage, enemyConfig.MagicDamage, enemyConfig.TrueDamage);
        }
    }

    void Update()
    {
        if(health <= 0)
        {
            Die();
        }
    }


    public void Damage(float physicalDamage, float magicDamage, float trueDamage)
    {
        float totalDamage = physicalDamage * physicalDamageMultiplier + magicDamage * magicDamageMultiplier + trueDamage;
        health -= totalDamage;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
