using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //Statistics
    private float attackSpeed, attackRange, physicalDamage, magicDamage, trueDamage;

    private float timeToWait;
    private float attackCooldown;
    private float distanceToEnemy;

    private GameObject objectToAttack;
    private Enemy enemy;
    [SerializeField] PlayerMovement playerMov;
    [SerializeField] PlayerAnimation playerAnim;
    [SerializeField] private bool followTarget;
    private bool follow;
    private bool blockAttack;
    private bool projectileAttack;
    private float projectileSpeed;
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


        if (!eqLeftHandSlot.Empty && eqLeftHandSlot.Item.IsWeapon)
        {
            projectilePrefab = eqLeftHandSlot.Item.ProjectilePrefab;
            projectileAttack = eqLeftHandSlot.Item.ProjectileAttack;

        }

        if (!eqRightHandSlot.Empty && eqRightHandSlot.Item.IsWeapon)
        {
            projectileAttack = eqRightHandSlot.Item.ProjectileAttack;
            projectilePrefab = eqRightHandSlot.Item.ProjectilePrefab;
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

        if (objectToAttack != null)
        {
            if (!objectToAttack.activeSelf)
            {
                objectToAttack = null;
                enemy = null;
                return;
            }

            if(!StatisticalUtility.CheckIfTargetInRange(gameObject, objectToAttack, attackRange, out InteractionPoints intStruct, true))
            {
                if (followTarget && follow)
                {
                    playerMov.MoveTo(intStruct.closestPoint);
                }
            }

            distanceToEnemy = Vector3.Distance(intStruct.startPoint, intStruct.targetPoint);

            CheckIfCanAttack();
        }
    }

    private void CheckIfCanAttack()
    {
        if (attackCooldown <= 0
            && distanceToEnemy <= attackRange
            && enemy != null
            && !blockAttack
            && !StatisticalUtility.CheckIfTargetIsBlocked(gameObject, objectToAttack))
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
            if (projectileAttack)
            {
                FireProjectilePrefab();
            }
            else
            {
                DealDamage();
            }

        }
    }

    private void FireProjectilePrefab()
    {
        if (projectilePrefab == null) return;

        GameObject newProjectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile newProjectile = newProjectileObject.AddComponent<Projectile>();
        newProjectile.SetProjectile(objectToAttack, projectileSpeed, DealDamage);

    }

    private void DealDamage()
    {
        if (enemy == null) return;

        enemy.Damage(physicalDamage, magicDamage, trueDamage);
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
        projectileSpeed = StatisticalUtility.ProjectileSpeed(physicalDamage, magicDamage, attackSpeed, attackRange);
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
