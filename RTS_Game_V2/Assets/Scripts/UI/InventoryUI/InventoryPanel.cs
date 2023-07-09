using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;

    private GameObject[] slotsArray;

    private Image slotPanel;
    private Sprite emptySlotSprite;
    private Color emptySlotColor;

    void Start()
    {
        slotPanel = slotPrefab.GetComponent<Image>();
        emptySlotSprite = slotPanel.sprite;
        emptySlotColor = slotPanel.color;

        PrepareInventoryUI();
    }

    private void PrepareInventoryUI()
    {
        slotsArray = new GameObject[Inventory.Instance.GetInventorySize()];
        if (slotPrefab != null)
        {
            GameObject tmp;
            for (int i = 0; i < slotsArray.Length; i++)
            {
                tmp = Instantiate(slotPrefab);
                tmp.name = "PanelSlot" + i;
                tmp.transform.SetParent(gameObject.transform);
                RectTransform rtTmp = tmp.GetComponent<RectTransform>();
                rtTmp.anchoredPosition = new Vector3(0f, -i * 108, 0f);

                InventorySlotPanel slotInter = tmp.GetComponent<InventorySlotPanel>();
                slotInter.SlotNumber = i;
                slotsArray[i] = tmp;
                tmp.SetActive(true);
            }
            RectTransform rtObj = gameObject.GetComponent<RectTransform>();
            rtObj.sizeDelta = new Vector2(105f, slotsArray.Length * 108f);
        }

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
                InventorySlotPanel invInter = slotsArray[i].GetComponent<InventorySlotPanel>();

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