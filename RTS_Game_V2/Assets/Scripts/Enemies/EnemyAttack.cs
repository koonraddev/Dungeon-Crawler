using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("Set Enemy Movement script if you want object to follow target if triggered")]
    [SerializeField] EnemyMovement enemyMovement;
    private float attackSpeed;
    private float attackRange;
    private float triggerRange;
    private float physicalDamage;
    private float magicDamage;
    private float trueDamage;
    private EnemyAttackConfigurationSO enemyAttackConfigurationSO;

    private float attackCooldown;

    private GameObject playerObject;
    private PlayerStatistics playerStats;
    private Vector3 pointToMove;

    void Start()
    {
        attackCooldown = 0;
    }

    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }


        if (playerObject != null)
        {
            transform.LookAt(playerObject.transform);
            float distance = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distance > triggerRange)
            {
                playerObject = null;
            }
            else
            {
                if (Physics.Linecast(this.transform.position,playerObject.transform.position,out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        playerObject = null;
                        return;
                    }
                }
                if (enemyMovement != null)
                {
                    if (distance > attackRange)
                    {
                        Vector3 dirToTarget = playerObject.transform.position - this.transform.position;
                        Vector3 dirToTargetNorm = dirToTarget.normalized;
                        float distToTarget = dirToTarget.magnitude;
                        float distToMove = distToTarget - attackRange;
                        pointToMove = this.transform.position + dirToTargetNorm * distToMove;
                        enemyMovement.MoveTo(pointToMove);
                    }
                }

                if (distance <= attackRange)
                {
                    Attack();
                }
            }
        }
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    playerObject = collider.gameObject;
                    enemyMovement.StopMovement();
                    playerStats = playerObject.GetComponent<PlayerStatistics>();
                }
            }
        }
    }

    private void Attack()
    {
        if (attackCooldown <= 0 && playerStats != null)
        {
            playerStats.Damage(physicalDamage, magicDamage, trueDamage);
            enemyAttackConfigurationSO.AttackEffect(this.gameObject,playerObject);
            attackCooldown = attackSpeed;
        }
    }

    public void SetEnemyAttack(float attackSpeed, float attackRange, float triggerRange, float physicalDamage, float magicDamage, float trueDamage, EnemyAttackConfigurationSO enemyAttackConfigurationSO)
    {
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.triggerRange = triggerRange;
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
        this.trueDamage = trueDamage;
        this.enemyAttackConfigurationSO = enemyAttackConfigurationSO;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, triggerRange);
        Gizmos.DrawSphere(pointToMove, 1);
    }
}
