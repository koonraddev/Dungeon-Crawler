using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private Animator animator;
    [SerializeField] private int attackAnimationsAmount, getHitAnimationsAmount;
    int performAttackAnimation, performHitAnimation, performDeathAnimation;
    int movementSpeedParameter, attackTypeIndex, getHitTypeIndex,attackAnimationSpeedParameter;
    System.Random rand;

    private float movementSpeed;


    private void Awake()
    {
        movementSpeedParameter = Animator.StringToHash("MovementSpeed");
        attackTypeIndex = Animator.StringToHash("AttackIndex");
        getHitTypeIndex = Animator.StringToHash("GetHitIndex");
        attackAnimationSpeedParameter = Animator.StringToHash("AnimationSpeed");
        performAttackAnimation = Animator.StringToHash("Attack");
        performHitAnimation = Animator.StringToHash("Hit");
        performDeathAnimation = Animator.StringToHash("Death");

        if (attackAnimationsAmount > 1 || getHitAnimationsAmount > 1)
        {
            rand = new();
        }
    }

    void Update()
    {
        if (!enemyScript.Dead)
        {
            animator.SetBool(performDeathAnimation, false);
        }

        if (!animator.IsInTransition(2))
        {

        }

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("New State"))
        {
            enemyMovement.blockMovement = false;
        }
        else
        {
            enemyMovement.blockMovement = true;
        }


        if (enemyAgent.velocity == Vector3.zero)
        {
            animator.SetFloat(movementSpeedParameter, 0);
        }
        else
        {
            animator.SetFloat(movementSpeedParameter, movementSpeed);
        }

    }


    public void SetEnemyAnimator(float attackSpeed, float movementSpeed)
    {
        float timeToWait = 60 / attackSpeed;
        float attackAnimationSpeed;
        if (timeToWait < 1)
        {
            attackAnimationSpeed = 1 / timeToWait;
        }
        else
        {
            attackAnimationSpeed = 1;
        }

        this.movementSpeed = movementSpeed;


        animator.SetFloat(attackAnimationSpeedParameter, attackAnimationSpeed);
    }


    public void DeathAnimation()
    {
        animator.SetBool(performDeathAnimation, true);
    }

    public void GetHitAnimation()
    {
        int getHitIndex = 0;
        
        if(getHitAnimationsAmount > 1 && rand != null)
        {
            getHitIndex = rand.Next(getHitAnimationsAmount);
        }

        animator.SetFloat(getHitTypeIndex, getHitIndex);
        animator.SetTrigger(performHitAnimation);
    }

    public void AttackAnimation()
    {
        int attackIndex = 0;
        if (attackAnimationsAmount > 1 && rand != null)
        {
            attackIndex = rand.Next(attackAnimationsAmount);
        }
        
        animator.SetFloat(attackTypeIndex, attackIndex);
        animator.SetTrigger(performAttackAnimation);
    }
}
