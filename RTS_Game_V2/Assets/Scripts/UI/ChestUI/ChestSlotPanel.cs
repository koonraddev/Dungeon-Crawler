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
    private InformationPanel infoPanel;

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
        infoPanel = infoObject.GetComponent<InformationPanel>();
        nameHolder = GetComponentInChildren<TMP_Text>();
        textureHolder = GetComponent<Image>();
    }
    public void SetChestSlotUI(ChestSO chestSO,TreasureSO treasureSO, Color slotColor)
    {
        this.chestSO = chestSO;
        textureHolder.color = panelSlotColor;
        textureHolder.sprite = this.panelSlotSprite;

        if (treasureSO != null)
        {
            this.treasureSO = treasureSO;
            nameHolder.text = treasureSO.GetName();
            textureHolder.color = slotColor;
            textureHolder.sprite = treasureSO.GetThumbnail();
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
        chestSO.RemoveTreasure(slotIndex);
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
                    if (chestSO.AddTreasure(slotIndex, invItem as TreasureSO))
                    {
                        Inventory.Instance.RemoveItem(invSlot.SlotNumber);
                        SetChestSlotUI(chestSO, invItem as TreasureSO, Color.white);
                    }
                }
            }

            ChestSlotPanel chestSlot = eventData.pointerDrag.GetComponent<ChestSlotPanel>();
            if (chestSlot != null)
            {
                chestSO.SwapItems(chestSlot.slotIndex, this.slotIndex);
            }

            EquipmentSlotPanel eqSlot = eventData.pointerDrag.GetComponent<EquipmentSlotPanel>();
            if(eqSlot != null)
            {
                IEquipmentItem eqItem = eqSlot.Item;
                if(eqItem is TreasureSO)
                {
                    if (chestSO.AddTreasure(slotIndex, eqItem as TreasureSO))
                    {
                        Equipment.Instance.RemoveItem(eqItem);
                        SetChestSlotUI(chestSO, eqItem as TreasureSO, Color.white);
                    }
                }
            }
        }
    }
}
