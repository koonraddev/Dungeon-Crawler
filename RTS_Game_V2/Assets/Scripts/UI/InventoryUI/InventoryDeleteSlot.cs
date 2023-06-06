using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDeleteSlot : MonoBehaviour, IDropHandler, ISpecialInventoryPanel
{
    private InventorySlotPanel invSlot;
    private Dictionary<string, string> contentToDisplay;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            invSlot = eventData.pointerDrag.GetComponent<InventorySlotPanel>();
            if(invSlot != null)
            {
                SetContentToDisplay(new Dictionary<string, string> { { "Message", (invSlot.SlotNumber + 1).ToString()} });
                UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.DELETE);
            }
        }
    }

    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string> { };
        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }

    public void DeleteAllSlotItems()
    {
        Inventory.Instance.ClearSlot(invSlot.SlotNumber);
        invSlot = null;
    }

    public void DoSpecialIntercation()
    {
        DeleteAllSlotItems();
    }

    public InventorySlotPanel GetRequestingSlot()
    {
        return invSlot;
    }

    public Dictionary<string, string> GetContentToDisplay()
    {
        return contentToDisplay;
    }
}
