using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private Color panelOnColor, panelOFFColor;
    private bool inventoryPanelStatus;
    [SerializeField] private Image statusIcon;
    private void OnEnable()
    {
        GameEvents.instance.OnInventoryPanelOpen += PanelStatus;
    }

    public void InventoryButtonClick()
    {
        GameEvents.instance.InventoryPanel(!inventoryPanelStatus);
    }

    private void PanelStatus(bool status)
    {
        inventoryPanelStatus = status;
        statusIcon.color = inventoryPanelStatus ? panelOnColor : panelOFFColor;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnInventoryPanelOpen -= PanelStatus;
    }
}
