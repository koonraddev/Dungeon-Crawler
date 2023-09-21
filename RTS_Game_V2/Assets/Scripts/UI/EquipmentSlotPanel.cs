using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private ItemSlotType slotType;
    [SerializeField] private Image textureHolder;
    public IEquipmentItem Item { get; private set; }

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InformationPanel infoPanel;

    private GameObject newObj;
    private RectTransform rect;

    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private Color emptySlotColor;

    void Start()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.GetInfoPanel();
        infoPanel = infoObject.GetComponent<InformationPanel>();

        GameEvents.instance.OnEquipmentUpdate += OnEquipmentUpdate;
    }

    public void SetInventorySlotUI(IEquipmentItem item, Color slotColor)
    {
        Debug.Log("UI");
        Item = item;
        textureHolder.color = slotColor;
        textureHolder.sprite = item.EquipmentThumbnail;
    }

    public void SetEmptySlot()
    {
        textureHolder.sprite = emptySlotSprite;
        textureHolder.color = emptySlotColor;
        Item = null;
    }

    private void OnEquipmentUpdate()
    {
        EquipmentSlot eqSlot = Equipment.Instance.GetEquipmentSlot(slotType);
        if (eqSlot != null)
        {
            if(eqSlot.ItemInSlot != null)
            {
                SetInventorySlotUI(eqSlot.ItemInSlot, Color.white);
            }
            else
            {
                SetEmptySlot();
            }
        }
        else
        {
            Debug.Log("NULL");
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
        if (Item != null)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            rect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();

            dragImage.sprite = Item.EquipmentThumbnail;
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

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ChestSlotPanel chestSlot = eventData.pointerDrag.GetComponent<ChestSlotPanel>();
            if (chestSlot != null)
            {
                TreasureSO treasure = chestSlot.GetTreasure();

                if (treasure is IEquipmentItem)
                {
                    // object myObject implements 
                    if (Equipment.Instance.AddItem(treasure as IEquipmentItem))
                    {
                        chestSlot.SetEmptySlot();
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnEquipmentUpdate -= OnEquipmentUpdate;
    }

}
