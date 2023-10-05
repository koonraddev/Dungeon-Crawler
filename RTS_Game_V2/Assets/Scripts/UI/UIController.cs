using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerInventoryUI;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject equipmentPanel;
    [SerializeField] private GameObject inventoryDropPanel;
    [SerializeField] private GameObject chestInfoPanel;
    [SerializeField] private GameObject statisticsPanel;

    private PlayerControls playerControls;
    private InputAction inventoryAction;
    private InputAction statisticsAction;
    private bool isInventoryActivated;
    private bool isStatisticsActivated = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();


    }
    private void Start()
    {
        GameEvents.instance.OnInventoryPanelOpen += InventoryPanelStatus;
        GameEvents.instance.OnInformationPanel += InformationPanelStatus;
        GameEvents.instance.OnStatisticPanel += StatisticsPanelStatus;
        inventoryAction = playerControls.BasicMovement.Inventory;
        statisticsAction = playerControls.BasicMovement.Statistics;
        InventoryPanelStatus(false);
    }

    private void Update()
    {
        if (inventoryAction.triggered)
        {
            InventoryPanelStatus(!isInventoryActivated);
            GameEvents.instance.CancelGameObjectAction();
        }

        if (statisticsAction.triggered)
        {
            GameEvents.instance.StatisticPanel(!isStatisticsActivated);
        }
    }


    public void InventoryPanelStatus(bool active)
    {
        isInventoryActivated = active;
        equipmentPanel.SetActive(active);
        playerInventoryUI.SetActive(active);
        inventoryDropPanel.SetActive(active);
        if (!active)
        {
            chestInfoPanel.SetActive(active);
        }
    }

    public void InformationPanelStatus(bool active)
    {
        informationPanel.SetActive(active);
    }

    public void StatisticsPanelStatus(bool active)
    {
        isStatisticsActivated = active;
        statisticsPanel.SetActive(active);
    }

    private void OnDisable()
    {
        GameEvents.instance.OnInventoryPanelOpen -= InventoryPanelStatus;
        GameEvents.instance.OnInformationPanel -= InformationPanelStatus;
    }

    public GameObject GetInfoPanel()
    {
        return informationPanel;
    }
}
