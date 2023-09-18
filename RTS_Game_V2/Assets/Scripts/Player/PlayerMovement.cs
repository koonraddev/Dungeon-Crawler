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
                    MoveTo(hitPoint.point);
                }
            }
        }
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
        //SoundManager.PlaySound(pointDestinationSound, 1f);
        playerAgent.SetDestination(destination);
    }
}
