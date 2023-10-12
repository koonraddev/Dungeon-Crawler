using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("Set Enemy Movement script if you want object to follow target if triggered")]
    [SerializeField] EnemyMovement enemyMovement;
    private string enemyName;
    private float attackSpeed;
    private float attackRange;
    private float triggerRange;
    private float physicalDamage;
    private float magicDamage;
    private float trueDamage;

    private float timeToWait;
    private float attackCooldown;

    private GameObject playerObject;
    private PlayerHealth playerHealthMan;
    private Vector3 pointToMove;


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

    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (playerObject != null)
        {
            transform.LookAt(playerObject.transform, Vector3.up);
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
                    playerHealthMan = playerObject.GetComponent<PlayerHealth>();
                }
            }
        }
    }

    private void Attack()
    {
        if (attackCooldown <= 0 && playerHealthMan != null)
        {
            //AttackEffect();
            playerHealthMan.Damage(enemyName, physicalDamage, magicDamage, trueDamage);
            attackCooldown = timeToWait;
        }
    }

    public void SetEnemyAttack(string enemyName, float attackSpeed, float attackRange, float triggerRange, float physicalDamage, float magicDamage, float trueDamage)
    {
        this.enemyName = enemyName;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.triggerRange = triggerRange;
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
        this.trueDamage = trueDamage;
        timeToWait = 60 / attackSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, triggerRange);
        Gizmos.DrawSphere(pointToMove, 1);
    }

    //private void AttackEffect()
    //{
    //    if(effeectPrefab != null)
    //    {
    //        if (GetPooledObject() == null)
    //        {
    //            CreateObject();
    //            effect = GetPooledObject();
    //        }
    //        else
    //        {
    //            effect = GetPooledObject();
    //        }
    //        effect.SetActive(true);
    //    }
    //}

    //private void CreateObject()
    //{
    //    GameObject tmp = Instantiate(effeectPrefab);
    //    tmp.SetActive(false);
    //    pooledObjects.Add(tmp);
    //}

    //private GameObject GetPooledObject()
    //{
    //    int objectsInList = pooledObjects.Count;
    //    for (int i = 0; i < objectsInList; i++)
    //    {
    //        if (!pooledObjects[i].activeInHierarchy)
    //        {
    //            return pooledObjects[i];
    //        }
    //    }
    //    return null;
    //}
}
