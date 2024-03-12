using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private RoomTeleport roomTeleport;
    [SerializeField] private PortalTrigger doorTrigger;
    [SerializeField] private Transform portalPoint;
    private PortalBehaviour portalLogic;
    ObjectContent objectContent;
    PortalSO portalSO;
    private PassiveItem keyItem;
    private bool destroyItemOnUse, keyRequired;
    private int interactionDistance = 3;

    public int InteractionDistance { get => interactionDistance; }
    public bool KeyRequired { get => keyRequired; }
    public PortalSO PortalSO { get => portalSO; }
    public ObjectContent ContentDoDisplay => objectContent;
    public Transform PortalPoint { get => portalPoint; }

    public void ObjectInteraction(GameObject interactingObject)
    {
        if(!interactingObject.TryGetComponent(out PlayerMovement playerMov))
        {
            return;
        }

        portalLogic.PlayerMovement = playerMov;

        if (keyRequired)
        {
            UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.OPEN);
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

                UnlockPortal();
                portalLogic.Teleport();
            }
            else
            {
                UIMessageObjectPool.instance.DisplayMessage(objectContent,PopupType.INFORMATION);
            }
        }
        else
        {
            portalLogic.Teleport();
        }
        
    }

    private void UnlockPortal()
    {
        keyRequired = false;
        roomTeleport.ActiveTeleport(true);
        MapManager.instance.ActivatePortal(this.gameObject);
    }


    private void OnMouseEnter()
    {
        roomTeleport.ActiveParticles(!keyRequired);

        UIMessageObjectPool.instance.DisplayMessage(objectContent,PopupType.NAME);

    }

    private void OnMouseExit()
    { 
        roomTeleport.ActiveParticles(false);
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
    }


    public void SetDoor(PortalSO newDoor, SpawnType teleportSide, bool forceUnlock = false, bool forceShow = false)
    {
        portalLogic = new(gameObject.transform.parent.gameObject);
        doorTrigger.teleportSide = teleportSide;
        doorTrigger.teleportLogic = portalLogic;
        if (newDoor != null)
        {
            objectContent = new(gameObject);
            portalSO = newDoor;
            if (portalSO.keyRequired != null)
            {
                keyItem = new(portalSO.keyRequired);
                destroyItemOnUse = !keyItem.MultipleUse;
                keyRequired = true;
                objectContent.Message = "You need: " + keyItem.Name;
            }
            else
            {
                keyRequired = false;
            }
            roomTeleport.ActiveTeleport(!keyRequired);
            objectContent.Nametext = portalSO.NameText;
            objectContent.Description = portalSO.Description;
            objectContent.YesButtonDelegate = DoInteraction;

            MapManager.instance.AddPortal(gameObject);
            if (forceUnlock)
            {
                UnlockPortal();
            }

            if (forceShow)
            {
                MapManager.instance.ShowPortal(gameObject);
            }
        }
    }
}
