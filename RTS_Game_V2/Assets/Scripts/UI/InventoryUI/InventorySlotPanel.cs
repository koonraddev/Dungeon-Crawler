using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] public TMP_Text amountHolder;
    [SerializeField] private Image textureHolder;
    [SerializeField] private int slotNumber;
    public InventorySlot invSlot { get; private set; }

    public int SlotNumber { get => slotNumber;}

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InformationPanel infoPanel;

    private GameObject newObj;
    private RectTransform rect;

    private bool getOne;
    private bool merge;
    private Sprite emptySlotSprite;
    private Color emptySlotColor;
    private void Awake()
    {
        invSlot = new(slotNumber);
        Image img = gameObject.GetComponent<Image>();
        emptySlotSprite = img.sprite;
        emptySlotColor = img.color;
    }

    void Start()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.GetInfoPanel();
        infoPanel = infoObject.GetComponent<InformationPanel>();
    }

    public void SetInventorySlotUI(InventoryItem item, int amount, Color slotColor)
    {
        invSlot.Item = item;
        invSlot.Amount = amount;
        invSlot.Empty = false;
        amountHolder.text = (amount == 1) ? amountHolder.text = "" : amount.ToString();
        textureHolder.color = slotColor;
        textureHolder.sprite = item.Sprite;
    }

    public void SetEmptySlot()
    {
        amountHolder.text = "";
        textureHolder.sprite = emptySlotSprite;
        textureHolder.color = emptySlotColor;
        invSlot.Empty = true;
        invSlot.Item = null;
        invSlot.Amount = 0;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            getOne = true;
        }
        else
        {
            getOne = false;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            merge = true;
        }
        else
        {
            merge = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!invSlot.Empty && infoPanel != null)
        {
            GameEvents.instance.InformationPanel(true);

            if (invSlot.Item is IStatisticItem statsItem)
            {
                infoPanel.SetInfoPanel(statsItem.Name, statsItem.Description, statsItem.Sprite, statsItem.Statistics);
            }
            else
            {
                infoPanel.SetInfoPanel(invSlot.Item.Name, invSlot.Item.Description, invSlot.Item.Sprite);
            }

        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!invSlot.Empty && infoPanel != null)
        {
            infoPanel.SetEmpty();
            GameEvents.instance.InformationPanel(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (invSlot.Item is IUsable usableItem)
        {
            if (usableItem.Use())
            {
                InventoryManager.instance.RemoveItem(slotNumber, amount: 1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!invSlot.Empty)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            rect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();
            
            dragImage.sprite = invSlot.Item.Sprite;
            canvGroup.alpha = 0.6f;
            canvGroup.blocksRaycasts = false;
            
            newObj.transform.SetParent(GameObject.Find("UICanvas").transform);
            rect.transform.position = gameObject.transform.position;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if(newObj != null)
        {
            rect.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(newObj);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventorySlotPanel invSlot = eventData.pointerDrag.GetComponent<InventorySlotPanel>();
            if (invSlot != null)
            {

                if (getOne)
                {
                    InventoryManager.instance.MoveOnePiece(invSlot.SlotNumber, this.SlotNumber);
                    return;
                }
                if (merge)
                {
                    InventoryManager.instance.MergeItems(invSlot.SlotNumber, this.SlotNumber);
                    return;
                }
                InventoryManager.instance.SwapItems(invSlot.SlotNumber, this.SlotNumber);
            }
            ContainerSlotPanel containerSlotPanel = eventData.pointerDrag.GetComponent<ContainerSlotPanel>();
            if(containerSlotPanel != null)
            {
                ContainerSlot chestCont = containerSlotPanel.GetContainerSlot();

                if (chestCont.Item is InventoryItem)
                {
                    // object myObject implements 
                    if (InventoryManager.instance.AddItem(chestCont.Item as InventoryItem, SlotNumber,chestCont.Amount))
                    {
                        containerSlotPanel.SetEmptySlot();
                    }
                }
            }
        }
    }
}
