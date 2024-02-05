using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropBag : MonoBehaviour, IInteractionObject
{
    public ContainerSlot bagSlot;
    private bool displayInfo = true;
    private bool displayMessage = true;


    private Dictionary<string, string> contentToDisplay;

    private int interactionDistance = 3;
    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }

    Tween enterTweener, exitTweener;

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

    public void ObjectInteraction(GameObject interactingObject = null)
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
                    if(enterTweener == null)
                    {
                        enterTweener = DOTween.Sequence().Append(objMaterial.DOColor(highLightColor, "_Color", 0.5f));
                    }
                    if(exitTweener != null)
                    {
                        exitTweener.Rewind();
                    }

                    enterTweener.Play();
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
                    if (exitTweener == null)
                    {
                        exitTweener = DOTween.Sequence().Append(objMaterial.DOColor(Color.white, "_Color", 0.5f));
                    }

                    if(enterTweener != null)
                    {
                        enterTweener.Rewind();
                    }

                    exitTweener.Play();
                }
            }
        }
        displayInfo = true;
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
    }
}
