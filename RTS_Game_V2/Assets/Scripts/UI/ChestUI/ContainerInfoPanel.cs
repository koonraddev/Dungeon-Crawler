using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerInfoPanel : MonoBehaviour
{
    public static ContainerInfoPanel instance;

    [SerializeField] private TMP_Text headerText;
    [SerializeField] private GameObject[] panelSlots;
    [SerializeField] private Sprite panelSlotSprite;
    [SerializeField] private Color panelSlotColor;
    private Image slotPanel;

    private Container container;

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
        GameEvents.instance.OnContainerUpdate += ContainerUpdate;
    }

    public void ResetPanelSlots()
    {
        for (int i = 0; i < panelSlots.Length; i++)
        {
            ContainerSlotPanel containerSlotPanel = panelSlots[i].GetComponentInChildren<ContainerSlotPanel>();
            containerSlotPanel.SetEssentials(panelSlotSprite, panelSlotColor, i);
            containerSlotPanel.gameObject.SetActive(false);
        }
    }

    public void SetAndActiveContainerPanel(Container container)
    {
        gameObject.SetActive(true);
        ResetPanelSlots();

        this.container = container;
        headerText.text = container.Name;
        List<ContainerSlot> contList = container.Slots;

        for (int i = 0; i < contList.Count; i++)
        {
            ContainerSlotPanel containerSlotPanel = panelSlots[i].GetComponent<ContainerSlotPanel>();
            containerSlotPanel.SetContainerSlotUI(container,contList[i], Color.white);
            containerSlotPanel.gameObject.SetActive(true);
        }

    }

    public void ContainerUpdate()
    {
        SetAndActiveContainerPanel(container);
    }

}
