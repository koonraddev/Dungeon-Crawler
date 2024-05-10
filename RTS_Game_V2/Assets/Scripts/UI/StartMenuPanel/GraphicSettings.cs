using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GraphicSettings
{
    public static void SetVSync(int vSyncValue)
    {
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.VSYNC, vSyncValue);
        QualitySettings.vSyncCount = vSyncValue;
    }

    public static void SetFullscreen(bool fullscreen)
    {
        int fsINT = fullscreen ? 1 : 0;
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.FULLSCREEN, fsINT);
        Screen.fullScreen = fullscreen;
    }

    public static void SetFrameRateLimiter(bool turnOnLimiter, int limiterValue = 60)
    {
        int frameLimitStausINT = turnOnLimiter ? 1 : 0;

        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.FPS_LIMITER_STATUS, frameLimitStausINT);
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.FPS_LIMITER_VALUE, limiterValue);

        if (turnOnLimiter)
        {
            Application.targetFrameRate = limiterValue;
        }
    }

    public static void SetResolution(int resolutionWidth, int resolutionHeight, bool fullscreen)
    {
        FullScreenMode fsMode = fullscreen ? FullScreenMode.MaximizedWindow : FullScreenMode.Windowed;
        int fsINT = fullscreen ? 1 : 0;

        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.FULLSCREEN, fsINT);
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.RES_WIDTH, resolutionWidth);
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.RES_HEIGHT, resolutionHeight);

        Screen.SetResolution(resolutionWidth, resolutionHeight, fsMode);
    }

    public static void SetResolution(Resolution resolution, bool fullscreen)
    {
        FullScreenMode fsMode = fullscreen ? FullScreenMode.MaximizedWindow : FullScreenMode.Windowed;
        int fsINT = fullscreen ? 1 : 0;
        int resolutionWidth = resolution.width;
        int resolutionHeight = resolution.height;

        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.FULLSCREEN, fsINT);
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.RES_WIDTH, resolutionWidth);
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.RES_HEIGHT, resolutionHeight);

        Screen.SetResolution(resolutionWidth, resolutionHeight, fsMode);
    }

    public static void SetQuality(int qualityLevel)
    {
        PlayerPrefsManager.SetPlayerPref(PlayerPrefKey.GRAPHICS_QUALITY, qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel, true);
    }
}
