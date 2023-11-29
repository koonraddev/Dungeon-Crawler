using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour, IInteractionObject
{
    DoorSO doorSO;
    private PassiveItem keyItem;
    private bool destroyItemOnUse;
    private bool keyRequired;
    private bool displayInfo;
    //[SerializeField] GameObject actualDoor;
    private Dictionary<string, string> contentToDisplay;
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }

    [SerializeField] private RoomTeleport roomTeleport;


    public void Start()
    {
        displayInfo = true;
    }

    private int interactionDistance = 3;
    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }
    //NOWE
    public List<Transform> sides;
    private GameObject interactingObject;

    [SerializeField] private RoomFader roomFader;
    public void ObjectInteraction(GameObject interactingObject)
    {
        this.interactingObject = interactingObject;
        //Debug.Log("Drzwi");
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
        Debug.LogWarning("TELEPORT");
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

    public void OnMouseEnterObject(Color highLightColor)
    {
        if (!keyRequired)
        {
            roomTeleport.ActiveParticles(!keyRequired);
        }

        if (displayInfo)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", doorSO.NameText } });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
            displayInfo = false;
        }
    }

    public void OnMouseExitObject()
    {
        displayInfo = true;

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
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
