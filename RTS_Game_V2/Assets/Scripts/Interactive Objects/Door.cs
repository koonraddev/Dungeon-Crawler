using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private RoomTeleport roomTeleport;
    [SerializeField] private RoomFader roomFader;
    [SerializeField] private List<Transform> sides;

    DoorSO doorSO;
    private PassiveItem keyItem;
    private bool destroyItemOnUse, keyRequired;
    private Dictionary<string, string> contentToDisplay;
    private int interactionDistance = 3;
    private GameObject interactingObject;

    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }
    public bool KeyRequired { get => keyRequired; }

    public void ObjectInteraction(GameObject interactingObject)
    {
        this.interactingObject = interactingObject;
        if (keyRequired)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", doorSO.NameText }, { "Description", doorSO.Description } });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.OPEN);
        }
        else
        {
            DoInteraction();
        }
    }

    public void DoInteraction()
    {
        if (keyRequired)
        {
            if (InventoryManager.instance.CheckItem(keyItem))
            {
                if (destroyItemOnUse)
                {
                    InventoryManager.instance.RemoveItem(keyItem);
                }

                keyRequired = false;
                roomTeleport.ActiveTeleport(true);
                MapManager.instance.ActivatePortal(this.gameObject);
                Teleport();
            }
            else
            {
                SetContentToDisplay(new Dictionary<string, string> { { "Message", "You need: " + keyItem.Name } });
                UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.INFORMATION);
            }
        }
        else
        {
            Teleport();
        }
        
    }

    private void Teleport()
    {
        if(interactingObject != null)
        {
            if(interactingObject.TryGetComponent(out PlayerMovement playerMovement))
            {
                float dist = 0;
                Transform farPoint = sides[0];
                foreach (var item in sides)
                {
                    float tmpDist = Vector3.Distance(interactingObject.transform.position, item.position);
                    if (tmpDist > dist)
                    {
                        dist = tmpDist;
                        farPoint = item;
                    }
                }

                playerMovement.TeleportTo(farPoint.position);
                roomFader.Teleport();
            }
        }
    }

    private void OnMouseEnter()
    {
        roomTeleport.ActiveParticles(!keyRequired);

        SetContentToDisplay(new Dictionary<string, string> { { "Name", doorSO.NameText } });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);

    }

    private void OnMouseExit()
    { 
        roomTeleport.ActiveParticles(false);
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
    }

    private  void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string> { };

        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }


    public void SetDoor(DoorSO newDoor)
    {
        if (newDoor != null)
        {
            doorSO = newDoor;
            if (doorSO.keyRequired != null)
            {
                keyItem = new(doorSO.keyRequired);
                destroyItemOnUse = !keyItem.IsReusable;
                keyRequired = true;
            }
            else
            {
                keyRequired = false;
            }
            roomTeleport.ActiveTeleport(!keyRequired);
            MapManager.instance.AddPortal(this.gameObject);
        }
    }
}
