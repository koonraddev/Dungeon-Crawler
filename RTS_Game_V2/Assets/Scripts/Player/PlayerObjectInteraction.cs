using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObjectInteraction : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hitPoint;
    private GameObject pointedObject;
    private GameObject clickedObject;
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private LayerMask interactiveObjectMask;
    [SerializeField] private PlayerMovement playerMovement;
    private float distanceFromObject;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (Physics.Raycast(ray, out hitPoint, 1000f, interactiveObjectMask))
        {
            if (!isOverUI)
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
            if (Mouse.current.leftButton.wasPressedThisFrame)
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
        distanceFromObject = Vector3.Distance(gameObject.transform.position, clickedObject.transform.position);
        if (distanceFromObject > 3 && playerMovement != null) 
        {
            playerMovement.MoveTo(clickedObject.transform.position);
        }
        yield return new WaitUntil(() => distanceFromObject <= 3);

        objectToInspect.ObjectInteraction();
    }
}
