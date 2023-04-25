using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class Key : MonoBehaviour, IInteractionObjects
{
    [SerializeField] private KeySO keySo;
    private bool displayInfo = true;
    private bool displayMessage = true;
    private Dictionary<string, string> contentToDisplay;
    public void ObjectInteraction()
    {
        if (displayMessage)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", keySo.NameText }, { "Description", keySo.Description } });
            UIObjectPool.instance.DisplayMessage(this, MessageType.TAKE);
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
            SetContentToDisplay(new Dictionary<string, string> { { "Name", keySo.NameText } });
            UIObjectPool.instance.DisplayMessage(this, MessageType.POPUP);
            displayInfo = false;
        }
    }

    public void DoInteraction()
    {
        Inventory.Instance.AddItem(keySo);
        Destroy(gameObject);
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
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string> { };
        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }

    public Dictionary<string,string> GetContentToDisplay()
    {
        return contentToDisplay;
    }
    

    public void SetKey(KeySO newKey)
    {
        if (newKey != null)
        {
            keySo = newKey;
        }
    }
}
