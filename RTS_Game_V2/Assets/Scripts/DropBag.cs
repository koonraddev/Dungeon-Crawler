using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropBag : MonoBehaviour, IInteractionObjects
{
    public ContainerSlot bagSlot;
    private bool displayInfo = true;
    private bool displayMessage = true;
    private Dictionary<string, string> contentToDisplay;

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

    public Dictionary<string, string> GetContentToDisplay()
    {
        return contentToDisplay;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string> { };
        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }

    public void ObjectInteraction()
    {
        if (displayMessage)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", bagSlot.Amount.ToString() + "x " + bagSlot.Item.Name } });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.TAKE);
        }
    }

    public void OnMouseEnterObject(Color highLightColor)
    {
        Material[] objectMaterials;
        objectMaterials = gameObject.GetComponent<Renderer>().materials;
        if (objectMaterials != null)
        {
            foreach (Material objMaterial in objectMaterials)
            {
                if (objMaterial.color != highLightColor)
                {
                    objMaterial.DOColor(highLightColor, "_Color", 0.5f);
                }
            }
        }

        if (displayInfo)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", "Bag"} });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
            displayInfo = false;
        }
    }

    public void OnMouseExitObject()
    {
        Material[] objectMaterials;
        objectMaterials = gameObject.GetComponent<Renderer>().materials;
        if (objectMaterials != null)
        {
            foreach (Material objMaterial in objectMaterials)
            {
                if (objMaterial.color != Color.white)
                {
                    objMaterial.DOColor(Color.white, "_Color", 0.5f);
                }
            }
        }
        displayInfo = true;
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
    }
}
