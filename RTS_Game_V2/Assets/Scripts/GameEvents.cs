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

    public void UpdateInventoryUI()
    {
        OnInventoryUpdate?.Invoke();
    }

    //public event Action onShowMessage;
    public event Action<int> OnCloseMessage;

    public void CloseMessage(int i)
    {
        OnCloseMessage?.Invoke(i);
    }

    public event Action<InputManager.InputType> OnInputChange;

    public void SwitchInput(InputManager.InputType inputType)
    {
        OnInputChange?.Invoke(inputType);
    }
    
}
