using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform targetObject;
    [SerializeField] private Vector3 localOffset;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private bool canZoom;
    [SerializeField] private float minFov;
    [SerializeField] private float maxFov;
    private Camera cam;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        transform.localPosition = localOffset;
        transform.LookAt(targetObject);
        if (canZoom)
        {
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);
        }
    }
}
