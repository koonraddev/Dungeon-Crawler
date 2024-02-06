using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
[System.Serializable]
public class DistanceAttackConfig
{
    [SerializeField] private AttackType attackType;
    [SerializeField] private GameObject projectilePrefab;

    public AttackType AttackType { get => attackType; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }
}


public class PlayerAttack : MonoBehaviour
{
    //Statistics
    private float attackSpeed, attackRange, physicalDamage, magicDamage,trueDamage;

    private float timeToWait;
    private float attackCooldown;
    private float distanceToEnemy;

    private GameObject objectToAttack;
    private Enemy enemy;
    [SerializeField] PlayerMovement playerMov;
    [SerializeField] PlayerAnimation playerAnim;
    [SerializeField] private bool followTarget;
    [SerializeField] private List<DistanceAttackConfig> distAttackConfigs;
    private bool follow;
    private bool blockAttack;
    private AttackType attackType;
    private GameObject projectilePrefab;

    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
        GameEvents.instance.OnEnemyClick += SetTarget;
        GameEvents.instance.OnCancelActions += DontFollow;
        GameEvents.instance.OnPlayerStateEvent += SuspendAttack;
        GameEvents.instance.OnEquipmentUpdate += UpdateAttackType;
    }

    private void UpdateAttackType()
    {
        EquipmentSlot eqLeftHandSlot = EquipmentManager.instance.GetEquipmentSlot(EquipmentSlotType.LEFT_ARM);
        EquipmentSlot eqRightHandSlot = EquipmentManager.instance.GetEquipmentSlot(EquipmentSlotType.RIGHT_ARM);

        attackType = AttackType.FISTS;

        if (!eqLeftHandSlot.Empty && eqLeftHandSlot.Item.IsWeapon)
        {
            attackType = eqLeftHandSlot.Item.AttackType;
        }

        if (!eqRightHandSlot.Empty && eqRightHandSlot.Item.IsWeapon)
        {
            attackType = eqRightHandSlot.Item.AttackType;
        }

        //bulletPrefab = distAttackConfigs.First(s => s.AttackType == attackType).BulletPrefab;

        foreach (var item in distAttackConfigs)
        {
            if(item.AttackType == attackType)
            {
                projectilePrefab = item.ProjectilePrefab;
            }
        }
    }

    private void SuspendAttack(PlayerStateEvent playerState)
    {
        switch (playerState)
        {
            case PlayerStateEvent.NONE:
                blockAttack = false;
                break;
            case PlayerStateEvent.STUN:
                blockAttack = true;
                break;
            default:
                break;
        }
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
            CheckForAttack();
        }
    }

    private void CheckForAttack()
    {
        if (attackCooldown <= 0 
            && distanceToEnemy <= attackRange 
            && enemy != null 
            && !blockAttack)
        {
            playerMov.RotateTo(enemy.transform.position);
            playerAnim.AttackAnimation();
            attackCooldown = timeToWait;
        }
    }

    private void Attack()
    {
        if (enemy != null)
        {
            switch (attackType)
            {
                case AttackType.FISTS:
                case AttackType.SWORD:
                    enemy.Damage(physicalDamage, magicDamage, trueDamage);
                    break;
                case AttackType.WAND:
                case AttackType.BOW:
                case AttackType.SPELL:
                    FireProjectilePrefab();
                    break;
                default:
                    break;
            }
            
        }
    }

    private void FireProjectilePrefab()
    {
        GameObject newBullet = Instantiate(projectilePrefab, transform.position,Quaternion.identity);
        float duration = distanceToEnemy / 20;
        newBullet.transform
            .DOMove(objectToAttack.transform.position, duration)
            .SetAutoKill(true)
            .SetEase(Ease.Linear)
            .OnComplete(() => BulletDamage(newBullet))
            .Play();
    }

    private void BulletDamage(GameObject bullet)
    {
        enemy.Damage(physicalDamage, magicDamage, trueDamage);
        Destroy(bullet);
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
        GameEvents.instance.OnPlayerStateEvent -= SuspendAttack;
        GameEvents.instance.OnEquipmentUpdate -= UpdateAttackType;
    }
}
