using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [SerializeField] private NewCharacterPanel newPlayerPanel;
    private Color inactiveButtonColor, activeButtonColor;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button button;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();

        inactiveButtonColor = newPlayerPanel.InactiveButtonColor;
        activeButtonColor = newPlayerPanel.ActiveButtonColor;
    }

    public void ActivateButton()
    {
        buttonImage.color = activeButtonColor;
        button.enabled = true;
    }

    public void DeactivateButton()
    {
        buttonImage.color = inactiveButtonColor;
        button.enabled = false;
    }
}
