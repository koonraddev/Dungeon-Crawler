using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text nameHolder;
    public TMP_Text amountHolder;
    [HideInInspector] public IInventoryItem Item { get; set; }
    [HideInInspector] public int SlotNumber { get; set; }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item != null)
        {
            Inventory.Instance.RemoveItem(SlotNumber);
        }
    }
}
