using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestSlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] public TMP_Text nameHolder;
    [SerializeField] private Image textureHolder;

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InventoryInfoPanel infoPanel;

    private GameObject newObj;
    private RectTransform rect;

    private TreasureSO treasureSO;
    private ChestSO chestSO;
    private Sprite panelSlotSprite;
    private Color panelSlotColor;
    public int slotIndex { get; private set; }
    private void Awake()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.GetInfoPanel();
        infoPanel = infoObject.GetComponent<InventoryInfoPanel>();
        nameHolder = GetComponentInChildren<TMP_Text>();
        textureHolder = GetComponent<Image>();
    }
    void Start()
    {


    }

    public void SetChestSlotUI(ChestSO chestSO,TreasureSO treasureSO, Color slotColor, Sprite panelSlotSprite = null)
    {
        this.chestSO = chestSO;
        this.treasureSO = treasureSO;
        nameHolder.text = treasureSO.GetName();
        textureHolder.color = slotColor;
        if(panelSlotSprite == null)
        {
            textureHolder.sprite = treasureSO.GetThumbnail();
        }
        else
        {
            textureHolder.sprite = panelSlotSprite;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (treasureSO != null && infoPanel != null)
        {
            infoPanel.SetInfoPanel(treasureSO.GetName(), treasureSO.GetDescription());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (treasureSO != null && infoPanel != null)
        {
            infoPanel.SetInfoPanel("", "");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (treasureSO != null)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            rect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();

            dragImage.sprite = treasureSO.GetThumbnail();
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

    public TreasureSO GetTreasure()
    {
        return treasureSO;
    }

    public void SetEssentials(Sprite panelSlotSprite, Color panelSlotColor, int slotIndex)
    {
        this.slotIndex = slotIndex;
        this.panelSlotSprite = panelSlotSprite;
        this.panelSlotColor = panelSlotColor;

        nameHolder.text = "";
        textureHolder.sprite = panelSlotSprite;
        textureHolder.color = panelSlotColor;
        treasureSO = null;
    }

    public void SetEmptySlot()
    {
        chestSO.RemoveTreasure(treasureSO);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventorySlotPanel invSlot = eventData.pointerDrag.GetComponent<InventorySlotPanel>();
            if (invSlot != null)
            {
                IInventoryItem invItem = invSlot.Item;
                if(invItem is TreasureSO)
                {
                    Inventory.Instance.RemoveItem(invSlot.SlotNumber);
                    SetChestSlotUI(chestSO,invItem as TreasureSO, Color.white, invItem.InventoryThumbnail);
                    chestSO.AddTreasure(invItem as TreasureSO);
                }
            }

            ChestSlotPanel chestSlot = eventData.pointerDrag.GetComponent<ChestSlotPanel>();
            if (chestSlot != null)
            {
                chestSO.SwapItems(chestSlot.slotIndex, this.slotIndex);
            }
        }
    }
}
