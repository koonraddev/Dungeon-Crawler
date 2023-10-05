using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour, IInteractionObjects
{
    [SerializeField] DoorSO doorSO;
    private PassiveItem keyItem;
    private bool opened;
    private bool destroyItemOnUse;
    private bool keyRequired;
    private bool displayInfo;
    private GameObject actualDoor;
    private Dictionary<string, string> contentToDisplay;
    private OffMeshLink meshLink;

    private void Awake()
    {
        meshLink = GetComponent<OffMeshLink>();
        meshLink.activated = false;
    }
    public void Start()
    {
        actualDoor = gameObject.transform.parent.gameObject;
        displayInfo = true;
        ChangeDoorStatus(false);
    }

    public void ObjectInteraction()
    {
        //Debug.Log("Drzwi");
        if (keyRequired && !opened)
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
        //Interakcja
        if (opened)
        {
            //Sa otwarte
            //ChangeDoorStatus(false);
        }
        else
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
                    ChangeDoorStatus(true);
                    keyRequired = false;
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
                ChangeDoorStatus(true);
            }
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
            SetContentToDisplay(new Dictionary<string, string> { { "Name", doorSO.NameText } });
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

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private  void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string> { };

        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }

    public Dictionary<string, string> GetContentToDisplay()
    {
        return contentToDisplay;
    }

    private bool CheckKeyRequired()
    {
        if (doorSO.keyRequired != null)
        {
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
            if (CheckKeyRequired())
            {
                keyItem = new(doorSO.keyRequired);
                LockDoor();
            }
        }
    }

    public void LockDoor()
    {
        if (keyItem != null)
        {
            opened = false;
        }
    }

    private void ChangeDoorStatus(bool doorOpened)
    {
        if (doorOpened)
        {
            actualDoor.transform.DOLocalRotate(new Vector3(0, 0, 90f), 2f).SetEase(Ease.Linear);
            opened = true;
            meshLink.activated = true;
        }
        else
        {
            actualDoor.transform.DOLocalRotate(new Vector3(0, 0, 0f), 2f).SetEase(Ease.Linear);
            opened = false;
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
