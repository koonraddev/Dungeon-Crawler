using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelButton : MonoBehaviour
{
    private bool menuPanelStatus;

    private void OnEnable()
    {
        GameEvents.instance.OnMenuPanel += PanelStatus;
    }

    public void MenuButtonClick()
    {
        GameEvents.instance.MenuPanel(!menuPanelStatus);
    }

    private void PanelStatus(bool status)
    {
        menuPanelStatus = status;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnMenuPanel -= PanelStatus;
    }
}
