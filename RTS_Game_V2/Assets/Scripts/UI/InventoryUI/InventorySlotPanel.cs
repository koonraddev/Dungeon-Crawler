using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    private Canvas canvas;
    [SerializeField] private TMP_Text amountHolder;
    [SerializeField] private Image textureHolder;
    [SerializeField] private int slotNumber;
    public InventorySlot InvSlot { get; private set; }

    public int SlotNumber { get => slotNumber;}

    private GameObject newObj;
    private InformationPanel infoPanel;
    private RectTransform newObjRect;

    private bool getOne, merge;
    private Sprite emptySlotSprite;
    private Color emptySlotColor;
    private void Awake()
    {
        InvSlot = new(slotNumber);
        emptySlotSprite = textureHolder.sprite;
        emptySlotColor = textureHolder.color;
        GameObject canvasObject = transform.root.gameObject;
        canvas = canvasObject.GetComponent<Canvas>();
        UICanvasController uiCtrl = canvasObject.GetComponent<UICanvasController>();
        GameObject infoPanelObject = uiCtrl.InfoPanel;
        infoPanel = infoPanelObject.GetComponent<InformationPanel>();
    }

    public void SetInventorySlotUI(InventoryItem item, int amount, Color slotColor)
    {
        InvSlot.Item = item;
        InvSlot.Amount = amount;
        InvSlot.Empty = false;
        amountHolder.text = (amount == 1) ? amountHolder.text = "" : amount.ToString();
        textureHolder.color = slotColor;
        textureHolder.sprite = item.Sprite;
    }

    public void SetEmptySlot()
    {
        amountHolder.text = "";
        textureHolder.sprite = emptySlotSprite;
        textureHolder.color = emptySlotColor;
        InvSlot.Empty = true;
        InvSlot.Item = null;
        InvSlot.Amount = 0;
    }

    private void Update()
    {
        getOne = Input.GetKey(KeyCode.LeftShift);
        merge = Input.GetKey(KeyCode.LeftControl);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!InvSlot.Empty && infoPanel != null)
        {
            GameEvents.instance.InformationPanel(true);
            infoPanel.SetInfoPanel(InvSlot.Item);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!InvSlot.Empty && infoPanel != null)
        {
            infoPanel.SetEmpty();
            GameEvents.instance.InformationPanel(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InvSlot.Item is IUsable usableItem)
        {
            if (usableItem.Use())
            {
                InventoryManager.instance.RemoveItem(slotNumber, amount: 1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!InvSlot.Empty)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            newObjRect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();
            
            dragImage.sprite = InvSlot.Item.Sprite;
            canvGroup.alpha = 0.6f;
            canvGroup.blocksRaycasts = false;
            
            newObj.transform.SetParent(GameObject.Find("UICanvas").transform);
            newObjRect.transform.position = gameObject.transform.position;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if(newObj != null)
        {
            newObjRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
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
            if (eventData.pointerDrag.TryGetComponent(out InventorySlotPanel inventorySlotPanel))
            {

                if (getOne)
                {
                    InventoryManager.instance.MoveOnePiece(inventorySlotPanel.SlotNumber, this.SlotNumber);
                    return;
                }
                if (merge)
                {
                    InventoryManager.instance.MergeItems(inventorySlotPanel.SlotNumber, this.SlotNumber);
                    return;
                }
                InventoryManager.instance.SwapItems(inventorySlotPanel.SlotNumber, this.SlotNumber);
            }

            if (eventData.pointerDrag.TryGetComponent(out ContainerSlotPanel containerSlotPanel))
            { 
                ContainerSlot chestCont = containerSlotPanel.ContainerSlot;

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
