using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] panelSlots;
    [SerializeField] private Image slotPanel;
    private Sprite emptySlotSprite;
    private Color emptySlotColor;

    void Start()
    {
        emptySlotSprite = slotPanel.sprite;
        emptySlotColor = slotPanel.color;
        GameEvents.instance.OnInventoryUpdate += OnInventoryUpdate;
        OnInventoryUpdate();
    }

    private void OnInventoryUpdate()
    {
        InventorySO.InventorySlot[] items = Inventory.Instance.GetInventorySlots();
        if(items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                InventorySlotPanel invInter = panelSlots[i].GetComponent<InventorySlotPanel>();

                if (items[i] != null)
                {
                    invInter.SetInventorySlotUI(items[i].ItemInSlot, items[i].ItemAmount, Color.white);
                }
                else
                {
                    invInter.SetEmptySlot(emptySlotSprite, emptySlotColor);
                }
            }
        }
        else
        {
            Debug.Log("NULL");
        }

    }

    private void OnDestroy()
    {
        GameEvents.instance.OnInventoryUpdate -= OnInventoryUpdate;
    }
}
