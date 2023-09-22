using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private TimeSaver timeSaver;
    private PlayerSaver inventorySaver;
    private SettingsSaver settingsSaver;

    private string timeSavePath;
    private string inventorySavePath;
    private string settingsSavePath;

    private void Awake()
    {
        // Sprawdzenie, czy istnieje inna instancja SaveManagera
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Utworzenie instancji klas podrzêdnych
        timeSaver = new TimeSaver();
        inventorySaver = new PlayerSaver();
        settingsSaver = new SettingsSaver();

        // Ustawienie œcie¿ek do plików zapisu
        timeSavePath = Path.Combine(Application.persistentDataPath, "timeSave.json");
        inventorySavePath = Path.Combine(Application.persistentDataPath, "inventorySave.json");
        settingsSavePath = Path.Combine(Application.persistentDataPath, "settingsSave.json");
    }

    public void SaveTime()
    {
        timeSaver.SaveTime(timeSavePath);
    }

    public void SavePlayer()
    {
        inventorySaver.SavePlayer(inventorySavePath);
    }

    public void SaveSettings()
    {
        settingsSaver.SaveSettings(settingsSavePath);
    }
}
