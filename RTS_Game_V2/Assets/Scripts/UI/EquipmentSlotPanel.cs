using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class EquipmentSlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private EquipmentSlotType slotType;
    [SerializeField] private Image textureHolder;

    private EquipmentSlot eqSlot;
    public EquipmentSlot EqSlot { get => eqSlot; private set => eqSlot = value; }

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InformationPanel infoPanel;

    private GameObject newObj;
    private RectTransform rect;

    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private Color emptySlotColor;

    private void Awake()
    {
        EqSlot = new(slotType);
    }
    void Start()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.GetInfoPanel();
        infoPanel = infoObject.GetComponent<InformationPanel>();
    }

    private void OnEnable()
    {
        GameEvents.instance.OnEquipmentUpdate += OnEquipmentUpdate;
    }

    private void SetEquipmentSlotUI(EquipmentItem item,Color slotColor)
    {
        EqSlot.Item = item;
        EqSlot.Empty = false;
        textureHolder.color = slotColor;
        textureHolder.sprite = item.Sprite;
    }

    private void SetEmptySlot()
    {
        EqSlot.Item = null;
        EqSlot.Empty = true;
        textureHolder.sprite = emptySlotSprite;
        textureHolder.color = emptySlotColor;
    }

    private void OnEquipmentUpdate()
    {
        EquipmentSlot eqSlot = EquipmentManager.instance.GetEquipmentSlot(slotType);
        if (eqSlot.Empty)
        {
            SetEmptySlot();
        }
        else
        {
            SetEquipmentSlotUI(eqSlot.Item, Color.white);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EqSlot.Empty && infoPanel != null)
        {
            GameEvents.instance.InformationPanel(true);
            infoPanel.SetInfoPanel(EqSlot.Item.Name, EqSlot.Item.Description, EqSlot.Item.Sprite, EqSlot.Item.Statistics);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!EqSlot.Empty && infoPanel != null)
        {
            infoPanel.SetEmpty();
            GameEvents.instance.InformationPanel(false);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!EqSlot.Empty)
        {
            newObj = new GameObject("dragItem", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            rect = newObj.GetComponent<RectTransform>();
            Image dragImage = newObj.GetComponent<Image>();
            CanvasGroup canvGroup = newObj.GetComponent<CanvasGroup>();

            dragImage.sprite = EqSlot.Item.Sprite;
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
            ContainerSlotPanel containerSlotPanel = eventData.pointerDrag.GetComponent<ContainerSlotPanel>();
            if (containerSlotPanel != null)
            {
                ContainerSlot containerSlot = containerSlotPanel.GetContainerSlot();

                if (containerSlot.Item is EquipmentItem eqItem && eqItem.ItemSlot == slotType)
                {
                    if (EquipmentManager.instance.AddItem(containerSlot.Item as EquipmentItem))
                    {
                        containerSlotPanel.SetEmptySlot();
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
