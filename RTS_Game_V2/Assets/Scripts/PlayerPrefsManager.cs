using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPrefsManager : MonoBehaviour
{
    [System.Serializable]
    private class PlayerPrefsPreset
    {
        [SerializeField] private PlayerPrefKey key;
        [SerializeField] private string keyName;
        [SerializeField] private int keyValue;

        public PlayerPrefKey Key { get => key; }
        public string KeyName { get => keyName; }
        public int KeyValue { get => keyValue; }
    }


    [Header("Player Prefs Keys")]
    [Space(10)]

    [SerializeField] private List<PlayerPrefsPreset> presetsList;

    private static Dictionary<PlayerPrefKey, string> playerPrefsKeysDict;
    private static PlayerPrefsManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

        }

        DontDestroyOnLoad(gameObject);

        playerPrefsKeysDict = new();

        foreach (PlayerPrefKey key in Enum.GetValues(typeof(PlayerPrefKey)))
        {
            if (!playerPrefsKeysDict.ContainsKey(key))
            {

                try
                {
                    PlayerPrefsPreset preset = presetsList.First(x => x.Key == key);
                    playerPrefsKeysDict.Add(key, preset.KeyName);
                }
                catch (Exception)
                {
                    playerPrefsKeysDict.Add(key, key.ToString());
                    continue;
                }
            }
        }

        foreach (PlayerPrefKey key in Enum.GetValues(typeof(PlayerPrefKey)))
        {
            if (!PlayerPrefs.HasKey(GetKeyString(key)))
            {
                try
                {
                    PlayerPrefsPreset preset = presetsList.First(x => x.Key == key);
                    SetPlayerPref(key, preset.KeyValue);
                }
                catch (Exception)
                {
                    SetPlayerPref(key, -1);
                    continue;
                }
            }
        }
    }


    private static string GetKeyString(PlayerPrefKey keyType)
    {
        return playerPrefsKeysDict[keyType];
    }

    public static void SetPlayerPref(PlayerPrefKey prefKey, int prefValue)
    {
        PlayerPrefs.SetInt(GetKeyString(prefKey), prefValue);
        PlayerPrefs.Save();
    }

    public static int GetPrefValue(PlayerPrefKey prefKey)
    {
        return PlayerPrefs.GetInt(playerPrefsKeysDict[prefKey]);
    }

}
