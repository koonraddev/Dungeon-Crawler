using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject cameraParentTransform;
    public Vector3 lookPos;
    public Quaternion rotation;
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        cameraParentTransform = mainCamera.transform.parent.gameObject;
    }
    void Update()
    {
        lookPos = mainCamera.transform.position - transform.position;

        lookPos.x = 0;
        lookPos.y += cameraParentTransform.transform.rotation.y;
        rotation = Quaternion.LookRotation(lookPos); //* new Quaternion(0, cameraParentTransform.transform.rotation.y, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3);
    }
}
