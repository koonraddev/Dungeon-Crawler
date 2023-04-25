using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovingObject : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private bool gameON = true;
    private void FixedUpdate()
    {
        if (gameON)
        {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");

            Vector3 forward = inputVertical * transform.forward * Time.deltaTime * movementSpeed;
            Vector3 right = inputHorizontal * transform.right * Time.deltaTime * movementSpeed;
            Vector3 pos = forward + right;
            transform.Translate(pos, Space.World);

            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(new Vector3(0f, rotationSpeed, 0f));
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(new Vector3(0f, -rotationSpeed, 0f));
            }
        }
    }
}
