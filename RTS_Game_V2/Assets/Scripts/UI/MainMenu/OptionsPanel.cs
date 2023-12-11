using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private ResolutionDropdown resDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMPro.TMP_Dropdown graphicsQualityDropdown;
    [SerializeField] private FrameLimiter frameLimiter;
    [SerializeField] private ButtonManager applyButton;

    private void OnEnable()
    {
        applyButton.DeactivateButton();
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("limitedFPS")) { PlayerPrefs.SetInt("limitedFPS", 0); }
        if (!PlayerPrefs.HasKey("graphicsQuality")) { PlayerPrefs.SetInt("graphicsQuality", 0); }
        if (!PlayerPrefs.HasKey("FPSlimiter")) { PlayerPrefs.SetInt("FPSlimiter", 60); }
        if (!PlayerPrefs.HasKey("fullscreen")) { PlayerPrefs.SetInt("fullscreen", 1); }
        if (!PlayerPrefs.HasKey("resolutionWidth")) { PlayerPrefs.SetInt("resolutionWidth", 1920); }
        if (!PlayerPrefs.HasKey("resolutionHeight")) { PlayerPrefs.SetInt("resolutionHeight", 1080); }
        ApplySettings();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplySettings()
    {
        ApplyQuality();
        ApplyResolution();
        PlayerPrefs.Save();
    }

    private void ApplyResolution()
    {
        Resolution newResolution = resDropdown.PickedResolution;
        int resWidth = newResolution.width;
        int resHeight = newResolution.height;
        int frameLimit = frameLimiter.FrameLimit;
        int fullscreenINT = fullscreenToggle.isOn ? 1 : 0;

        PlayerPrefs.SetString("resolutionWidth", resWidth.ToString());
        PlayerPrefs.SetString("resolutionHeight", resHeight.ToString());
        PlayerPrefs.SetInt("fullscreen", fullscreenINT);
        PlayerPrefs.SetInt("limitedFPS", frameLimit);


        Screen.SetResolution(resWidth, resHeight, fullscreenToggle.isOn); ;
        Application.targetFrameRate = frameLimit;
    }

    private void ApplyQuality()
    {
        PlayerPrefs.SetInt("graphicsQuality", graphicsQualityDropdown.value);
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.value);
    }
}
