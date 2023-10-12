using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
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

    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
        GameEvents.instance.OnEnemyClick += SetTarget;
    }
    void Start()
    {

    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if(objectToAttack!= null)
        {

            distanceToEnemy = Vector3.Distance(transform.position, objectToAttack.transform.position);
            //if(enemy == null)
            //{
            //    enemy = objectToAttack.GetComponent<Enemy>();
            //}

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

    public void SetTarget(Enemy target)
    {
        if(target == null)
        {
            enemy = null;
            objectToAttack = null;
        }
        else
        {
            objectToAttack = target.transform.gameObject;
            enemy = target;
        }
    }
    public void UpdateStats(StatisticType statisticType, float value)
    {
        switch (statisticType)
        {
            case StatisticType.PhysicalDamage:
                physicalDamage = value;
                break;
            case StatisticType.MagicDamage:
                magicDamage = value;
                break;
            case StatisticType.TrueDamage:
                trueDamage = value;
                break;
            case StatisticType.AttackSpeed:
                attackSpeed = value;
                timeToWait = 60 / attackSpeed;
                break;
            case StatisticType.AttackRange:
                attackRange = value;
                break;
            default:
                break;
        }

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
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
        GameEvents.instance.OnEnemyClick -= SetTarget;
    }
}
