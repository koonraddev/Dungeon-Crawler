using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{

    public static UIFollowMouse instance;
    public RectTransform MovingObject;
    public Vector3 offset;
    public RectTransform BasisObject;
    public Camera cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveObject();
    }

    public void MoveObject()
    {
        if (MovingObject != null && BasisObject != null)
        {
            Vector3 pos = Input.mousePosition + offset;
            pos.z = BasisObject.position.z;
            MovingObject.position = cam.ScreenToWorldPoint(pos);
        }

    }
}

