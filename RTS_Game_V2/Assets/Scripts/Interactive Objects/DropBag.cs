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

    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }

    ObjectContent objectContent;
    public ObjectContent ContentDoDisplay => objectContent;
    public void SetDropBag(InventoryItem inventoryItem, int itemAmount)
    {
        bagSlot = new(0, inventoryItem, itemAmount);
        bagSlot.Item = inventoryItem;
        bagSlot.Amount = itemAmount;

        objectContent = new(gameObject);
        objectContent.Nametext = "Drop Bag";
        objectContent.Description = "Containts " + bagSlot.Amount.ToString() + "x " + bagSlot.Item.Name;
        objectContent.YesButtonDelegate = DoInteraction;
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


    public void ObjectInteraction(GameObject interactingObject = null)
    { 
        UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.TAKE);
    }

    private void OnMouseEnter()
    {
        foreach (var item in renderers)
        {
            item.material.DOColor(highLightObjectColor, "_BaseColor", 0.5f).SetAutoKill(true).Play();
        }

        UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.NAME);
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
