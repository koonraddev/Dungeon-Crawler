using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask pointerMask;
    private Material mat;
    private Color originalColor;
    private Color transparentColor;


    private CursorControls cursorControls;

    private InputAction moveAction;
    private InputAction pointAction;

    private void Awake()
    {
        cursorControls = new CursorControls();
    }

    private void OnEnable()
    {
        cursorControls.Enable();
    }

    private void OnDisable()
    {
        cursorControls.Disable();
    }
    void Start()
    {
        groundMask = LayerMask.NameToLayer("Ground");
        //pointerMask = LayerMask.NameToLayer("Pointer");
        mat = GetComponent<MeshRenderer>().material;
        originalColor = mat.color;
        transparentColor = originalColor;
        transparentColor.a = 0f;


        moveAction = cursorControls.Cursor.Move;
        pointAction = cursorControls.Cursor.Point;

    }

    // Update is called once per frame
    void Update()
    {
        
        /*
        //!EventSystem.current.IsPointerOverGameObject()
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hitPoint, Mathf.Infinity))
        {
            if (hitPoint.collider.gameObject.layer == groundMask.value)
            {
                transform.position = hitPoint.point;
                mat.color = originalColor;
            }
            else
            {
                mat.color = transparentColor;
            }
        }
        */
    }
}
