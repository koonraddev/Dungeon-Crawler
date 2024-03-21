using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] panelSlots;
    [SerializeField] private Image slotPanel;

    void Start()
    {
        GameEvents.instance.OnInventoryUpdate += OnInventoryUpdate;
        OnInventoryUpdate();
    }

    private void OnInventoryUpdate()
    {
        List<InventorySlot> items = InventoryManager.instance.Slots;
        if(items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                InventorySlotPanel invInter = panelSlots[i].GetComponent<InventorySlotPanel>();

                if (!items[i].Empty)
                {
                    invInter.SetInventorySlotUI(items[i].Item, items[i].Amount, Color.white);
                }
                else
                {
                    invInter.SetEmptySlot();
                }
            }
        }
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnInventoryUpdate -= OnInventoryUpdate;
    }
}
