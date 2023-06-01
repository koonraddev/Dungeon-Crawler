using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private Transform targetObject;
    [SerializeField] private Vector3 localOffset;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private bool canZoom;
    [SerializeField] private float minFov;
    [SerializeField] private float baseFov;
    [SerializeField] private float maxFov;
    private Camera cam;

    private CameraControls cameraControls;
    private InputAction zoomAction;

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
    }
    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        targetObject = gameObject.transform.parent.gameObject.transform;
        zoomAction = cameraControls.MainCamera.Zoom;
    }

    private void LateUpdate()
    {
        transform.localPosition = localOffset;
        transform.LookAt(targetObject);
        cam.fieldOfView -= zoomAction.ReadValue<float>() * scrollSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);
    }
}
