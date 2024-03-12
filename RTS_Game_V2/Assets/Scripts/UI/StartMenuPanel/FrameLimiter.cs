using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Toggle limiterToggle;
    [SerializeField] private TMPro.TMP_InputField limiterInputField;
    [SerializeField] private ButtonManager applyButton;
    [SerializeField] private int limiterMinValue;
    private int limiterValue;
    public int FrameLimit
    {
        get
        {
            if (limiterToggle.isOn)
            {
                return limiterValue;
            }
            else
            {
                return 999;
            }
        }
    }

    private void Awake()
    {
        limiterToggle.isOn = PlayerPrefs.GetInt("limitedFPS") != 0;
        limiterValue = PlayerPrefs.GetInt("FPSlimiter");
        limiterInputField.text = limiterValue.ToString();
    }

    private void OnEnable()
    {
        OnLimiterInputField();
    }
    public void OnToggleLimiter()
    {
        if (limiterToggle.isOn)
        {
            limiterInputField.enabled = true;
        }
        else
        {
            limiterInputField.enabled = false;
        }
    }

    public void OnLimiterInputField()
    {
        if (int.TryParse(limiterInputField.text, out int parsedValue) && parsedValue >= limiterMinValue)
        {
            limiterValue = parsedValue;
            applyButton.ActivateButton();
        }
        else
        {
            applyButton.DeactivateButton();
        }
    }
}
