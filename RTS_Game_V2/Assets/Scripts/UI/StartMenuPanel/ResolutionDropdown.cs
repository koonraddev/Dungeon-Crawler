using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(TMP_Dropdown))]
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

    private void OnEnable()
    {
        SetDropdownValue();
    }

    private void SetDropdownValue()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                dropdown.value = i;
                dropdown.RefreshShownValue();
                break;
            }
        }
    }
    private void GetGameResolution()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        dropdown.ClearOptions();
        List<string> options = new();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            options.Add(option);
        }

        dropdown.AddOptions(options);
    }
}
