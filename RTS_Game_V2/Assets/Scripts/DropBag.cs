using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropBag : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private Renderer[] renderers;
    private ContainerSlot bagSlot;
    private int interactionDistance = 3;
    private Dictionary<string, string> contentToDisplay;

    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }

    public void SetDropBag(InventoryItem inventoryItem, int itemAmount)
    {
        bagSlot.Item = inventoryItem;
        bagSlot.Amount = itemAmount;
    }

    public void DoInteraction()
    {
        if (InventoryManager.instance.AddItem(bagSlot.Item as InventoryItem, amount: bagSlot.Amount))
        {
            ConsolePanel.instance.InfoLog("You picked up " + bagSlot.Amount + "x " + bagSlot.Item.Name);
            Destroy(gameObject);
        }
        else
        {
            ConsolePanel.instance.InfoLog("No empty slot in inventory");
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

    public void ObjectInteraction(GameObject interactingObject = null)
    {
        SetContentToDisplay(new Dictionary<string, string> { { "Name", bagSlot.Amount.ToString() + "x " + bagSlot.Item.Name } });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.TAKE);
    }

    private void OnMouseEnter()
    {
        foreach (var item in renderers)
        {
            item.material.DOColor(highLightObjectColor, "_BaseColor", 0.5f).SetAutoKill(true).Play();
        }

        SetContentToDisplay(new Dictionary<string, string> { { "Name", "Bag" } });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
    }

    private void OnMouseExit()
    {
        foreach (var item in renderers)
        {
            item.material.DOColor(Color.white, "_BaseColor", 0.5f).SetAutoKill(true).Play();
        }

        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
    }
}
