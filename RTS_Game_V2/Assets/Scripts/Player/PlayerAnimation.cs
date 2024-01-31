using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AttackType
{
    FISTS,
    SWORD,
    BOW,
    SPELL
}

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private NavMeshAgent playerAgent;


    [SerializeField] private float movementSpeed;
    [SerializeField] private AttackType attackType;
    [SerializeField] private float attackSpeed;
    public float animationSpeed, timeToWait;
    int movementSpeedParameter, attackTypeIndex, attackAnimationSpeed;
    int performAttackAnimation, performHitAnimation, performDeathAnimation, performBuffAnimation, performStunAnimation;

    [SerializeField] private Animator animator;
    private PlayerStateEvent lastPlayerState;

    private void Awake()
    {
        attackType = AttackType.FISTS;
        movementSpeedParameter = Animator.StringToHash("MovementSpeed");
        attackTypeIndex = Animator.StringToHash("AttackIndex");
        attackAnimationSpeed = Animator.StringToHash("AnimationSpeed");
        performAttackAnimation = Animator.StringToHash("Attack");
        performHitAnimation = Animator.StringToHash("Hit");
        performDeathAnimation = Animator.StringToHash("Death");
        performBuffAnimation = Animator.StringToHash("Buff");
        performStunAnimation = Animator.StringToHash("Stun");
    }
    void Start()
    {

    }

    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
        GameEvents.instance.OnEquipmentUpdate += UpdateAttackType;
        GameEvents.instance.OnPlayerStateEvent += PlayerStateAnimation;
    }

    private void PlayerStateAnimation(PlayerStateEvent playerStateEvent)
    {

        switch (playerStateEvent)
        {
            case PlayerStateEvent.NONE:
                switch (lastPlayerState)
                {
                    case PlayerStateEvent.STUN:
                        animator.SetBool(performStunAnimation, false);
                        break;
                    default:
                        break;
                }
                break;
            case PlayerStateEvent.BUFF:
                BuffAnimation();
                break;
            case PlayerStateEvent.STUN:
                StunAnimation();
                break;
            case PlayerStateEvent.DEATH:
                DeathAnimation();
                break;
            default:
                break;
        }

        lastPlayerState = playerStateEvent;

    }

    private void UpdateAttackType()
    {
        EquipmentSlot eqSlot = EquipmentManager.instance.GetEquipmentSlot(EquipmentSlotType.RIGHT_ARM);
        if (eqSlot.Empty)
        {
            attackType = AttackType.FISTS;
        }
        else
        {
            attackType = eqSlot.Item.AttackType;
        }
    }

    private void UpdateStats(StatisticType statisticType, float value)
    {
        switch (statisticType)
        {
            case StatisticType.MovementSpeed:
                movementSpeed = value;
                break;
            case StatisticType.AttackSpeed:
                attackSpeed = value;

                timeToWait = 60 / attackSpeed;

                if (timeToWait <= 1)
                {
                    animationSpeed = 1 / timeToWait;
                }
                else
                {
                    animationSpeed = 1;
                }

                animator.SetFloat(attackAnimationSpeed, animationSpeed);
                break;
            default:
                break;
        }
    }

    

    void Update()
    {
        if (playerAgent.velocity == Vector3.zero)
        {
            animator.SetFloat(movementSpeedParameter, 0);
        }
        else
        {
            animator.SetFloat(movementSpeedParameter, movementSpeed);
        }

        animator.SetFloat(attackTypeIndex, (int)attackType);
    }

    public void HitAnimation()
    {
        animator.SetBool(performHitAnimation, true);
        StartCoroutine(WaitAndSetFalse(0.35f, performHitAnimation));
    }

    public void AttackAnimation()
    {
        animator.SetBool(performAttackAnimation, true);
        StartCoroutine(WaitAndSetFalse(animationSpeed, performAttackAnimation));
    }

    public void DeathAnimation()
    {
        animator.SetBool(performDeathAnimation, true);
        StartCoroutine(WaitAndSetFalse(5f, performDeathAnimation));
    }

    public void StunAnimation()
    {
        animator.SetBool(performStunAnimation, true);
    }

    public void BuffAnimation()
    {
        animator.SetBool(performBuffAnimation, true);
        StartCoroutine(WaitAndSetFalse(1f, performBuffAnimation));
    }

    IEnumerator WaitAndSetFalse(float waitTime, int hashSetVariable)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool(hashSetVariable, false);
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
        GameEvents.instance.OnEquipmentUpdate -= UpdateAttackType;
        GameEvents.instance.OnPlayerStateEvent -= PlayerStateAnimation;
    }
}
