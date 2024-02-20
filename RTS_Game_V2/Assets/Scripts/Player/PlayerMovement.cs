using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private LayerMask groundMask;
    [SerializeField]private NavMeshAgent playerAgent;

    private PlayerControls playerControls;
    private InputAction moveInspectAction;

    private float movementSpeed;
    private bool blockMovement = false;
    public bool blcked;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
        GameEvents.instance.OnPlayerStateEvent += StopAndBlockMovement;
    }

    private void StopAndBlockMovement(PlayerStateEvent playerState)
    {
        switch (playerState)
        {
            case PlayerStateEvent.NONE:
                UnlockMovement();
                break;
            case PlayerStateEvent.BUFF:
            case PlayerStateEvent.STUN:
            case PlayerStateEvent.DEATH:
                blockMovement = true;
                StopMovement();
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        playerControls.Disable();
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
        GameEvents.instance.OnPlayerStateEvent -= StopAndBlockMovement;
    }

    void Start()
    {
        moveInspectAction = playerControls.BasicMovement.Move;
        groundMask = LayerMask.NameToLayer("Ground");
        UnlockMovement();
    }


    void Update()
    {
        blcked = playerAgent.isStopped;
        if (moveInspectAction.IsInProgress() && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hitPoint, Mathf.Infinity))
            {
                if (hitPoint.collider.gameObject.layer == groundMask)
                {
                    GameEvents.instance.CancelActions();
                    MoveTo(hitPoint.point);
                }
            }
        }

        if (playerAgent.isOnOffMeshLink)
        {
            GameEvents.instance.CancelActions();
            GameEvents.instance.CancelGameObjectAction();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StopMovement();
        //}
    }

    public void UpdateStats(StatisticType statisticType, float value)
    {
        if(statisticType == StatisticType.MovementSpeed)
        {
            movementSpeed = value;
            playerAgent.speed = movementSpeed;
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (blockMovement)
        {
            return;
        }
        //SoundManager.PlaySound(pointDestinationSound, 1f);

        //Debug.Log(destination);

        playerAgent.isStopped = false;
        playerAgent.SetDestination(destination);

    }

    public void TeleportTo(Vector3 destination)
    {
        playerAgent.Warp(destination);
    }

    public void StopMovement()
    {
        playerAgent.isStopped = true;
    }

    public void RotateTo(Vector3 destination)
    {
        Vector3 target = destination - transform.position;
        target.y = 0;
        Quaternion rot = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1000f);
    }
    private void UnlockMovement()
    {
        blockMovement = false;
        playerAgent.isStopped = false;
    }
}
