using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform rectTransform;

    [SerializeField] public TMP_Text nameHolder;
    [SerializeField] public TMP_Text amountHolder;
    [SerializeField] private Image textureHolder;
    private IInventoryItem Item { get; set; }
    [HideInInspector] public int SlotNumber { get; set; }

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InventoryInfoPanel infoPanel;

    private GameObject newObj;
    private RectTransform rect;

    private bool getOne;
    private bool merge;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.InventoryInfoPanel;
        infoPanel = infoObject.GetComponent<InventoryInfoPanel>();
    }

    public void SetInventorySlotUI(IInventoryItem item, int amount, Color slotColor)
    {
        Item = item;
        nameHolder.text = Item.NameText;
        amountHolder.text = (amount == 1) ? amountHolder.text = "" : amount.ToString();
        textureHolder.color = slotColor;
        textureHolder.sprite = item.InventoryTexture;
    }

    public void SetEmptySlot(Sprite slotSprite, Color slotColor)
    {
        nameHolder.text = "";
        amountHolder.text = "";
        textureHolder.sprite = slotSprite;
        textureHolder.color = slotColor;
        Item = null;
    }
    public void OnPointerClick(PointerEventData eventData)
    {


        //if (Item != null)
        //{
        //    Inventory.Instance.RemoveItem(SlotNumber);
        //    infoPanel.SetInfoPanel("", "");
        //}
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
        if (Item != null && infoPanel != null)
        {
            infoPanel.SetInfoPanel(Item.NameText, Item.Description);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item != null && infoPanel != null)
        {
            infoPanel.SetInfoPanel("", "");
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Item != null)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            rect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();
            
            dragImage.sprite = Item.InventoryTexture;
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
            InventorySlotUI invSlotDrag = eventData.pointerDrag.GetComponent<InventorySlotUI>();
            if (invSlotDrag != null)
            {
                if (getOne)
                {
                    Inventory.Instance.GetOneItem(invSlotDrag.SlotNumber, this.SlotNumber);
                    return;
                }
                if (merge)
                {
                    Inventory.Instance.MergeItems(invSlotDrag.SlotNumber, this.SlotNumber);
                    return;
                }
                Inventory.Instance.SwapItems(invSlotDrag.SlotNumber, this.SlotNumber);
            }
        }
    }
}
