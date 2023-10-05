using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Chest : MonoBehaviour, IInteractionObjects
{
    public GameObject panel;
    private Dictionary<string, string> contentToDisplay;
    private bool displayPopup = true;

    public ContainerObject container;
    public void Start()
    {
        ChangeChestStatus(false);
        panel = ContainerInfoPanel.instance.gameObject;
    }

    public void ObjectInteraction()
    {
        SetContentToDisplay(new Dictionary<string, string> { { "Name", container.contName }, { "Description", container.contDesc} });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.OPEN);
    }

    public void DoInteraction()
    {
        ChangeChestStatus(true);
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
        if (displayPopup)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", container.contName } });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
            displayPopup = false;
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
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
        displayPopup = false;
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
            container = new(newChest.GetContainerSlots(), newChest.GetNameText(), newChest.GetDescription());
        }
    }

    private void ChangeChestStatus(bool chestStatus)
    {
        if (chestStatus)
        {
            gameObject.transform.DOLocalRotate(new Vector3(-140, 0, 0), 2f).SetEase(Ease.OutBounce);
            ContainerInfoPanel.instance.SetAndActiveChestPanel(container);
            GameEvents.instance.InventoryPanel(true);
            GameEvents.instance.OnCancelGameObjectAction += OnCancelGameObject;
        }
        else
        {
            gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 2f).SetEase(Ease.Linear);
            GameEvents.instance.OnCancelGameObjectAction -= OnCancelGameObject;
        }
    }

    public void OnCancelGameObject()
    {
        ChangeChestStatus(false);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
