using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }
    public event Action OnInventoryUpdate;
    public event Action<int> OnCloseMessage;
    public event Action<InputManager.InputType> OnInputChange;
    public event Action<GameController.GameStatus> OnGameStatusChange;
    public event Action OnPrepareGame;

    public void UpdateInventoryUI()
    {
        OnInventoryUpdate?.Invoke();
    }

    public void CloseMessage(int i)
    {
        OnCloseMessage?.Invoke(i);
    }

    public void SwitchInput(InputManager.InputType inputType)
    {
        OnInputChange?.Invoke(inputType);
    }

    public void ChangeGameStatus(GameController.GameStatus gameStatus)
    {
        OnGameStatusChange?.Invoke(gameStatus);
    }

    public void PrepareGame()
    {
        OnPrepareGame?.Invoke();
    }
}
