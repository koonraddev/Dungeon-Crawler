using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IInteractionObjects
{
    [SerializeField] ItemSO openingKey;
    private bool opened;
    private bool keyRequired;
    private GameObject actualDoor;

    public void Start()
    {
        actualDoor = gameObject.transform.parent.gameObject;
        if (openingKey != null)
        {
            keyRequired = true;
        }
        else
        {
            keyRequired = false;
        }
        ChangeDoorStatus(false);
    }

    public void OnMouseEnterObject(Color highLightColor)
    {
        if (!opened)
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
    }

    public void ObjectInteraction()
    {
        if (opened)
        {
            ChangeDoorStatus(false);
        }
        else
        {
            if (keyRequired)
            {
                List<ItemSO> invToCheck = Inventory.Instance.GetInventory();
                foreach (ItemSO item in invToCheck)
                {
                    if (item == openingKey)
                    {
                        ChangeDoorStatus(true);
                        opened = true;
                        keyRequired = false;
                        break;
                    }
                }
            }
            else
            {
                ChangeDoorStatus(true);
            }
            
        }
    }


    private void ChangeDoorStatus(bool doorStatus)
    {
        if (doorStatus)
        {
            actualDoor.transform.DOLocalRotate(new Vector3(0, 0, 90f), 2f).SetEase(Ease.Linear);
            opened = true;
        }
        else
        {
            actualDoor.transform.DOLocalRotate(new Vector3(0, 0, 0f), 2f).SetEase(Ease.Linear);
            opened = false;
        }
    }
}
