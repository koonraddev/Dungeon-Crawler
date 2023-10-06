using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ContainerSlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image textureHolder;
    [SerializeField] private TMP_Text amountHolder;

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InformationPanel infoPanel;

    private GameObject newObj;
    private RectTransform rect;


    private ContainerSlot contSlot;
    public ContainerSlot ContSlot { get => contSlot; set => contSlot = value; }
    private ContainerObject container;
    private Sprite panelSlotSprite;
    private Color panelSlotColor;
    public int slotIndex { get; private set; }
    private void Awake()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.GetInfoPanel();
        infoPanel = infoObject.GetComponent<InformationPanel>();
        textureHolder = GetComponent<Image>();
    }
    public void SetContainerSlotUI(ContainerObject container,ContainerSlot containerSlot, Color slotColor)
    {
        this.container = container;
        contSlot = containerSlot;
        Debug.Log("Set");
        if (containerSlot.Empty)
        {
            textureHolder.color = panelSlotColor;
            textureHolder.sprite = panelSlotSprite;
            amountHolder.text = "";
        }
        else
        {
            contSlot = containerSlot;
            textureHolder.color = slotColor;
            textureHolder.sprite = contSlot.Item.Sprite;
            if (containerSlot.Amount > 1)
            {
                amountHolder.text = containerSlot.Amount.ToString();
            }
            else
            {
                amountHolder.text = "";
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (contSlot.Item != null && infoPanel != null)
        {
            GameEvents.instance.InformationPanel(true);
            infoPanel.SetInfoPanel(contSlot.Item.Name, contSlot.Item.Description);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (contSlot.Item != null && infoPanel != null)
        {
            infoPanel.SetEmpty();
            GameEvents.instance.InformationPanel(false);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(contSlot.Item is IUsable usableItem)
        {
            usableItem.Use();
            if(contSlot.Amount > 1)
            {
                contSlot.Amount -= 1;
                GameEvents.instance.ContainerUpdate();
            }
            else
            {
                SetEmptySlot();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (contSlot.Item != null)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            rect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();

            dragImage.sprite = contSlot.Item.Sprite;
            canvGroup.alpha = 0.6f;
            canvGroup.blocksRaycasts = false;

            newObj.transform.SetParent(GameObject.Find("UICanvas").transform);
            rect.transform.position = gameObject.transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (newObj != null)
        {
            rect.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(newObj);
    }

    public ContainerSlot GetContainerSlot()
    {
        return contSlot;
    }

    public void SetEssentials(Sprite panelSlotSprite, Color panelSlotColor, int slotIndex)
    {
        this.slotIndex = slotIndex;
        this.panelSlotSprite = panelSlotSprite;
        this.panelSlotColor = panelSlotColor;

        textureHolder.sprite = panelSlotSprite;
        textureHolder.color = panelSlotColor;
        contSlot = new(slotIndex);
    }

    public void SetEmptySlot()
    {
        container.RemoveTreasure(slotIndex);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventorySlotPanel invSlotPanel = eventData.pointerDrag.GetComponent<InventorySlotPanel>();
            if (invSlotPanel != null)
            {
                InventorySlot contSlot = invSlotPanel.invSlot;

                if (container.AddTreasure(slotIndex, contSlot.Item, contSlot.Amount))
                {
                    InventoryManager.instance.RemoveItem(invSlotPanel.SlotNumber,contSlot.Amount);
                }
               
            }

            ContainerSlotPanel containerSlotPanel = eventData.pointerDrag.GetComponent<ContainerSlotPanel>();
            if (containerSlotPanel != null)
            {
                container.SwapItems(containerSlotPanel.slotIndex, this.slotIndex);
            }

            EquipmentSlotPanel eqSlotPanel = eventData.pointerDrag.GetComponent<EquipmentSlotPanel>();
            if(eqSlotPanel != null)
            {
                EquipmentItem eqItem = eqSlotPanel.eqSlot.Item;
                if (container.AddTreasure(slotIndex, eqItem))
                {
                    EquipmentManager.instance.RemoveItem(eqItem);
                }
            }
        }
    }
}
