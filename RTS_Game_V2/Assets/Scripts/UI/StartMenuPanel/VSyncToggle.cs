using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Toggle))]
public class VSyncToggle : MonoBehaviour
{
    private Toggle vSyncToggle;

    private void Awake()
    {
        vSyncToggle = GetComponent<Toggle>();
    }

    public int VSyncValue
    {
        get => vSyncToggle.isOn? 1 : 0;
    }

    private void OnEnable()
    {
        int vSyncStatus = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.VSYNC);
        vSyncToggle.isOn = vSyncStatus == 1;
    }
}
