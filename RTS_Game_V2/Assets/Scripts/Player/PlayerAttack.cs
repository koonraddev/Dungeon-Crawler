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

    private float timeToWait;
    private float attackCooldown;
    private float distanceToEnemy;

    private GameObject objectToAttack;
    private Enemy enemy;
    [SerializeField] PlayerMovement playerMov;
    [SerializeField] private bool followTarget;
    private bool follow;

    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
        GameEvents.instance.OnEnemyClick += SetTarget;
        GameEvents.instance.OnCancelActions += DontFollow;
    }
    void Start()
    {

    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if(objectToAttack!= null)
        {
            if (!objectToAttack.activeSelf)
            {
                objectToAttack = null;
                enemy = null;
                return;
            }
            distanceToEnemy = Vector3.Distance(transform.position, objectToAttack.transform.position);
            if (followTarget && follow)
            {
                if (playerMov != null && distanceToEnemy > attackRange)
                {
                    Vector3 dirToTarget = objectToAttack.transform.position - this.transform.position;
                    Vector3 dirToTargetNorm = dirToTarget.normalized;
                    float distToTarget = dirToTarget.magnitude;
                    float distToMove = Mathf.Ceil(distToTarget - attackRange);
                    Vector3 pointToMove = this.transform.position + dirToTargetNorm * distToMove;
                    playerMov.MoveTo(pointToMove);
                }
            }
            Attack();
        }
    }

    public void Attack()
    {
        if (attackCooldown <= 0 && distanceToEnemy <= attackRange)
        {
            enemy.Damage(physicalDamage, magicDamage, trueDamage);
            attackCooldown = timeToWait;
        }
    }

    public void SetTarget(Enemy target)
    { 
        if (target == null)
        {
            enemy = null;
            objectToAttack = null;
            follow = false;
        }
        else
        {
            objectToAttack = target.transform.gameObject;
            enemy = target;
            follow = true;
        }
    }

    private void DontFollow()
    {
        follow = false;
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

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
        GameEvents.instance.OnEnemyClick -= SetTarget;
        GameEvents.instance.OnCancelActions -= DontFollow;
    }
}
