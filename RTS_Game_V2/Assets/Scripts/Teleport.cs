using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractiveObject
{
    [SerializeField] Color highLightColor;
    private int interactionDistance = 3;
    private bool activated = false;
    public GameObject GameObject => gameObject;
    public int InteractionDistance => interactionDistance;

    ObjectContent objectContent;
    public ObjectContent ContentDoDisplay => objectContent;

    private void Awake()
    {
        objectContent = new(gameObject);
        objectContent.Nametext = "Teleport";
        objectContent.Description = "Teleport to the next floor";
        objectContent.Message = "Teleport is unactive. Defeat Dungeon Boss.";
    }

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
        ConsolePanel.instance.InfoLog("Teleport to the next floor has been acitvated!");
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
            UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.INFORMATION);
        }
    }

    public void ObjectInteraction(GameObject interactingObject = null)
    {
        UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.OPEN);
    }

    private void OnMouseEnter()
    {
        UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.NAME);
    }

    private void OnMouseExit()
    {
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
    }

}
