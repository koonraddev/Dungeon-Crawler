using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        targetObject = gameObject.transform.parent.gameObject.transform;
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
