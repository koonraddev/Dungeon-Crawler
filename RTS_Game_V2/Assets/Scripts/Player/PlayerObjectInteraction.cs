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
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    private int minimumDistanceFromObject;
    private float distanceFromObject;

    private PlayerControls playerControls;
    private InputAction moveInspectAction;

    private IEnumerator inspectCor;
    private bool canInteract;
    public GameObject hitted;
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
        if (Physics.Raycast(ray, out RaycastHit hitPoint))
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
            Vector3 dir = clickedObject.transform.position - transform.position;
            Ray playerRay = new(this.transform.position, dir);
            //Debug.DrawRay(transform.position, dir, Color.yellow);
            if (Physics.Raycast(playerRay, out RaycastHit hitObject))
            {
                Vector3 dir2 = hitObject.point - transform.position;
                hitted = hitObject.transform.gameObject;

                //
                //if (hitObject.transform.gameObject.CompareTag("Wall"))
                //{
                //    canInteract = false;
                //}
                //else
                //{
                //    canInteract = true;
                //}

                //if (hitObject.transform.gameObject == clickedObject)
                //{
                //    distanceFromObject = Vector3.Distance(gameObject.transform.position, hitObject.point);
                //    canInteract = true;
                //    Debug.DrawRay(transform.position, dir2, Color.green);
                //}
                //else
                //{
                //    canInteract = false;
                //    Debug.DrawRay(transform.position, dir2, Color.red);
                //}
            }
        }
    }

    public void CheckObject(GameObject pointedObject)
    {
        //Debug.Log("Check");
        if (pointedObject != null)
        {
            if(pointedObject.TryGetComponent(out IInteractionObject pointedScript))
            {
                pointedScript.OnMouseEnterObject(highLightObjectColor);
                if (moveInspectAction.IsPressed())
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

    public void UncheckObject(GameObject pointedObject)
    {
        //Debug.Log("Uncheck");
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
        //Debug.Log("INSPECt");
        if (distanceFromObject > minimumDistanceFromObject && playerMovement != null)
        {
            Vector3 dirToTarget = clickedObject.transform.position - this.transform.position;
            Vector3 dirToTargetNorm = dirToTarget.normalized;
            float distToTarget = distanceFromObject;// dirToTarget.magnitude;
            float distToMove = Mathf.Ceil(distToTarget - minimumDistanceFromObject);
            Vector3 pointToMove = this.transform.position + dirToTargetNorm * distToMove;
            playerMovement.MoveTo(pointToMove);
            //Debug.Log("NEED MOVe");
        }
        yield return new WaitUntil(() => (distanceFromObject <= minimumDistanceFromObject));// && (canInteract));
        //Debug.Log("CAN INSPECt");
        playerMovement.StopMovement();
        objectToInspect.ObjectInteraction(this.gameObject);
        StartCoroutine(InspectingObject());
    }

    public IEnumerator InspectingObject()
    {
        //Debug.Log("inspecting");
        yield return new WaitUntil(() => (distanceFromObject > minimumDistanceFromObject));// || (!canInteract));
        //Debug.Log("distance > min distance");
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

}
