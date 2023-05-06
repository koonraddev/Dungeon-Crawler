using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameHolder;
    public TMP_Text amountHolder;
    [HideInInspector] public IInventoryItem Item { get; set; }
    [HideInInspector] public int SlotNumber { get; set; }

    private GameObject canvas;
    private UIController uiCtrl;
    private GameObject infoObject;
    private InventoryInfoPanel infoPanel;
    void Start()
    {
        canvas = transform.root.gameObject;
        uiCtrl = canvas.GetComponent<UIController>();
        infoObject = uiCtrl.InventoryInfoPanel;
        infoPanel = infoObject.GetComponent<InventoryInfoPanel>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item != null)
        {
            Inventory.Instance.RemoveItem(SlotNumber);
            infoPanel.SetInfoPanel("", "");
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
}
