using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Toggle), typeof(TMP_InputField), typeof(InvertedToggleDynamicBoolLogic))]
public class FrameLimiter : MonoBehaviour
{
    [SerializeField] private Toggle limiterToggle;
    [SerializeField] private TMP_InputField limiterInputField;

    public bool FrameLimitStatus
    {
        get => limiterToggle.isOn;
    }
    public int FrameLimitValue
    {
        get
        {
            if (limiterToggle.isOn)
            {
                if (int.TryParse(limiterInputField.text, out int parsedValue))
                {
                    return parsedValue;
                }
            }
            return 9999;
        }
    }

    private void Update()
    {
        if (limiterToggle.interactable && limiterToggle.isOn)
        {
            limiterInputField.interactable = true;
        }
        else
        {
            limiterInputField.interactable = false;
        }

        if (!limiterToggle.interactable)
        {
            limiterToggle.isOn = false;
        }

    }

    private void OnEnable()
    {
        int limiterStatus = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.FPS_LIMITER_STATUS);
        int limiterValue = PlayerPrefsManager.GetPrefValue(PlayerPrefKey.FPS_LIMITER_VALUE);

        limiterToggle.isOn = limiterStatus == 1;
        limiterInputField.text = limiterStatus == 1 ? limiterValue.ToString() : "";
    }
}
