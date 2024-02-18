using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerObjectInteraction : MonoBehaviour
{
    private GameObject pointedObject, clickedObject;
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    private int minimumDistanceFromObject;
    private float distanceFromObject;

    private PlayerControls playerControls;
    private InputAction moveInspectAction;

    private IEnumerator inspectCor;
    private bool canInteract;

    private RaycastHit[] hits = new RaycastHit[5];
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
            distanceFromObject = Vector3.Distance(gameObject.transform.position, clickedObject.transform.position);
            Vector3 dir = clickedObject.transform.position - transform.position;
            Ray playerRay = new(this.transform.position, dir);
            int numHits = Physics.RaycastNonAlloc(playerRay, hits, distanceFromObject);
            if (numHits > 0)
            {
                for (int i = 0; i < numHits; i++)
                {
                    canInteract = hits[i].collider.gameObject.CompareTag("Wall");
                }
            }
        }
    }

    public IEnumerator InspectObject(IInteractiveObject objectToInspect)
    {
        distanceFromObject = Vector3.Distance(gameObject.transform.position, clickedObject.transform.position);
        minimumDistanceFromObject = objectToInspect.InteractionDistance;

        if (!StatisticalUtility.CheckIfTargetInRange(gameObject, clickedObject, objectToInspect.InteractionDistance, out Vector3 pointToMove, true) && playerMovement != null)
        {
            playerMovement.MoveTo(pointToMove);
            yield return new WaitUntil(() => distanceFromObject <= minimumDistanceFromObject);
            playerMovement.StopMovement();
        }

        yield return new WaitUntil(() => canInteract);
        objectToInspect.ObjectInteraction(this.gameObject);
        StartCoroutine(InspectingObject());
    }


    public IEnumerator InspectingObject()
    {
        yield return new WaitUntil(() => distanceFromObject > minimumDistanceFromObject);// || (!canInteract));
        GameEvents.instance.CancelGameObjectAction();
        clickedObject = null;
    }

    private void CancelAction()
    {
        //Debug.Log("Cancel");
        if(inspectCor != null)
        {
            StopCoroutine(inspectCor);
        }
        else
        {

        }
        //GameEvents.instance.CancelGameObjectAction();
    }

    private void OnDisable()
    {
        playerControls.Disable();
        GameEvents.instance.OnCancelActions -= CancelAction;
    }

}
