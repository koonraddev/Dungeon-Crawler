using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour, IInteractionObject
{
    [SerializeField] DoorSO doorSO;
    private PassiveItem keyItem;
    private bool destroyItemOnUse;
    private bool keyRequired;
    private bool displayInfo;
    [SerializeField] GameObject actualDoor;
    private Dictionary<string, string> contentToDisplay;
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }

    public void Start()
    {
        displayInfo = true;
        ChangeDoorStatus(false);
    }

    private int interactionDistance = 3;
    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }


    //NOWE
    public List<Transform> sides;
    private GameObject interactingObject;
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

        //Sa zamkniete
        if (keyRequired)
        {
            //wymagaja klucza
        
            if (InventoryManager.instance.CheckItem(keyItem))
            {
                if (destroyItemOnUse)
                {
                    InventoryManager.instance.RemoveItem(keyItem);
                }
                keyRequired = false;
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
            //nie wymagaja klucza
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
            }
        }
    }

    public void OnMouseEnterObject(Color highLightColor)
    {
        Material[] objectMaterials;
        objectMaterials = actualDoor.GetComponent<Renderer>().materials;
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
        if (!keyRequired)
        {
            ChangeDoorStatus(true);
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
        Material[] objectMaterials;
        objectMaterials = actualDoor.GetComponent<Renderer>().materials;
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

        ChangeDoorStatus(false);
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


    private bool CheckKeyRequired()
    {
        if (doorSO.keyRequired != null)
        {
            keyItem = new(doorSO.keyRequired);
            destroyItemOnUse = !keyItem.IsReusable;
            keyRequired = true;
            return true;
        }
        else
        {
            keyRequired = false;
            return false;
        }
    }

    public void SetDoor(DoorSO newDoor)
    {
        if (newDoor != null)
        {
            doorSO = newDoor;
        }
    }



    private void ChangeDoorStatus(bool doorOpened)
    {
        if (doorOpened)
        {
            actualDoor.transform.DOLocalRotate(new Vector3(90f,15f, 0), 0.25f).SetEase(Ease.Linear);
        }
        else
        {
            actualDoor.transform.DOLocalRotate(new Vector3(90f, 0, 0), 0.25f).SetEase(Ease.Linear);
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
