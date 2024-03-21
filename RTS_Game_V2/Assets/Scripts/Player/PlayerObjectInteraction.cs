using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerObjectInteraction : MonoBehaviour
{
    private GameObject pointedObject, clickedObject;
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    private int minimumDistanceFromObject;
    public float distanceFromObject, distanceFromClosestPoint;

    private PlayerControls playerControls;
    private InputAction moveInspectAction;

    private IEnumerator inspectCor;
    private bool canInteract;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        GameEvents.instance.OnCancelActions += CancelAction;
    }

    void Start()
    {
        moveInspectAction = playerControls.BasicMovement.Inspect;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hitPoint))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                pointedObject = hitPoint.transform.gameObject;
                if (moveInspectAction.triggered)
                {
                    if (pointedObject.TryGetComponent(out IInteractiveObject pointedScript))
                    {
                        GameEvents.instance.CancelGameObjectAction();
                        StopAllCoroutines();
                        clickedObject = pointedObject;
                        inspectCor = InspectObject(pointedScript);
                        StartCoroutine(inspectCor);
                    }
                }
            }
        }

        if (clickedObject != null)
        {
            StatisticalUtility.CheckIfTargetInRange(gameObject, clickedObject, minimumDistanceFromObject, out Vector3 closest, true);
            distanceFromObject = Vector3.Distance(gameObject.transform.position, clickedObject.transform.position);
            distanceFromClosestPoint = Vector3.Distance(closest, clickedObject.transform.position);

            canInteract = !StatisticalUtility.CheckIfTargetIsBlocked(gameObject, clickedObject);
        }
    }

    public IEnumerator InspectObject(IInteractiveObject objectToInspect)
    {
        minimumDistanceFromObject = objectToInspect.InteractionDistance;

        if (minimumDistanceFromObject < 100)
        {
            if (!StatisticalUtility.CheckIfTargetInRange(gameObject, clickedObject, minimumDistanceFromObject, out Vector3 closest, true))
            {
                distanceFromObject = Vector3.Distance(gameObject.transform.position, clickedObject.transform.position);
                distanceFromClosestPoint = Vector3.Distance(closest, clickedObject.transform.position);
                if (playerMovement != null)
                {
                    playerMovement.MoveTo(clickedObject.transform.position);
                }

                yield return new WaitUntil(() => distanceFromObject <= distanceFromClosestPoint);
                playerMovement.StopMovement();
            }
            yield return new WaitUntil(() => canInteract);
        }

        objectToInspect.ObjectInteraction(this.gameObject);
        StartCoroutine(InspectingObject());
    }


    public IEnumerator InspectingObject()
    {
        yield return new WaitUntil(() => distanceFromObject > minimumDistanceFromObject);
        GameEvents.instance.CancelGameObjectAction();
        clickedObject = null;
    }

    private void CancelAction()
    {
        if(inspectCor != null)
        {
            StopCoroutine(inspectCor);
        }
    }

    private void OnDisable()
    {
        playerControls.Disable();
        GameEvents.instance.OnCancelActions -= CancelAction;
    }

}
