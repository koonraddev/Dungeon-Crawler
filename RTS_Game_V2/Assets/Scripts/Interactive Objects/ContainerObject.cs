using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ContainerObject : MonoBehaviour, IInteractiveObject
{
    [SerializeField, ColorUsage(true,true)] private Color highLightObjectColor;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private Renderer[] renderers;
    private Container container;
    private int interactionDistance = 3;
    private float existingTime, existingTimeLeft;
    private bool disappearON = false, stopExistingTime;
   
    public GameObject GameObject => gameObject;
    public int InteractionDistance => interactionDistance;
    ObjectContent objectContent;
    public ObjectContent ContentDoDisplay => objectContent;


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

        objectContent = new(gameObject);
        objectContent.Nametext = container.Name;
        objectContent.Description = container.Description;
        objectContent.YesButtonDelegate = DoInteraction;
    }

    public void ObjectInteraction(GameObject interactingObject = null)
    {
        DoInteraction();
    }

    public void DoInteraction()
    {
        ChangeContainerStatus(true);
    }

    private void ChangeContainerStatus(bool status)
    {
        if (status)
        {
            Debug.Log("OPEN");
            gameObject.transform.DOLocalRotate(new Vector3(-140, 0, 0), 2f).SetEase(Ease.OutBounce);
            ContainerInfoPanel.instance.SetAndActiveContainerPanel(container);
            GameEvents.instance.InventoryPanel(true);
            GameEvents.instance.OnCancelGameObjectAction += OnCancelGameObject;
            stopExistingTime = true;
            existingTimeLeft = existingTime;
        }
        else
        {
            Debug.Log("CLOSE");
            stopExistingTime = false;
            gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 2f).SetEase(Ease.Linear);
            GameEvents.instance.OnCancelGameObjectAction -= OnCancelGameObject;
        }
    }

    public void OnCancelGameObject()
    {
        GameEvents.instance.InventoryPanel(false);
        ChangeContainerStatus(false);
    }

    private void OnMouseEnter()
    {
        foreach (var item in renderers)
        {
            item.material.DOColor(highLightObjectColor, "_BaseColor", 0.5f).SetAutoKill(true).Play();
        }

        UIMessageObjectPool.instance.DisplayMessage(objectContent, PopupType.NAME);
    }

    private void OnMouseExit()
    {
        foreach (var item in renderers)
        {
            item.material.DOColor(Color.white, "_BaseColor", 0.5f).SetAutoKill(true).Play();
        }

        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
    }

}
