using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContainerInfoPanel : MonoBehaviour
{
    public static ContainerInfoPanel instance;

    [SerializeField] private TMP_Text headerText;
    [SerializeField] private GameObject[] panelSlots;
    [SerializeField] private Sprite panelSlotSprite;
    [SerializeField] private Color panelSlotColor;
    private Image slotPanel;

    private ContainerObject container;

    private void Awake()
    {
        instance = this;
        slotPanel = panelSlots[0].GetComponent<Image>();
        panelSlotSprite = slotPanel.sprite;
        panelSlotColor = slotPanel.color;

        gameObject.SetActive(false);
    }

    private void Start()
    {
        GameEvents.instance.OnChestUpdate += ChestUpdate;
    }

    public void ResetPanelSlots()
    {
        for (int i = 0; i < panelSlots.Length; i++)
        {
            ContainerSlotPanel chestSlot = panelSlots[i].GetComponentInChildren<ContainerSlotPanel>();
            chestSlot.SetEssentials(panelSlotSprite, panelSlotColor, i);
        }
    }

    public void SetAndActiveChestPanel(ContainerObject container)
    {
        gameObject.SetActive(true);
        ResetPanelSlots();

        this.container = container;
        headerText.text = container.contName;
        List<ContainerSlot> contList = container.contSlots;

        for (int i = 0; i < contList.Count; i++)
        {
            ContainerSlotPanel chestSlotPanel = panelSlots[i].GetComponent<ContainerSlotPanel>();
            chestSlotPanel.SetContainerSlotUI(container,contList[i], Color.white);
        }

    }

    public void ChestUpdate()
    {
        SetAndActiveChestPanel(container);
    }

}
