using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ContainerObject : MonoBehaviour, IInteractionObject
{
    [SerializeField] private GameObject parentObject;
    private Dictionary<string, string> contentToDisplay;
    private bool displayPopup = true;
    private Container container;
    private int interactionDistance = 3;
    private float existingTime;
    private float existingTimeLeft;
    private bool disappearON = false;
    private bool stopExistingTime;

    public GameObject GameObject => gameObject;
    public int InteractionDistance => interactionDistance;

    public Dictionary<string, string> ContentToDisplay => contentToDisplay;

    public void Start()
    {
        ChangeContainerStatus(false);
    }

    private void Update()
    {
        if (disappearON)
        {
            if (!stopExistingTime)
            {
                existingTimeLeft -= Time.deltaTime;
            }

            if (existingTimeLeft <= 0)
            {
                parentObject.SetActive(false);
            }
        }
    }

    public void SetContainer(Container newContainer, float lootExistingTime = 0)
    {
        container = newContainer;

        if (lootExistingTime > 0)
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

    public void ObjectInteraction(GameObject interactingObject = null)
    {
        SetContentToDisplay(new Dictionary<string, string> { { "Name", container.Name }, { "Description", container.Description } });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.OPEN);
    }

    public void DoInteraction()
    {
        ChangeContainerStatus(true);
    }

    private void ChangeContainerStatus(bool status)
    {
        if (status)
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
        ChangeContainerStatus(false);
    }

    public void OnMouseEnterObject(Color highLightColor)
    {
        Material[] objectMaterials = gameObject.GetComponent<Renderer>().materials;

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
            SetContentToDisplay(new Dictionary<string, string> { { "Name", container.Name } });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
            displayPopup = false;
        }
    }

    public void OnMouseExitObject()
    {
        Material[] objectMaterials = gameObject.GetComponent<Renderer>().materials;

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
        displayPopup = true;
    }


    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string>();

        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }
}
