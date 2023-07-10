using System.IO;
using UnityEngine;

public class SettingsSaver
{
    [SerializeField] private Resolution resolution;
    [SerializeField] private float musicVolume;
    [SerializeField] private float soundVolume;

    public void SaveSettings(string savePath)
    {
        // Logika zapisu ustawień

        // Serializacja danych do formatu JSON
        string jsonData = JsonUtility.ToJson(this);

        // Zapis danych do pliku
        File.WriteAllText(savePath, jsonData);
    }
}