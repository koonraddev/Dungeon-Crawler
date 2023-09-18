using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] public TMP_Text amountHolder;
    [SerializeField] private Image textureHolder;
    public IInventoryItem Item { get; private set; }
    [HideInInspector] public int SlotNumber { get; set; }

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InformationPanel infoPanel;

    private GameObject newObj;
    private RectTransform rect;

    private bool getOne;
    private bool merge;

    void Start()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.GetInfoPanel();
        infoPanel = infoObject.GetComponent<InformationPanel>();
    }

    public void SetInventorySlotUI(IInventoryItem item, int amount, Color slotColor)
    {
        Item = item;
        amountHolder.text = (amount == 1) ? amountHolder.text = "" : amount.ToString();
        textureHolder.color = slotColor;
        textureHolder.sprite = item.InventoryThumbnail;
    }

    public void SetEmptySlot(Sprite slotSprite, Color slotColor)
    {
        amountHolder.text = "";
        textureHolder.sprite = slotSprite;
        textureHolder.color = slotColor;
        Item = null;
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
            infoPanel.SetEmpty();
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
            
            dragImage.sprite = Item.InventoryThumbnail;
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
                    Inventory.Instance.GetOneItem(invSlot.SlotNumber, this.SlotNumber);
                    return;
                }
                if (merge)
                {
                    Inventory.Instance.MergeItems(invSlot.SlotNumber, this.SlotNumber);
                    return;
                }
                Inventory.Instance.SwapItems(invSlot.SlotNumber, this.SlotNumber);
            }
            ChestSlotPanel chestSlot = eventData.pointerDrag.GetComponent<ChestSlotPanel>();
            if(chestSlot != null)
            {
                TreasureSO treasure = chestSlot.GetTreasure();

                if (treasure is IInventoryItem)
                {
                    // object myObject implements 
                    if (Inventory.Instance.AddItem(treasure as IInventoryItem, SlotNumber))
                    {
                        chestSlot.SetEmptySlot();
                    }
                }
            }
        }
    }
}
