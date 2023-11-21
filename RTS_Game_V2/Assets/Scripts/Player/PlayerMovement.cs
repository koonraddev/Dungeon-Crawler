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

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        GameEvents.instance.OnStatisticUpdate += UpdateStats;
    }
    private void OnDisable()
    {
        playerControls.Disable();
        GameEvents.instance.OnStatisticUpdate -= UpdateStats;
    }

    void Start()
    {
        moveInspectAction = playerControls.BasicMovement.Move;
        groundMask = LayerMask.NameToLayer("Ground");
    }


    void Update()
    {
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
            Debug.LogWarning("STOP");
            GameEvents.instance.CancelActions();
            GameEvents.instance.CancelGameObjectAction();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopMovement();
        }
    }

    public void UpdateStats(StatisticType statisticType, float value)
    {
        if(statisticType == StatisticType.MovementSpeed)
        {
            Debug.Log("SET SPEED");
            movementSpeed = value;
            playerAgent.speed = movementSpeed;
        }
    }

    public void MoveTo(Vector3 destination)
    {
        playerAgent.isStopped = false;
        //SoundManager.PlaySound(pointDestinationSound, 1f);

        //Debug.Log(destination);
        playerAgent.SetDestination(destination);
    }

    public void StopMovement()
    {
        Debug.Log("STOP MOVE");
        playerAgent.isStopped = true;
    }
}
