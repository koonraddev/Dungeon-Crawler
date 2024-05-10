using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class GraphicsQualityDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    string[] qualitySettingsNames;
    public int QualityLevel { get => dropdown.value; }

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        qualitySettingsNames = QualitySettings.names;

        dropdown.ClearOptions();

        List<string> options = new();
        foreach (var item in qualitySettingsNames)
        {
            options.Add(item);
        }

        dropdown.AddOptions(options);
    }

    private void OnEnable()
    {
        dropdown.value = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.GRAPHICS_QUALITY);
        dropdown.RefreshShownValue();

    }
}
