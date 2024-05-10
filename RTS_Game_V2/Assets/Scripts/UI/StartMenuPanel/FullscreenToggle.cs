using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class FullscreenToggle : MonoBehaviour
{
    private Toggle fullscreenToggle;

    private void Awake()
    {
        fullscreenToggle = GetComponent<Toggle>();
    }

    public bool FullscreenStatus
    {
        get => fullscreenToggle.isOn;
    }

    private void OnEnable()
    {
        int fullscreenINT = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.FULLSCREEN);
        fullscreenToggle.isOn = fullscreenINT == 1;
    }
}
