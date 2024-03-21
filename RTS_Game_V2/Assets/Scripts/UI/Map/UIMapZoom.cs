using UnityEngine;
using UnityEngine.EventSystems;

public class UIMapZoom : MonoBehaviour, IScrollHandler
{
    private Vector3 initialScale;

    [SerializeField] private float zoomSpeed, maxZoom;
    [SerializeField] private float minimapScale;
    public bool fullSizeMode = false;

    private Vector3 targetPos;

    public Vector3 TargetRoomPos { get => targetPos; set => targetPos = value; }

    private void Awake()
    {
        initialScale = transform.localScale;
        transform.localScale = new(minimapScale, minimapScale, minimapScale);
    }
    private void OnEnable()
    {
        GameEvents.instance.OnMapPanel += SwitchMapMode;
    }
    private void SwitchMapMode(bool fullSizeMode)
    {
        this.fullSizeMode = fullSizeMode;
    }

    private void Update()
    {
        if (!fullSizeMode)
        {
            transform.localScale = new(minimapScale, minimapScale, minimapScale);
            transform.localPosition = targetPos * (-minimapScale);
        }
    }
    public void OnScroll(PointerEventData eventData)
    {
        if (fullSizeMode)
        {
            var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
            var desiredScale = transform.localScale + delta;

            desiredScale = ClampDesiredScale(desiredScale);

            transform.localScale = desiredScale;
        }
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initialScale, desiredScale);
        desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
        return desiredScale;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnMapPanel += SwitchMapMode;
    }
}
