using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    private static GameSettingsManager instance;
    void Awake()
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

        int resWIDTH = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.RES_WIDTH);
        int resHEIGHT = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.RES_HEIGHT);
        int fullscreen = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.FULLSCREEN);
        int fpsLimiterStatus = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.FPS_LIMITER_STATUS);
        int fpsLimiterValue = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.FPS_LIMITER_VALUE);
        int graphicqQuality = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.GRAPHICS_QUALITY);
        int vsync = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.VSYNC);

        Resolution currRes = Screen.currentResolution;

        #region RESOLUTION & FULLSCREEN CHECK
        if (resHEIGHT <= 0 || resHEIGHT > currRes.height || resWIDTH <= 0 || resWIDTH > currRes.width)
        {
            GraphicSettings.SetResolution(currRes.width, currRes.height, fullscreen == 1);
        }
        else
        {
            GraphicSettings.SetResolution(resWIDTH, resHEIGHT, fullscreen == 1);
        }
        #endregion

        #region FPS LIMITER CHECK
        bool limiterON = fpsLimiterStatus == 1;
        if(fpsLimiterValue < 30)
        {
            GraphicSettings.SetFrameRateLimiter(limiterON);
        }
        else
        {
            GraphicSettings.SetFrameRateLimiter(limiterON, fpsLimiterValue);
        }
        #endregion

        #region GRAPHICS QUALITY CHECK
        if(graphicqQuality < 0)
        {
            graphicqQuality = 0;
        }

        GraphicSettings.SetQuality(graphicqQuality);
        #endregion

        #region VSYNC CHECK
        if(vsync < 0)
        {
            vsync = 0;
        }

        GraphicSettings.SetVSync(vsync);
        #endregion
    }
}