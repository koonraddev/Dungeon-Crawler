using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIEffects : MonoBehaviour
{
    public TMP_Text textObj;
    void Start()
    {
        GameEvents.instance.OnInputChange += ChangeInput;
    }


    private void ChangeInput(InputManager.InputType inputType)
    {
        switch (inputType)
        {
            case InputManager.InputType.KeyboardAndMouse:
                textObj.text = "keyboard and mouse";
                break;
            case InputManager.InputType.Gamepad:
                textObj.text = "gamepad";
                break;
            default:
                break;
        }
    }
}
