using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ContainerSlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image textureHolder;
    [SerializeField] private TMP_Text amountHolder;
    private Sprite emptySlotSprite;
    private Color emptySlotColor;
    private InformationPanel infoPanel;
    private GameObject newObj;
    private RectTransform newObjRect;
    private Container container;
    private ContainerSlot contSlot;
    public ContainerSlot ContainerSlot { get => contSlot; }


    private int slotIndex;
    public int SlotIndex { get { return slotIndex; } private set { slotIndex = value; } }
    private void Awake()
    {
        GameObject canvas = transform.root.gameObject;
        UICanvasController uiCtrl = canvas.GetComponent<UICanvasController>();
        GameObject infoObject = uiCtrl.InfoPanel;
        infoPanel = infoObject.GetComponent<InformationPanel>();

        emptySlotSprite = textureHolder.sprite;
        emptySlotColor = textureHolder.color;
    }
    public void SetContainerSlotUI(Container container,ContainerSlot containerSlot, Color slotColor)
    {
        this.container = container;
        contSlot = containerSlot;
        if (containerSlot.Empty)
        {
            textureHolder.color = emptySlotColor;
            textureHolder.sprite = emptySlotSprite;
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

    public void SetEssentials(int slotIndex)
    {
        this.SlotIndex = slotIndex;
        textureHolder.sprite = emptySlotSprite;
        textureHolder.color = emptySlotColor;
        contSlot = new(slotIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (contSlot.Item != null && infoPanel != null)
        {
            GameEvents.instance.InformationPanel(true);
            infoPanel.SetInfoPanel(contSlot.Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (contSlot.Item != null && infoPanel != null)
        {
            GameEvents.instance.InformationPanel(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(contSlot.Item is IUsable usableItem)
        {
            if (usableItem.Use())
            {
                if (contSlot.Amount > 1)
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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!contSlot.Empty)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            newObjRect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();

            dragImage.sprite = contSlot.Item.Sprite;
            canvGroup.alpha = 0.6f;
            canvGroup.blocksRaycasts = false;

            newObj.transform.SetParent(GameObject.Find("UICanvas").transform);
            newObjRect.transform.position = gameObject.transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (newObj != null)
        {
            newObjRect.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(newObj);
    }

    public void SetEmptySlot()
    {
        container.RemoveTreasure(SlotIndex);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventorySlotPanel invSlotPanel = eventData.pointerDrag.GetComponent<InventorySlotPanel>();
            if (invSlotPanel != null)
            {
                InventorySlot contSlot = invSlotPanel.InvSlot;

                if (container.AddTreasure(SlotIndex, contSlot.Item, contSlot.Amount))
                {
                    InventoryManager.instance.RemoveItem(invSlotPanel.SlotNumber,contSlot.Amount);
                }
               
            }

            ContainerSlotPanel containerSlotPanel = eventData.pointerDrag.GetComponent<ContainerSlotPanel>();
            if (containerSlotPanel != null)
            {
                container.SwapItems(containerSlotPanel.SlotIndex, this.SlotIndex);
            }

            EquipmentSlotPanel eqSlotPanel = eventData.pointerDrag.GetComponent<EquipmentSlotPanel>();
            if(eqSlotPanel != null)
            {
                EquipmentItem eqItem = eqSlotPanel.EqSlot.Item;
                if (container.AddTreasure(SlotIndex, eqItem))
                {
                    EquipmentManager.instance.RemoveItem(eqItem);
                }
            }
        }
    }
}
