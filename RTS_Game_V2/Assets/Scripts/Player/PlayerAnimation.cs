using UnityEngine;
using UnityEngine.AI;

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

    private void Start()
    {
        UpdateAttackType();
        animator.keepAnimatorControllerStateOnDisable = true;
    }

    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
        GameEvents.instance.OnEquipmentUpdate += UpdateAttackType;
        GameEvents.instance.OnPlayerStateEvent += PlayerStateAnimation;
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
    }

    private void PlayerStateAnimation(PlayerStateEvent playerStateEvent)
    {
        switch (playerStateEvent)
        {
            case PlayerStateEvent.NONE:
                if (lastPlayerState == PlayerStateEvent.STUN)
                {
                    animator.SetBool(performStunAnimation, false);
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
                animator.SetBool(performStunAnimation, false);
                break;
            default:
                break;
        }

        lastPlayerState = playerStateEvent;

    }

    private void UpdateAttackType()
    {
        EquipmentSlot eqLeftHandSlot = EquipmentManager.instance.GetEquipmentSlot(EquipmentSlotType.LEFT_ARM);
        EquipmentSlot eqRightHandSlot = EquipmentManager.instance.GetEquipmentSlot(EquipmentSlotType.RIGHT_ARM);

        attackType = AttackType.FISTS;

        if (!eqLeftHandSlot.Empty && eqLeftHandSlot.Item.IsWeapon)
        {
            attackType = eqLeftHandSlot.Item.AttackType;
            animator.SetFloat(attackTypeIndex, (int)attackType);
            return;
        }

        if (!eqRightHandSlot.Empty && eqRightHandSlot.Item.IsWeapon)
        {
            attackType = eqRightHandSlot.Item.AttackType;
            animator.SetFloat(attackTypeIndex, (int)attackType);
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


    public void HitAnimation() => animator.SetTrigger(performHitAnimation);

    public void AttackAnimation() => animator.SetTrigger(performAttackAnimation);
    

    public void DeathAnimation() => animator.SetTrigger(performDeathAnimation);

    public void StunAnimation() => animator.SetBool(performStunAnimation, true);

    public void BuffAnimation() => animator.SetTrigger(performBuffAnimation);

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
        GameEvents.instance.OnEquipmentUpdate -= UpdateAttackType;
        GameEvents.instance.OnPlayerStateEvent -= PlayerStateAnimation;
    }
}
