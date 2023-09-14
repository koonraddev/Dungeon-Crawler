using System.IO;
using UnityEngine;

public class InventorySaver
{
    private InventorySO inventory;

    public void SaveInventory(string savePath)
    {
        // Logika zapisu ekwipunku

        // Serializacja danych do formatu JSON
        string jsonData = JsonUtility.ToJson(inventory);

        // Zapis danych do pliku
        File.WriteAllText(savePath, jsonData);
    }
}