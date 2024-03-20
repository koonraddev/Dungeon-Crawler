using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDropSlot : MonoBehaviour, IDropHandler, ISpecialInventoryPanel
{
    private InventorySlotPanel invSlotPanel;
    public InventorySlotPanel RequestingSlot { get => invSlotPanel; }
    [SerializeField] private GameObject dropObjectPrefab;
    private GameObject playerObject;
    public GameObject GameObject => playerObject;

    ObjectContent objectContent;
    public ObjectContent ContentDoDisplay => objectContent;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            invSlotPanel = eventData.pointerDrag.GetComponent<InventorySlotPanel>();
            if (invSlotPanel != null)
            {

                objectContent = new(gameObject);
                objectContent.Nametext = invSlotPanel.InvSlot.Item.Name;
                objectContent.Description = invSlotPanel.InvSlot.Amount.ToString();
                objectContent.YesButtonDelegate = DoSpecialIntercation;
                UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.DROP);
            }
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
