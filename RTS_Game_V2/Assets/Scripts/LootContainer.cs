using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class LootContainer : MonoBehaviour, IInteractionObject
{
    [SerializeField] private GameObject ParentObject;
    private Dictionary<string, string> contentToDisplay;
    private bool displayPopup = true;

    private ContainerObject container;
    private int interactionDistance = 3;
    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }

    float existingTime;
    float existingTimeLeft;
    bool disappearON = false;
    bool stopExistingTime;
    public void Start()
    {
        ChangeChestStatus(false);
    }

    private void Update()
    {
        if (disappearON)
        {
            if (!stopExistingTime)
            {
                existingTimeLeft -= Time.deltaTime;
            }

            if(existingTimeLeft <= 0)
            {
                ParentObject.SetActive(false);
            }
        }
    }
    public void SetLoot(List<ContainerSlot> slots, string lootName, string lootDescription, float lootExistingTime = 0)
    {
        container = new(slots, lootName, lootDescription);

        if(lootExistingTime > 0)
        {
            disappearON = true;
            existingTimeLeft = lootExistingTime;
            existingTime = lootExistingTime;
        }
        else
        {
            disappearON = false;
        }
    }

    public void ObjectInteraction()
    {
        SetContentToDisplay(new Dictionary<string, string> { { "Name", container.contName }, { "Description", container.contDesc } });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.OPEN);
    }

    public void DoInteraction()
    {
        ChangeChestStatus(true);
    }

    private void ChangeChestStatus(bool chestStatus)
    {
        if (chestStatus)
        {
            gameObject.transform.DOLocalRotate(new Vector3(-140, 0, 0), 2f).SetEase(Ease.OutBounce);
            ContainerInfoPanel.instance.SetAndActiveContainerPanel(container);
            GameEvents.instance.InventoryPanel(true);
            GameEvents.instance.OnCancelGameObjectAction += OnCancelGameObject;
            stopExistingTime = true;
            existingTimeLeft = existingTime;                  
        }
        else
        {
            stopExistingTime = false;
            gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 2f).SetEase(Ease.Linear);
            GameEvents.instance.OnCancelGameObjectAction -= OnCancelGameObject;
        }
    }

    public void OnCancelGameObject()
    {
        ChangeChestStatus(false);
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


    public Dictionary<string, string> GetContentToDisplay()
    {
        return contentToDisplay;
    }
    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string> { };
        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }
}
