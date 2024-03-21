using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int currentLevel;

    private void Awake()
    {
        instance = this;
    }
    public int Level
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }

    private void OnEnable()
    {
        GameEvents.instance.OnSavedPlayerData += LoadNextLevel;
        GameEvents.instance.OnLoadedPlayerData += LoadNextLevel;
    }

    private void LoadNextLevel()
    {
        currentLevel++;
        OnLoadingStuff();
    }

    private void OnLoadingStuff()
    {
        RoomsGenerator.instance.SetRoomsGenerator(currentLevel);
        GameEvents.instance.LevelSettingsSet();
    }

    private void OnDisable()
    {
        GameEvents.instance.OnSavedPlayerData -= LoadNextLevel;
        GameEvents.instance.OnLoadedPlayerData -= LoadNextLevel;
    }
}
