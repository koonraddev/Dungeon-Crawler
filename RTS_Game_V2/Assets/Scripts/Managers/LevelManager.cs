using System.Collections;
using System.Collections.Generic;
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
        set { currentLevel = value +1; }
    }

    private void OnEnable()
    {
        GameEvents.instance.OnSavedPlayerData += LoadNextLevel;
        GameEvents.instance.OnLoadedPlayerData += LoadLevel;
    }

    private void LoadLevel()
    {
        OnLoadingStuff();
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            GameEvents.instance.ActivateTeleport();
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnSavedPlayerData -= LoadLevel;
        GameEvents.instance.OnLoadedPlayerData -= LoadLevel;
    }
}