using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    Resolution[] resolutions;

    public Resolution PickedResolution
    {
        get
        {
            if (resolutions != null)
            {
                return resolutions[dropdown.value];
            }
            else
            {
                return Screen.currentResolution;
            }
        }
    }
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        GetGameResolution();
    }
    private void GetGameResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        dropdown.ClearOptions();
        List<string> options = new();

        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResIndex;
        dropdown.RefreshShownValue();

    }
}
