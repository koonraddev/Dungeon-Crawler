using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private EquipmentSlotType slotType;
    [SerializeField] private Image textureHolder;
    private Sprite emptySlotSprite;
    private Canvas canvas;

    private EquipmentSlot eqSlot;
    public EquipmentSlot EqSlot { get => eqSlot; private set => eqSlot = value; }

    private GameObject infoObject,canvasObject, newObj;
    private UICanvasController uiCtrl;
    private InformationPanel infoPanel;

    private RectTransform rect;

    private Color emptySlotColor;

    private void Awake()
    {
        EqSlot = new(slotType);

        emptySlotSprite = textureHolder.sprite;
        emptySlotColor = textureHolder.color;


        canvasObject = transform.root.gameObject;
        uiCtrl = canvasObject.GetComponent<UICanvasController>();
        infoObject = uiCtrl.InfoPanel;
        infoPanel = infoObject.GetComponent<InformationPanel>();
        canvas = canvasObject.GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        GameEvents.instance.OnEquipmentUpdate += OnEquipmentUpdate;
    }

    private void SetEquipmentSlotUI(EquipmentItem item)
    {
        EqSlot.Item = item;
        EqSlot.Empty = false;
        textureHolder.color = Color.white;
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
            SetEquipmentSlotUI(eqSlot.Item);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EqSlot.Empty && infoPanel != null)
        {
            GameEvents.instance.InformationPanel(true);
            infoPanel.SetInfoPanel(EqSlot.Item);
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
            rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
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
            if (eventData.pointerDrag.TryGetComponent(out ContainerSlotPanel containerSlotPanel))
            {
                ContainerSlot containerSlot = containerSlotPanel.ContainerSlot;

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
