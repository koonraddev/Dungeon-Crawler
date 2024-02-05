using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDropSlot : MonoBehaviour, IDropHandler, ISpecialInventoryPanel
{
    private InventorySlotPanel invSlotPanel;
    public InventorySlotPanel RequestingSlot { get => invSlotPanel; }
    private Dictionary<string, string> contentToDisplay;
    [SerializeField] private GameObject dropObjectPrefab;
    private GameObject playerObject;

    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }
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

    private void DropAllSlotItems()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (playerObject != null)
        {
            InventorySlot invSlot = InventoryManager.instance.Slots[invSlotPanel.SlotNumber];

            GameObject dropBagObject = Instantiate(dropObjectPrefab);
            dropBagObject.transform.position = playerObject.transform.position + new Vector3(0f, 1f, 0f);
            DropBag dropBag = dropBagObject.GetComponent<DropBag>();

            dropBag.SetDropBag(invSlot.Item, invSlot.Amount);

            InventoryManager.instance.ClearSlot(invSlotPanel.SlotNumber);
            invSlotPanel = null;
        }
    }

    public void DoSpecialIntercation()
    {
        DropAllSlotItems();
    }
}
