using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
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
        slotsArray = new GameObject[Inventory.Instance.InventorySize];
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

                InventorySlotUI slotInter = tmp.GetComponent<InventorySlotUI>();
                slotInter.SlotNumber = i;
                slotsArray[i] = tmp;
                tmp.SetActive(true);
            }
            RectTransform rtObj = gameObject.GetComponent<RectTransform>();
            rtObj.sizeDelta = new Vector2(105f, slotsArray.Length * 108f);
        }


        GameEvents.instance.onInventoryUpdate += OnInventoryUpdate;
        OnInventoryUpdate();
    }

    private void OnInventoryUpdate()
    {
        Inventory.InventorySlot[] items = Inventory.Instance.GetInventorySlots();
        for (int i = 0; i < items.Length; i++)
        {
            InventorySlotUI invInter = slotsArray[i].GetComponent<InventorySlotUI>();
            Image image = slotsArray[i].GetComponent<Image>();
            TMP_Text nameHolder = invInter.nameHolder;
            TMP_Text namountHolder = invInter.amountHolder;
            if (items[i] != null)
            {
                nameHolder.text = items[i].SlotName;
                namountHolder.text = items[i].itemAmount.ToString();
                image.sprite = items[i].ItemInSlot.InventoryTexture;
                image.color = Color.white;
                invInter.Item = items[i].ItemInSlot;

            }
            else
            {
                nameHolder.text = "";
                namountHolder.text = "";
                image.sprite = emptySlotSprite;
                image.color = emptySlotColor;
                invInter.Item = null;
            }
        }
    }

    private void OnDestroy()
    {
        GameEvents.instance.onInventoryUpdate -= OnInventoryUpdate;
    }
}
