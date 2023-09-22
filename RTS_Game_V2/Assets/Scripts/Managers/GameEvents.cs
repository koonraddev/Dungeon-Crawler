using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }


    public event Action OnChestUpdate;
    public event Action<int> OnCloseMessage;
    public event Action<InputManager.InputType> OnInputChange;
    public event Action<GameController.GameStatus> OnGameStatusChange;
    public event Action OnCancelGameObjectAction;
    public event Action OnPrepareGame;
    public event Action<int> OnSpawn;
    public event Action<bool> OnInventoryPanelOpen;
    public event Action<bool> OnInformationPanel;
    public event Action<bool> OnStatisticPanel;

    public event Action OnCanSave;
    public event Action OnCannotSave;
    public event Action OnLoadLevel;
    public event Action OnExitToMenu;
    public event Action OnEndGeneratingLevel;

    public event Action OnInventoryUpdate;
    public event Action OnEquipmentUpdate;
    public event Action<StatisticType,float> OnStatisticUpdate;


    public event Action<float> onUpdateCurrentHP;
    public event Action<float> onUpdateMaxHP;

    //Spawns ready status
    //public event Action OnLastSpawnPoint
    public event Action OnLastRoomReady;


    public void UpdateCurrentHP(float value)
    {
        onUpdateCurrentHP?.Invoke(value);
    }


    public void LastRoomReady()
    {
        OnLastRoomReady?.Invoke();
    }

    public void InformationPanel(bool activePanel)
    {
        OnInformationPanel?.Invoke(activePanel);
    }    
    public void StatisticPanel(bool activePanel)
    {
        OnStatisticPanel?.Invoke(activePanel);
    }

    public void Spawn(int id)
    {
        OnSpawn?.Invoke(id);
    }

    public void InventoryUpdate()
    {
        OnInventoryUpdate?.Invoke();
    }

    public void EquipmentUpdate()
    {
        OnEquipmentUpdate?.Invoke();
    }
    

    public void StatisticUpdate(StatisticType statisticType, float value)
    {
        OnStatisticUpdate?.Invoke(statisticType, value);
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

    public void InventoryPanel(bool activePanel)
    {
        OnInventoryPanelOpen?.Invoke(activePanel);
    }

    public void CancelGameObjectAction()
    {
        OnCancelGameObjectAction?.Invoke();
    }

    public void ChestUpdate()
    {
        OnChestUpdate?.Invoke();
    }

    public void EnableSave()
    {
        OnCanSave?.Invoke();
    }

    public void DisableSave()
    {
        OnCannotSave?.Invoke();
    }

    public void LoadLevel()
    {
        OnLoadLevel?.Invoke();
    }

    public void ExitToMenu()
    {
        OnExitToMenu?.Invoke();
    }

    public void EndGeneratingLevel()
    {
        OnEndGeneratingLevel?.Invoke();
    }

}
