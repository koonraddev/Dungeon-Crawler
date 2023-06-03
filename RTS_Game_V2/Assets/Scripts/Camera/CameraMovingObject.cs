using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovingObject : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject playerCharacter;
    private bool followPlayerCharacter;
    private bool gameON = true;

    private CameraControls cameraControls;
    private InputAction moveAction;
    private InputAction rotateAction;
    private InputAction lockCameraAction;


    private Vector3 mov;
    private Vector3 forward;
    private Vector3 right;
    private Vector3 pos;

    private string inputTypeString;
    private float rot;
    private void Awake()
    {
        cameraControls = new CameraControls();
        moveAction = cameraControls.MainCamera.Move;
        rotateAction = cameraControls.MainCamera.Rotate;
        lockCameraAction = cameraControls.MainCamera.LockCamera;
    }

    private void OnEnable()
    {
        cameraControls.Enable();
        
    }
    private void OnDisable()
    {
        cameraControls.Disable();
        GameEvents.instance.OnInputChange -= ChangeInput;
    }

    void Start()
    {
        GameEvents.instance.OnInputChange += ChangeInput;
    }

    private void Update()
    {
        //Debug.Log(moveAction.ReadValue<Vector2>());
        if (lockCameraAction.triggered)
        {
            followPlayerCharacter = !followPlayerCharacter;
        }

        if (moveAction.IsInProgress() && (moveAction.activeControl.device.displayName == inputTypeString))
        {
            mov = moveAction.ReadValue<Vector2>();
            forward = mov.y * transform.forward;
            right = mov.x * transform.right;
        }
        else
        {
            forward = Vector3.zero;
            right = Vector3.zero;

        }
        if (rotateAction.IsInProgress() && (rotateAction.activeControl.device.displayName == inputTypeString))
        {
            rot = rotateAction.ReadValue<float>();
        }
        else
        {
            rot = 0;
        }
    }

    private void FixedUpdate()
    {
        if (gameON)
        {
            if(!followPlayerCharacter)
            {
                pos = (forward + right) * Time.fixedDeltaTime * movementSpeed;
                transform.Translate(pos, Space.World);
            }
            transform.Rotate(new Vector3(0f, rot * rotationSpeed, 0f));
        }
    }

    private void LateUpdate()
    {
        if (followPlayerCharacter && playerCharacter != null)
        {
            gameObject.transform.position = playerCharacter.transform.position;
        }
    }

    private void ChangeInput(InputManager.InputType inputType)
    {
        switch (inputType)
        {
            case InputManager.InputType.KeyboardAndMouse:
                inputTypeString = "Keyboard";
                moveAction.expectedControlType = inputTypeString;
                break;
            case InputManager.InputType.Gamepad:
                inputTypeString = "Xbox Controller";
                break;
            default:
                break;
        }
    }
}
