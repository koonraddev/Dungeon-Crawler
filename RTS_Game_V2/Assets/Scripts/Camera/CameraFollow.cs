using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private Transform targetObject;
    [SerializeField] private Vector3 localOffset;
    [SerializeField] private float camDistanceMin;
    [SerializeField] private float camDistanceMax;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float zoomSensitivity;
    [SerializeField] private bool canZoom;
    [SerializeField] private float minFov;
    [SerializeField] private float baseFov;
    [SerializeField] private float maxFov;
    private Camera cam;

    private CameraControls cameraControls;
    private InputAction zoomAction;
    private InputAction zoomActionGamepad;

    private Vector3 directionToObject;

    private string inputTypeString;
    private float dist;
    private float input;

    [SerializeField] private ZoomType zoomType;
    public enum ZoomType
    {
        POSITION,
        FOV
    }
    private void Awake()
    {
        cameraControls = new CameraControls();
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
    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        cam.fieldOfView = baseFov;

        targetObject = gameObject.transform.parent.gameObject.transform;
        zoomAction = cameraControls.MainCamera.ZoomMouse;
        zoomActionGamepad = cameraControls.MainCamera.ZoomGamepad;



        transform.localPosition = localOffset;
        directionToObject = (transform.position - targetObject.position).normalized;
        dist = Vector3.Distance(transform.position, targetObject.position);

        GameEvents.instance.OnInputChange += ChangeInput;
    }

    private void LateUpdate()
    {
        transform.LookAt(targetObject);
        input = 0;

        if (inputTypeString == "Keyboard")
        {
            input = zoomAction.ReadValue<float>() * scrollSpeed;
        }

        if(inputTypeString == "Xbox Controller")
        {
            input = zoomActionGamepad.ReadValue<float>() * zoomSensitivity;
        }

        switch (zoomType)
        {
            case ZoomType.POSITION:
                dist -= input;
                dist = Mathf.Clamp(dist, camDistanceMin, camDistanceMax);
                transform.localPosition = directionToObject * dist;

                break;
            case ZoomType.FOV:
                cam.fieldOfView -= input;
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);

                break;
            default:
                break;
        }
    }

    private void ChangeInput(InputManager.InputType inputType)
    {
        switch (inputType)
        {
            case InputManager.InputType.KeyboardAndMouse:
                inputTypeString = "Keyboard";
                break;
            case InputManager.InputType.Gamepad:
                inputTypeString = "Xbox Controller";
                break;
            default:
                break;
        }
    }
}
