using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputControler inputControler;

    private InputAction switchScheme;
    public enum InputType
    {
        KeyboardAndMouse,
        Gamepad
    }


    [SerializeField] private InputType startInputType;

    private void Awake()
    {
        inputControler = new InputControler();
    }

    private void OnEnable()
    {
        inputControler.Enable();
    }
    void Start()
    {
        switchScheme = inputControler.SwitchingMap.SwitchScheme;

        switchScheme.performed += ctx => SwitchScheme(ctx);


        GameEvents.instance.SwitchInput(startInputType);
    }

    private void SwitchScheme(InputAction.CallbackContext ctx)
    {
        var device = ctx.action.activeControl.device.displayName;
        switch (device)
        {
            case "Xbox Controller":
                GameEvents.instance.SwitchInput(InputType.Gamepad);
                break;
            case "Keyboard":
            case "Mouse":
                GameEvents.instance.SwitchInput(InputType.KeyboardAndMouse);
                break;
            default:
                break;
        }

        /*
        var control = ctx.control.ToString();
        string[] controlElements = control.ToString().Split('/');
        switch (controlElements[1])
        {
            case "XInputControllerWindows":
                GameEvents.instance.SwitchInput(InputType.Gamepad);
                break;
            case "Keyboard":
            case "Mouse":
                GameEvents.instance.SwitchInput(InputType.KeyboardAndMouse);
                break;
            default:
                break;
        }
        */
    }
    private void OnDisable()
    {
        inputControler.Disable();
        switchScheme.performed -= ctx => SwitchScheme(ctx);
    }
}
