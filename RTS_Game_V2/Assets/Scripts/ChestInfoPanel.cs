using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChestInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private GameObject[] panelSlots;
    [SerializeField] private Sprite panelSlotSprite;
    [SerializeField] private Color panelSlotColor;
    private ChestSO chestSO;
    private Image slotPanel;
    private void Awake()
    {
        slotPanel = panelSlots[0].GetComponent<Image>();
        panelSlotSprite = slotPanel.sprite;
        panelSlotColor = slotPanel.color;
        GameEvents.instance.OnChestUpdate += ChestUpdate;
    }

    public void ResetPanelSlots()
    {
        for (int i = 0; i < panelSlots.Length; i++)
        {
            ChestSlotPanel chestSlot = panelSlots[i].GetComponentInChildren<ChestSlotPanel>();
            chestSlot.SetEssentials(panelSlotSprite, panelSlotColor, i);
        }
    }

    public void SetChestPanel(ChestSO chestSO)
    {
        ResetPanelSlots();

        this.chestSO = chestSO;
        headerText.text = chestSO.GetNameText();
        List<TreasureSO> treasuerList = chestSO.GetTreasure();

        for (int i = 0; i < treasuerList.Count; i++)
        {
            ChestSlotPanel chestSlotPanel = panelSlots[i].GetComponent<ChestSlotPanel>();
            chestSlotPanel.SetChestSlotUI(this.chestSO,treasuerList[i], Color.white);
        }
    }

    public void ChestUpdate()
    {
        SetChestPanel(chestSO);
    }

}
