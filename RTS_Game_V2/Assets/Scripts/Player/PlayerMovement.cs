using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private LayerMask groundMask;
    private NavMeshAgent playerAgent;

    private PlayerControls playerControls;
    private InputAction moveInspectAction;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        moveInspectAction = playerControls.BasicMovement.MoveInspect;

        playerAgent = gameObject.GetComponent<NavMeshAgent>();
        groundMask = LayerMask.NameToLayer("Ground");
    }


    void Update()
    {
        if (moveInspectAction.triggered && !EventSystem.current.IsPointerOverGameObject())
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

    public void MoveTo(Vector3 destination)
    {
        //SoundManager.PlaySound(pointDestinationSound, 1f);
        playerAgent.SetDestination(destination);
    }
}
