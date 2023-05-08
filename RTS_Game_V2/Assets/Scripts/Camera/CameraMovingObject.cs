using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovingObject : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private bool gameON = true;

    private PlayerInput camInp;

    private InputAction moveAction;
    private InputAction rotateAction;

    private void Awake()
    {
        camInp = GetComponent<PlayerInput>();
        moveAction = camInp.actions["Move"];
        rotateAction = camInp.actions["Rotate"];
    }

    private void FixedUpdate()
    {
        if (gameON)
        {
            Vector3 mov = moveAction.ReadValue<Vector3>();
            float rot = rotateAction.ReadValue<float>();

            Vector3 forward = mov.z * transform.forward;
            Vector3 right = mov.x * transform.right;
            Vector3 pos = (forward + right) * Time.deltaTime * movementSpeed;

            transform.Translate(pos, Space.World);
            transform.Rotate(new Vector3(0f, rot * rotationSpeed, 0f));
        }
    }
}
