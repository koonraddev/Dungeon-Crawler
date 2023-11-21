using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIEffects : MonoBehaviour
{
    public TMP_Text textInputObj;
    public TMP_Text textGameStatusObj;
    void Start()
    {
        GameEvents.instance.OnInputChange += ChangeInput;
        GameEvents.instance.OnGameStatusChange += ChangeGameStatus;
    }

    private void ChangeGameStatus(GameStatus gameStatus)
    {
        switch (gameStatus)
        {
            case GameController.GameStatus.START:
                textGameStatusObj.text = "GAME START";
                break;
            case GameController.GameStatus.RUN:
                textGameStatusObj.text = "GAME RUN";
                break;
            case GameController.GameStatus.PAUSE:
                textGameStatusObj.text = "GAME PAUSE";
                break;
            case GameController.GameStatus.END:
                textGameStatusObj.text = "GAME END";
                break;
            default:
                break;
        }
    }


    private void ChangeInput(InputManager.InputType inputType)
    {
        switch (inputType)
        {
            case InputManager.InputType.KeyboardAndMouse:
                textInputObj.text = "keyboard and mouse";
                break;
            case InputManager.InputType.Gamepad:
                textInputObj.text = "gamepad";
                break;
            default:
                break;
        }
    }
}
