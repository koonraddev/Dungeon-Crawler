using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractiveObject
{
    [SerializeField] Color highLightColor;
    private Dictionary<string, string> contentToDisplay;
    private int interactionDistance = 3;
    private bool activated = false;
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
        SetContentToDisplay(new Dictionary<string, string> { { "Name", "Teleport" } });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
    }

    private void OnMouseExit()
    {
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
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
