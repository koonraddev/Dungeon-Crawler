using System.IO;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public string jsonInventory;
    public string jsonEquipment;
    public string jsonBasicStatistics;
}
public class PlayerSaver
{
    private InventorySO inventory;
    private EquipmentSO equipmentSO;
    private PlayerBasicStatisticsSO playerBasicStatisticsSO;

    public void SavePlayer(string savePath)
    {
        // Tworzenie obiektu PlayerData i przypisanie danych
        PlayerData playerData = new PlayerData();
        playerData.jsonInventory = JsonUtility.ToJson(inventory);
        playerData.jsonEquipment = JsonUtility.ToJson(equipmentSO);
        playerData.jsonBasicStatistics = JsonUtility.ToJson(playerBasicStatisticsSO);

        // Serializacja obiektu PlayerData do formatu JSON
        string jsonData = JsonUtility.ToJson(playerData);

        // Zapisanie danych do pliku
        File.WriteAllText(savePath, jsonData);
    }
}