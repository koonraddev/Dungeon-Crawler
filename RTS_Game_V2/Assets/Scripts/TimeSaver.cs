using System.IO;
using UnityEngine;

public class TimeSaver
{
    [SerializeField] private float totalTime;
    [SerializeField] private float lastSessionTime;

    public void SaveTime(string savePath)
    {
        // Logika zapisu czasu

        // Serializacja danych do formatu JSON
        string jsonData = JsonUtility.ToJson(this);

        // Zapis danych do pliku
        File.WriteAllText(savePath, jsonData);
    }
}