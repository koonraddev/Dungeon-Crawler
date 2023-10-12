using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerObjectInteraction : MonoBehaviour
{
    private GameObject pointedObject;
    private GameObject clickedObject;
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private LayerMask interactiveObjectMask;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    private int minimumDistanceFromObject;
    private float distanceFromObject;

    private PlayerControls playerControls;
    private InputAction moveInspectAction;

    private IEnumerator inspectCor;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        GameEvents.instance.OnCancelActions += CancelAction;
    }
    private void OnDisable()
    {
        playerControls.Disable();
        GameEvents.instance.OnCancelActions -= CancelAction;
    }

    void Start()
    {
        moveInspectAction = playerControls.BasicMovement.Inspect;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hitPoint, 1000f))
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
            //minimumDistanceFromObject = Mathf.Clamp(minimumDistanceFromObject, 0, 10);
        }
    }

    public void CheckObject(GameObject pointedObject)
    {
        if (pointedObject != null)
        {
            if(pointedObject.TryGetComponent(out IInteractionObject pointedScript))
            {
                pointedScript.OnMouseEnterObject(highLightObjectColor);
                if (moveInspectAction.IsPressed())
                {
                    StopAllCoroutines();
                    clickedObject = pointedObject;
                    inspectCor = InspectObject(pointedScript);
                    StartCoroutine(inspectCor);
                }
            }
        }
    }

    public void UncheckObject(GameObject pointedObject)
    {
        if (pointedObject != null)
        {
            if(pointedObject.TryGetComponent(out IInteractionObject pointedScript))
            {
                pointedScript.OnMouseExitObject();
            }
        }
    }

    public IEnumerator InspectObject(IInteractionObject objectToInspect)
    {
        minimumDistanceFromObject = objectToInspect.InteractionDistance;
        //minimumDistanceFromObject = Mathf.Clamp(minimumDistanceFromObject, 0, 10);
        float distToMove = minimumDistanceFromObject;
        if (distanceFromObject > minimumDistanceFromObject && playerMovement != null) 
        {
            Vector3 dirToTarget = clickedObject.transform.position - this.transform.position;
            Vector3 dirToTargetNorm = dirToTarget.normalized;
            float distToTarget = dirToTarget.magnitude;
            distToMove = distToTarget - minimumDistanceFromObject;
            Vector3 pointToMove = this.transform.position + dirToTargetNorm * distToMove;
            playerMovement.MoveTo(pointToMove);
        }
        yield return new WaitUntil(() => distanceFromObject <= distToMove);

        objectToInspect.ObjectInteraction();
        StartCoroutine(InspectingObject());
    }

    public IEnumerator InspectingObject()
    {
        Debug.Log("start");
        yield return new WaitUntil(() => distanceFromObject > minimumDistanceFromObject);
        Debug.Log("distance > min distance");
        GameEvents.instance.CancelGameObjectAction();
        clickedObject = null;
    }

    private void CancelAction()
    {
        Debug.Log("Cancel");
        if(inspectCor != null)
        {
            StopCoroutine(inspectCor);
        }
        else
        {

        }
        //GameEvents.instance.CancelGameObjectAction();
    }

}
