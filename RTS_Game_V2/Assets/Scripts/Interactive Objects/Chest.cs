using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class Chest : MonoBehaviour, IInteractionObjects
{
    [SerializeField] private ChestSO chestSO;
    [SerializeField] private Key key;
    private KeySO treasure;
    private bool displayPopup = true;
    private bool displayMessage = true;
    private bool opened;
    private Dictionary<string, string> contentToDisplay;

    public void Start()
    {
        ChangeChestStatus(false);
        CheckKey();
    }

    public void ObjectInteraction()
    {
        if (!opened && displayMessage)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", chestSO.NameText }, { "Description", chestSO.Description } });
            UIObjectPool.instance.DisplayMessage(this, MessageType.OPEN);
        }
    }

    public void DoInteraction()
    {
        ChangeChestStatus(true);
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
            if (displayPopup)
            {
                SetContentToDisplay(new Dictionary<string, string> { { "Name", chestSO.NameText } });
                UIObjectPool.instance.DisplayMessage(this, MessageType.POPUP);
                displayPopup = false;
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
        displayPopup = true;
    }



    private void OnDrawGizmos()
    {
        float chestCollRadius = gameObject.GetComponentInChildren<MeshCollider>().bounds.max.x;
        Gizmos.DrawWireSphere(gameObject.transform.position, chestCollRadius/2);
    }
    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
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
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetChest(ChestSO newChest)
    {
        if (newChest != null)
        {
            chestSO = newChest;
        }
    }

    private void ChangeChestStatus(bool chestStatus)
    {
        if (chestStatus)
        {
            gameObject.transform.DOLocalRotate(new Vector3(-140, 0, 0), 2f).SetEase(Ease.OutBounce);
            opened = true;
        }
        else
        {
            gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 2f).SetEase(Ease.Linear);
            opened = false;
        }
    }

    private void CheckKey()
    {
        if (chestSO.Treasure != null)
        {
            treasure = chestSO.Treasure;
            displayPopup = true;
            key.SetKey(treasure);
            key.gameObject.SetActive(true);
        }
        else
        {
            key.gameObject.SetActive(false);
        }
    }
}
