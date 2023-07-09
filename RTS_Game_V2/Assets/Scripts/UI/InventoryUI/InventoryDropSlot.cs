using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDropSlot : MonoBehaviour, IDropHandler, ISpecialInventoryPanel
{
    private InventorySlotPanel invSlotPanel;
    private Dictionary<string, string> contentToDisplay;
    [SerializeField] private GameObject dropObjectPrefab;
    private GameObject playerObject;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            invSlotPanel = eventData.pointerDrag.GetComponent<InventorySlotPanel>();
            if (invSlotPanel != null)
            {
                SetContentToDisplay(new Dictionary<string, string> { { "Message", (invSlotPanel.SlotNumber + 1).ToString() } });
                UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.DROP);
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

    public void DropAllSlotItems()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (playerObject != null)
        {
            //drop items
            InventorySO.InventorySlot invSlot = Inventory.Instance.GetInventorySlot(invSlotPanel.SlotNumber);

            IInventoryItem invItem = invSlot.ItemInSlot;
            int itemAmount = invSlot.ItemAmount;

            GameObject dropBagObject = Instantiate(dropObjectPrefab);
            DropBag dropBag = dropBagObject.GetComponent<DropBag>();

            dropBag.invItem = invItem;
            dropBag.invItemAmount = itemAmount;


            dropBagObject.transform.position = playerObject.transform.position + new Vector3(0f,1f,0f);

            //then
            Inventory.Instance.ClearSlot(invSlotPanel.SlotNumber);
            invSlotPanel = null;
        }
    }

    public void DoSpecialIntercation()
    {
        DropAllSlotItems();
    }

    public InventorySlotPanel GetRequestingSlot()
    {
        return invSlotPanel;
    }

    public Dictionary<string, string> GetContentToDisplay()
    {
        return contentToDisplay;
    }
}
