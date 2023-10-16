using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    private bool inventoryPanelStatus;

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
    }

    private void OnDisable()
    {
        GameEvents.instance.OnInventoryPanelOpen -= PanelStatus;
    }
}
