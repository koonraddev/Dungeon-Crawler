using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectInteraction : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hitPoint;
    private GameObject pointedObject;
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private LayerMask interactiveObjectMask;

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
    }

    public void CheckObject(GameObject pointedObject)
    {
        if (pointedObject != null)
        {
            if (pointedObject.TryGetComponent<IInteractionObjects>(out IInteractionObjects pointedScript))
            {
                pointedScript.OnMouseEnterObject(highLightObjectColor);
                if (Input.GetMouseButtonDown(0))
                {
                    pointedScript.ObjectInteraction();
                }
            }
        }
    }

    public void UncheckObject(GameObject pointedObject)
    {
        if(pointedObject != null)
        {
            if (pointedObject.TryGetComponent<IInteractionObjects>(out IInteractionObjects pointedScript))
            {
                pointedScript.OnMouseExitObject();
            }
        }
    }
}
