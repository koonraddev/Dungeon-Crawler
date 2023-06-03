using UnityEngine;
using UnityEngine.InputSystem;

public class CursorsController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private CursorControls cursorControls;
    private InputAction moveAction;

    public Vector2 startPosition;

    private int screenWidth;
    private int screenHeight;


    private void Awake()
    {
        cursorControls = new CursorControls();
        moveAction = cursorControls.Cursor.Move;
    }

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameEvents.instance.OnInputChange += ChangeInput;
    }

    private void OnEnable()
    {
        cursorControls.Enable();
    }
    private void OnDisable()
    {
        cursorControls.Disable();
        GameEvents.instance.OnInputChange -= ChangeInput;
    }

    void Update()
    {
        startPosition.x = Mathf.Clamp(startPosition.x, 0, screenWidth);
        startPosition.y = Mathf.Clamp(startPosition.y, 0, screenHeight);

        Vector2 mov = moveAction.ReadValue<Vector2>();
        if (moveAction.IsInProgress() && (moveAction.activeControl.device.displayName == "Xbox Controller"))
        {
            Vector2 pos = mov * Time.deltaTime * movementSpeed;
            startPosition += pos;
            Mouse.current.WarpCursorPosition(startPosition);
        }
    }

    private void ChangeInput(InputManager.InputType inputType)
    {
        switch (inputType)
        {
            case InputManager.InputType.KeyboardAndMouse:
                break;
            case InputManager.InputType.Gamepad:
                startPosition = Mouse.current.position.ReadValue();
                break;
            default:
                break;
        }
    }
}
