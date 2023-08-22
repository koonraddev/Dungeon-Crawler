using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerObjectInteraction : MonoBehaviour
{
    private GameObject pointedObject;
    private GameObject clickedObject;
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private LayerMask interactiveObjectMask;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private int minimumDistanceFromObject;
    private float distanceFromObject;

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
        moveInspectAction = playerControls.BasicMovement.Inspect;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hitPoint, 1000f, interactiveObjectMask))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (pointedObject != hitPoint.transform.gameObject)
                {
                    UncheckObject(pointedObject);
                }
                pointedObject = hitPoint.transform.gameObject;
                if (pointedObject != null)
                {
                    CheckObject(pointedObject);
                }
            }
        }
        else
        {
            UncheckObject(pointedObject);
        }

        if (clickedObject != null)
        {
            distanceFromObject = Vector3.Distance(gameObject.transform.position, clickedObject.transform.position);
        }
    }

    public void CheckObject(GameObject pointedObject)
    {
        if (pointedObject != null && pointedObject.TryGetComponent<IInteractionObjects>(out IInteractionObjects pointedScript))
        {
            pointedScript.OnMouseEnterObject(highLightObjectColor);
            if (moveInspectAction.IsPressed())
            {
                StopAllCoroutines();
                clickedObject = pointedObject;
                StartCoroutine(InspectObject(pointedScript));
            }
        }
    }

    public void UncheckObject(GameObject pointedObject)
    {
        if (pointedObject != null && pointedObject.TryGetComponent<IInteractionObjects>(out IInteractionObjects pointedScript))
        {
            pointedScript.OnMouseExitObject();
        }
    }

    public IEnumerator InspectObject(IInteractionObjects objectToInspect)
    {
        if (distanceFromObject > 3 && playerMovement != null) 
        {
            playerMovement.MoveTo(clickedObject.transform.position);
        }
        yield return new WaitUntil(() => distanceFromObject <= minimumDistanceFromObject);

        objectToInspect.ObjectInteraction();
        StartCoroutine(IfInspectingObject());
    }

    public IEnumerator IfInspectingObject()
    {
        yield return new WaitUntil(() => distanceFromObject >= minimumDistanceFromObject);
        GameEvents.instance.InventoryPanel(false);
        GameEvents.instance.CancelGameObjectAction();
    }
}
