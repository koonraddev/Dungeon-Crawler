using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }


    public event Action OnContainerUpdate;
    public event Action<int> OnCloseMessage;
    public event Action<InputManager.InputType> OnInputChange;
    public event Action<GameStatus> OnGameStatusChange;
    public event Action OnCancelGameObjectAction;
    public event Action<int> OnSpawn;

    //UI Panels
    public event Action<bool> OnInventoryPanelOpen;
    public event Action<bool> OnInformationPanel;
    public event Action<bool> OnStatisticPanel;
    public event Action<bool> OnConsolePanel;
    public event Action<bool> OnMapPanel;

    //Menu Panels
    public event Action<bool> OnMenuPanel;
    public event Action<bool> OnMainPanel;
    public event Action<bool> OnOptionsPanel;
    public event Action<bool> OnAskPanel;

    //
    public event Action OnLoadNextLevel;
    public event Action OnLoadLevel;
    public event Action OnLoadGameScene;
    public event Action OnExitToMenu;
    public event Action OnEndGeneratingLevel;

    //Player actions;
    public event Action OnInventoryUpdate;
    public event Action OnEquipmentUpdate;
    public event Action<StatisticType,float> OnStatisticUpdate;
    public event Action<float> OnUpdateCurrentHP;
    public event Action<StatisticType, float, float> OnBuffActivate;
    public event Action<StatisticType, float> OnBuffDeactivate;

    public event Action OnSavedPlayerData;
    public event Action OnLoadedPlayerData;

    public event Action<Enemy> OnEnemyClick;
    public event Action OnCancelActions;
    public event Action OnActivateTeleport;
    public event Action OnLevelSettingsSet;

    //Spawns ready status
    //public event Action OnLastSpawnPoint
    public event Action OnGeneratingReady;
    public event Action OnLastRoomReady;

    public event Action OnSwitchScene;
    public event Action OnPlayerSpawn;
    public event Action OnRestartFloor;

    //Game Status
    public event Action OnPauseGame;
    public event Action OnResumeGame;
    public event Action OnStartLevel;
    public event Action OnGameOver;


    public void GeneratingReady() => OnGeneratingReady?.Invoke();

    public void PasueGame() => OnPauseGame?.Invoke();

    public void ResumeGame() => OnResumeGame?.Invoke();

    public void RestartGame() => OnRestartFloor?.Invoke();

    public void GameOver() => OnGameOver?.Invoke();

    public void LoadGameScene() => OnLoadGameScene?.Invoke();

    public void LevelSettingsSet() => OnLevelSettingsSet?.Invoke();

    public void PlayerSpawn() => OnPlayerSpawn?.Invoke();

    public void StartLevel() => OnStartLevel?.Invoke();

    public void SwitchScene() => OnSwitchScene?.Invoke();

    public void ActivateTeleport() => OnActivateTeleport?.Invoke();

    public void BuffActivate(StatisticType statType, float statValue, float duration) => OnBuffActivate?.Invoke(statType, statValue, duration);

    public void BuffDeactivate(StatisticType statType, float statValue) => OnBuffDeactivate?.Invoke(statType, statValue);

    public void EnemyClick(Enemy enemy) => OnEnemyClick?.Invoke(enemy);

    public void CancelActions() => OnCancelActions?.Invoke();

    public void PlayerDataSaved() => OnSavedPlayerData?.Invoke();

    public void PlayerDataLoaded() => OnLoadedPlayerData?.Invoke();

    public void UpdateCurrentHP(float value) => OnUpdateCurrentHP?.Invoke(value);

    public void LastRoomReady() => OnLastRoomReady?.Invoke();

    public void InformationPanel(bool activePanel) => OnInformationPanel?.Invoke(activePanel);
    
    public void StatisticPanel(bool activePanel) => OnStatisticPanel?.Invoke(activePanel);

    public void ConsolePanel(bool activePanel) => OnConsolePanel?.Invoke(activePanel);

    public void MenuPanel(bool activePanel) => OnMenuPanel?.Invoke(activePanel);

    public void MainPanel(bool activePanel) => OnMainPanel?.Invoke(activePanel);

    public void OptionsPanel(bool activePanel) => OnOptionsPanel?.Invoke(activePanel);

    public void AskPanel(bool activePanel) => OnAskPanel?.Invoke(activePanel);

    public void MapPanel(bool fullSizeMode) => OnMapPanel?.Invoke(fullSizeMode);

    public void Spawn(int id) => OnSpawn?.Invoke(id);

    public void InventoryUpdate() => OnInventoryUpdate?.Invoke();

    public void EquipmentUpdate() => OnEquipmentUpdate?.Invoke();

    public void StatisticUpdate(StatisticType statisticType, float value) => OnStatisticUpdate?.Invoke(statisticType, value);

    public void CloseMessage(int i) => OnCloseMessage?.Invoke(i);

    public void SwitchInput(InputManager.InputType inputType) => OnInputChange?.Invoke(inputType);

    public void ChangeGameStatus(GameStatus gameStatus) => OnGameStatusChange?.Invoke(gameStatus);

    public void InventoryPanel(bool activePanel) => OnInventoryPanelOpen?.Invoke(activePanel);

    public void CancelGameObjectAction() => OnCancelGameObjectAction?.Invoke();

    public void ContainerUpdate() => OnContainerUpdate?.Invoke();

    public void LoadNextLevel() => OnLoadNextLevel?.Invoke();

    public void LoadLevel() => OnLoadLevel?.Invoke();

    public void ExitToMenu() => OnExitToMenu?.Invoke();

    public void EndGeneratingLevel() => OnEndGeneratingLevel?.Invoke();
}
