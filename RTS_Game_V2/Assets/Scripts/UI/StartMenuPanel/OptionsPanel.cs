using System;
using UnityEngine;
using System.Collections.Generic;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private ResolutionDropdown resDropdown;
    [SerializeField] private GraphicsQualityDropdown graphicsQualityDropdown;
    [SerializeField] private FrameLimiter frameLimiter;
    [SerializeField] private FullscreenToggle fullscreenToggle;
    [SerializeField] private ButtonManager applyButtonManager;
    [SerializeField] private VSyncToggle vSyncToggle;

    public static OptionsPanel panel;

    private List<Action> actionslist = new();

    private void OnEnable()
    {
        panel = this;
        actionslist.Clear();
        applyButtonManager.DeactivateButton();
    }

    public void FrameLimiterChanged()
    {
        applyButtonManager.ActivateButton();
        AddAction(ApplyFPSlimiterSettings);
    }

    public void FullscreenToggleChanged()
    {
        if (actionslist.Contains(ApplyResolutionSettings))
        {
            return;
        }
        AddAction(ApplyFullscreenSettings);
    }

    public void ResolutionDropdownChanged()
    {
        if (actionslist.Contains(ApplyFullscreenSettings))
        {
            actionslist.Remove(ApplyFullscreenSettings);
        }

        AddAction(ApplyResolutionSettings);
    }

    public void QualityDropdownChanged()
    {
        AddAction(ApplyQualitySettings);
    }

    public void VSyncChanged()
    {
        AddAction(ApplyVSyncSetting);
    }

    private void AddAction(Action newAction)
    {
        if (actionslist.Contains(newAction))
        {
            return;
        }

        actionslist.Add(newAction);
        applyButtonManager.ActivateButton();
    }

    public void ApplySettings()
    {
        foreach (var item in actionslist) { item?.Invoke(); }

        actionslist.Clear();
        applyButtonManager.DeactivateButton();
    }

    private void ApplyVSyncSetting()
    {
        GraphicSettings.SetVSync(vSyncToggle.VSyncValue);
    }

    private void ApplyFullscreenSettings()
    {
        GraphicSettings.SetFullscreen(fullscreenToggle.FullscreenStatus);
    }

    private void ApplyFPSlimiterSettings()
    {
        GraphicSettings.SetFrameRateLimiter(frameLimiter.FrameLimitStatus, frameLimiter.FrameLimitValue);
    }

    private void ApplyResolutionSettings()
    {
        GraphicSettings.SetResolution(resDropdown.PickedResolution, fullscreenToggle.FullscreenStatus);
    }

    private void ApplyQualitySettings()
    {
        GraphicSettings.SetQuality(graphicsQualityDropdown.QualityLevel);
    }

    private void OnDisable()
    {
        actionslist.Clear();
        applyButtonManager.DeactivateButton();
    }
}
