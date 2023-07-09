using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerInventoryUI;
    [SerializeField] private GameObject inventoryInfoPanel;
    [SerializeField] private GameObject inventoryDeletePanel;
    [SerializeField] private GameObject inventoryDropPanel;
    [SerializeField] private GameObject chestInfoPanel;

    private PlayerControls playerControls;
    private InputAction inventoryAction;
    private bool isInventoryActivated;

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
        GameEvents.instance.OnInventoryPanel += InventoryPanelStatus;
        inventoryAction = playerControls.BasicMovement.Inventory;
        InventoryPanelStatus(false);
    }



    private void Update()
    {
        if (inventoryAction.triggered)
        {
            InventoryPanelStatus(!isInventoryActivated);
            GameEvents.instance.CancelGameObjectAction();
        }
    }


    public void InventoryPanelStatus(bool active)
    {
        isInventoryActivated = active;
        playerInventoryUI.SetActive(active);
        inventoryInfoPanel.SetActive(active);
        inventoryDeletePanel.SetActive(active);
        inventoryDropPanel.SetActive(active);
        if (!active)
        {
            chestInfoPanel.SetActive(active);
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnInventoryPanel -= InventoryPanelStatus;
    }

    public GameObject GetInfoPanel()
    {
        return inventoryInfoPanel;
    }
}
