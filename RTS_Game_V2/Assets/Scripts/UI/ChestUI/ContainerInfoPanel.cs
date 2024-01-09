using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerInfoPanel : MonoBehaviour
{
    public static ContainerInfoPanel instance;

    [SerializeField] private TMP_Text headerText;
    [SerializeField] private GameObject[] panelSlots;
    private Container container;
    private Vector3 startPos;

    private void Awake()
    {
        instance = this;

        startPos = gameObject.transform.position;
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
            containerSlotPanel.SetEssentials(i);
            containerSlotPanel.gameObject.SetActive(false);
        }
    }

    public void SetAndActiveContainerPanel(Container container)
    {
        this.transform.position = startPos;
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
