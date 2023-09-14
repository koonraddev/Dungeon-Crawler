using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerStatistics playerStats;
    private float attackSpeed;
    private float attackRange;
    private float physicalDamage;
    private float magicDamage;
    private float trueDamage;
    private GameObject objectToAttack;
    private Enemy enemy;

    private float timeToWait;
    private float attackCooldown;


    private float distanceToEnemy;
    void Start()
    {
        UpdateStats();
        GameEvents.instance.OnStatsUpdate += UpdateStats;
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if(objectToAttack!= null)
        {

            distanceToEnemy = Vector3.Distance(transform.position, objectToAttack.transform.position);
            if(enemy == null)
            {
                enemy = objectToAttack.GetComponent<Enemy>();
            }

            if (!objectToAttack.activeSelf)
            {
                objectToAttack = null;
                enemy = null;
            }
        }
        else
        {
            enemy = null;
        }

        Attack();
    }

    public void SetTarget(GameObject enemyTarget)
    {
        objectToAttack = enemyTarget;

        if(enemyTarget == null)
        {
            enemy = null;
        }
    }
    public void UpdateStats()
    {
        attackSpeed = playerStats.AttackSpeed;
        attackRange = playerStats.AttackRange;
        physicalDamage = playerStats.PhysicalDamage;
        magicDamage = playerStats.MagicDamage;
        trueDamage = playerStats.TrueDamage;

        timeToWait = 60 / attackSpeed;
    }

    public void Attack()
    {
        if (attackCooldown <= 0 && distanceToEnemy <= attackRange && enemy != null)
        {
            enemy.Damage(physicalDamage, magicDamage, trueDamage);
            attackCooldown = timeToWait;   
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatsUpdate -= UpdateStats;
    }
}
