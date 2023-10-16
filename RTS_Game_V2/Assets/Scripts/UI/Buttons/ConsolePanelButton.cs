using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePanelButton : MonoBehaviour
{
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
    }

    private void OnDisable()
    {
        GameEvents.instance.OnConsolePanel -= PanelStatus;
    }
}
