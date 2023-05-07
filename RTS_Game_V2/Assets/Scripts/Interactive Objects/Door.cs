using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour, IInteractionObjects
{
    [SerializeField] DoorSO doorSO;
    private IInventoryItem keyItem;
    private bool opened;
    private bool destroyItemOnUse;
    private bool keyRequired;
    private bool displayInfo;
    private GameObject actualDoor;
    private Dictionary<string, string> contentToDisplay;
    public void Start()
    {
        actualDoor = gameObject.transform.parent.gameObject;
        displayInfo = true;
        CheckKey();
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
        //Debug.Log("Interakcja");
        if (opened)
        {
            //Debug.Log("Sa otwarte");
            ChangeDoorStatus(false);
        }
        else
        {
            //Debug.Log("Sa zamkniete");
            if (keyRequired)
            {
                //Debug.Log("wymagaja klucza");

                if(Inventory.Instance.CheckItem(keyItem))
                {
                    if (destroyItemOnUse)
                    {
                        Inventory.Instance.RemoveItem(keyItem);
                    }
                    ChangeDoorStatus(true);
                    keyRequired = false;
                }
                else
                {
                    SetContentToDisplay(new Dictionary<string, string> { { "Message", "You need: " + keyItem.NameText } });
                    UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.INFORMATION);
                }
            }
            else
            {
                //Debug.Log("nie wymagaja klucza");
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

    private void CheckKey()
    {
        if (doorSO.keyRequired != null)
        {
            keyItem = doorSO.keyRequired;
            destroyItemOnUse = !keyItem.IsReusable;
            keyRequired = true;
            LockDoor();
        }
        else
        {
            keyRequired = false;
        }
    }

    public void SetDoor(DoorSO newDoor)
    {
        if (newDoor != null)
        {
            doorSO = newDoor;
            CheckKey();
        }
    }

    public void LockDoor()
    {
        if (keyItem != null)
        {
            keyRequired = true;
            opened = false;
        }
    }

    private void ChangeDoorStatus(bool doorOpened)
    {
        if (doorOpened)
        {
            //Debug.Log("otwieram drzwi");
            actualDoor.transform.DOLocalRotate(new Vector3(0, 0, 90f), 2f).SetEase(Ease.Linear);
            opened = true;
        }
        else
        {
            //Debug.Log("zamykam drzwi");
            actualDoor.transform.DOLocalRotate(new Vector3(0, 0, 0f), 2f).SetEase(Ease.Linear);
            opened = false;
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
