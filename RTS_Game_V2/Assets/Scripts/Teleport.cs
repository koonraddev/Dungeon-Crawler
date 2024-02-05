using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractiveObject
{
    [SerializeField] Color highLightColor;
    private Dictionary<string, string> contentToDisplay;
    private int interactionDistance = 3;
    private bool displayPopup = true;
    private bool activated = false;
    Tween enterTweener, exitTweener;
    public GameObject GameObject => gameObject;
    public int InteractionDistance => interactionDistance;

    public Dictionary<string, string> ContentToDisplay => contentToDisplay;

    private void OnEnable()
    {
        GameEvents.instance.OnActivateTeleport += ActivateTeleport;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnActivateTeleport -= ActivateTeleport;
    }

    private void ActivateTeleport()
    {
        activated = true;
    }
    public void DoInteraction()
    {
        if (activated)
        {
            //teleport logic
            GameEvents.instance.LoadNextLevel();
        }
        else
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Message", "Teleport is unactive. Defeat Dungeon Boss."} });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.INFORMATION);
        }
    }

    public void ObjectInteraction(GameObject interactingObject = null)
    {
        SetContentToDisplay(new Dictionary<string, string> { { "Name", "Teleport" }, { "Description", "Teleport to another level of dungeon."} });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.OPEN);
    }

    private void OnMouseEnter()
    {
        Material[] objectMaterials = gameObject.GetComponent<Renderer>().materials;

        if (objectMaterials != null)
        {
            foreach (Material objMaterial in objectMaterials)
            {
                if (objMaterial.color != highLightColor)
                {
                    if (enterTweener == null)
                    {
                        enterTweener = objMaterial.DOColor(highLightColor, "_Color", 0.5f);
                    }
                    if (exitTweener != null)
                    {
                        exitTweener.Rewind();
                    }

                    enterTweener.Play();
                }
            }
        }

        if (displayPopup)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", "Teleport" } });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
            displayPopup = false;
        }
    }

    private void OnMouseExit()
    {
        Material[] objectMaterials = gameObject.GetComponent<Renderer>().materials;

        if (objectMaterials != null)
        {
            foreach (Material objMaterial in objectMaterials)
            {
                if (objMaterial.color != Color.white)
                {
                    if (exitTweener == null)
                    {
                        exitTweener = objMaterial.DOColor(Color.white, "_Color", 0.5f);
                    }

                    if (enterTweener != null)
                    {
                        enterTweener.Rewind();
                    }

                    exitTweener.Play();
                }
            }
        }

        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
        displayPopup = true;
    }

    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string>();

        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }
}
