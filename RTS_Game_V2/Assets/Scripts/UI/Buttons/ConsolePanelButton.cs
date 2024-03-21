using UnityEngine;
using UnityEngine.UI;

public class ConsolePanelButton : MonoBehaviour
{
    [SerializeField] private Color panelOnColor, panelOFFColor;
    [SerializeField] private Image statusIcon;
    private bool consolePanelStatus;

    private void OnEnable()
    {
        GameEvents.instance.OnConsolePanel += PanelStatus;
    }

    public void ConsoleButtonClick()
    {
        GameEvents.instance.ConsolePanel(!consolePanelStatus);
    }

    private void PanelStatus(bool status)
    {
        consolePanelStatus = status;
        statusIcon.color = consolePanelStatus ? panelOnColor : panelOFFColor;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnConsolePanel -= PanelStatus;
    }
}
