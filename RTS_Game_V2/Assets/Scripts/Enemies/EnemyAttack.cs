using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("Set Enemy Movement script if you want object to follow target if triggered")]
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] EnemyAnimation enemyAnimation;
    private string enemyName;
    private float attackSpeed ,attackRange ,triggerRange ,physicalDamage ,magicDamage ,trueDamage;
    private bool projectileAttack;
    private GameObject projectilePrefab;
    private float projectileSpeed;

    private float timeToWait, attackCooldown;


    private GameObject playerObject;
    private PlayerHealth playerHealthMan;

    private float distance;
    private bool dead;
    public bool Dead { get => dead; set => dead = value; }

    //[SerializeField] EnemyAttackConfigurationSO enemyAttackConfigurationSO;
    //private AttackType attackType;
    //private float effectSpeed;
    //private GameObject effeectPrefab;
    //private GameObject effect;

    //private List<GameObject> pooledObjects;
    void Start()
    {
        attackCooldown = 0;
    }

    private void Awake()
    {
        //pooledObjects = new List<GameObject>();
        //attackType = enemyAttackConfigurationSO.AttackType;
        //effectSpeed = enemyAttackConfigurationSO.EffectSpeed;
        //effeectPrefab = enemyAttackConfigurationSO.EffectPrefab;
        //CreateObject();
    }

    private void OnEnable()
    {
        if (dead)
        {
            dead = false;
        }
    }

    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (dead)
        {
            return;
        }

        if (playerObject != null)
        {


            distance = Vector3.Distance(transform.position, playerObject.transform.position);

            if (CheckPlayerInterest())
            {
                Vector3 lookAt = new Vector3
                    (playerObject.transform.position.x,
                    this.gameObject.transform.position.y,
                    playerObject.transform.position.z
                    );
                transform.LookAt(lookAt);
                if (!StatisticalUtility.CheckIfTargetInRange(gameObject, playerObject, attackRange, out Vector3 pointMove))
                {
                    Vector3 newPointToMove = pointMove;
                    if (enemyMovement != null)
                    {
                        //distToMove = Mathf.Ceil(distToTarget - attackRange);
                        enemyMovement.MoveTo(newPointToMove);
                    }
                }
                else
                {
                    PerformAttack();
                }
            }

        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, triggerRange);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    playerObject = collider.gameObject;
                    //enemyMovement.StopMovement();
                    playerHealthMan = playerObject.GetComponent<PlayerHealth>();
                }
            }
        }
    }

    private bool CheckPlayerInterest()
    {
        if (!StatisticalUtility.CheckIfTargetInRange(gameObject, playerObject, triggerRange))
        {
            playerObject = null;
            playerHealthMan = null;
            return false;
        }

        if(StatisticalUtility.CheckIfTargetIsBlocked(gameObject, playerObject))
        {
            return false;
        }

        return true;
    }

    private void PerformAttack()
    {
        if (attackCooldown <= 0)
        {
            enemyMovement.StopMovement();
            enemyAnimation.AttackAnimation();
            attackCooldown = timeToWait;
        }

    }

    private void Attack()
    {
        if (distance > attackRange) return;
        
        if (projectileAttack)
        {
            FireProjectilePrefab();
        }
        else
        {
            DealDamage();
        }
    }

    private void FireProjectilePrefab()
    {
        if( projectilePrefab == null) return;

        GameObject newProjectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile newProjectile = newProjectileObject.AddComponent<Projectile>();
        newProjectile.SetProjectile(playerObject, projectileSpeed, DealDamage);
    }

    private void DealDamage()
    {
        if (playerHealthMan == null) return;

        playerHealthMan.Damage(enemyName,physicalDamage, magicDamage, trueDamage);
    }

    public void SetEnemyAttack(string enemyName, float attackSpeed, float attackRange, float triggerRange, float physicalDamage, float magicDamage, float trueDamage, bool projectileAttack, GameObject projectilePrefab)
    {
        this.enemyName = enemyName;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.triggerRange = triggerRange;
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
        this.trueDamage = trueDamage;
        this.projectileAttack = projectileAttack;
        this.projectilePrefab = projectilePrefab;
        projectileSpeed = StatisticalUtility.ProjectileSpeed(physicalDamage, magicDamage, attackSpeed, attackRange);
        timeToWait = StatisticalUtility.AttackCooldown(attackSpeed);
    }


    private void OnDisable()
    {
        playerObject = null;
        playerHealthMan = null;
    }
}
