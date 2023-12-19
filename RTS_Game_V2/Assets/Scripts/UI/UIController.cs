using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerInventoryUI;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject equipmentPanel;
    [SerializeField] private GameObject inventoryDropPanel;
    [SerializeField] private GameObject containerPanel;
    [SerializeField] private GameObject statisticsPanel;
    [SerializeField] private GameObject enemyInformationPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject mapPanel;

    private Vector3 playerUIPos, infoPanelPos, eqPanelPos, invDropPanelPos, containerPanelPos, statsPanelPos, enemyPanelPos, mapPanelPos;
    [SerializeField] private Vector3 miniMapPanelPos;

    private PlayerControls playerControls;
    private InputAction inventoryAction;
    private InputAction statisticsAction;
    private bool isInventoryActivated;
    private bool fullSizeMapMode,isStatisticsActivated = false;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerUIPos = playerInventoryUI.transform.position;
        infoPanelPos = informationPanel.transform.position;
        eqPanelPos = equipmentPanel.transform.position;
        invDropPanelPos = inventoryDropPanel.transform.position;
        containerPanelPos = containerPanel.transform.position;
        statsPanelPos = statisticsPanel.transform.position;
        enemyPanelPos = enemyInformationPanel.transform.position;
        mapPanelPos = mapPanel.transform.localPosition;
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
        GameEvents.instance.OnEnemyClick += EnemyClick;
        GameEvents.instance.OnLoadNextLevel += ActiveLoadingPanel;
        GameEvents.instance.OnMapPanel += FullSizeMap;
        inventoryAction = playerControls.BasicMovement.Inventory;
        statisticsAction = playerControls.BasicMovement.Statistics;
        InventoryPanelStatus(false);
        InformationPanelStatus(false);
        StatisticsPanelStatus(false);
        EnemyClick(null);
    }

    private void Update()
    {
        if (inventoryAction.triggered)
        {
            GameEvents.instance.InventoryPanel(!isInventoryActivated);
            GameEvents.instance.CancelGameObjectAction();
        }

        if (statisticsAction.triggered)
        {
            GameEvents.instance.StatisticPanel(!isStatisticsActivated);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameEvents.instance.MapPanel(!fullSizeMapMode);
        }
    }

    public void ActiveLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }


    public void EnemyClick(Enemy enemy)
    {
        if(enemy != null)
        {
            //enemyInformationPanel.SetActive(true);
            enemyInformationPanel.transform.position = enemyPanelPos;
            EnemyInformationPanel enemyInfo = enemyInformationPanel.GetComponent<EnemyInformationPanel>();
            enemyInfo.SetPanel(enemy);
        }
        else
        {
            //enemyInformationPanel.SetActive(false);
            enemyInformationPanel.transform.position = enemyPanelPos + new Vector3(5000, 0, 0);
        }

    }

    public void FullSizeMap(bool activeFullSizeMap)
    {
        fullSizeMapMode = activeFullSizeMap;
        if (activeFullSizeMap)
        {
            mapPanel.transform.localScale = Vector3.one;
            mapPanel.transform.localPosition = mapPanelPos;
        }
        else
        {
            mapPanel.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            mapPanel.transform.localPosition = miniMapPanelPos;
        }
    }

    public void InventoryPanelStatus(bool active)
    {
        isInventoryActivated = active;
        //equipmentPanel.SetActive(active);
        //playerInventoryUI.SetActive(active);
        //inventoryDropPanel.SetActive(active);
        if (active)
        {
            equipmentPanel.transform.position = eqPanelPos;
            playerInventoryUI.transform.position = playerUIPos;
            inventoryDropPanel.transform.position = invDropPanelPos;
        }
        else
        {
            equipmentPanel.transform.position = eqPanelPos + new Vector3(5000, 0, 0);
            playerInventoryUI.transform.position = playerUIPos + new Vector3(5000, 0, 0);
            inventoryDropPanel.transform.position = invDropPanelPos + new Vector3(5000, 0, 0);
        }

        if (!active)
        {

            //containerPanel.SetActive(active);
            
            containerPanel.transform.position = containerPanelPos + new Vector3(5000, 0, 0);
        }


    }

    public void InformationPanelStatus(bool active)
    {
        //informationPanel.SetActive(active);
        if (active)
        {
            informationPanel.transform.position = infoPanelPos;
        }
        else
        {
            informationPanel.transform.position = infoPanelPos + new Vector3(5000, 0, 0);
        }

    }

    public void StatisticsPanelStatus(bool active)
    {
        isStatisticsActivated = active;
        //statisticsPanel.SetActive(active);

        if (active)
        {
            statisticsPanel.transform.position = statsPanelPos;
        }
        else
        {
            statisticsPanel.transform.position = statsPanelPos +new Vector3(5000, 0, 0);
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnInventoryPanelOpen -= InventoryPanelStatus;
        GameEvents.instance.OnInformationPanel -= InformationPanelStatus;
        GameEvents.instance.OnStatisticPanel -= StatisticsPanelStatus;
        GameEvents.instance.OnEnemyClick -= EnemyClick;
        GameEvents.instance.OnLoadNextLevel -= ActiveLoadingPanel;
        GameEvents.instance.OnMapPanel -= FullSizeMap;
    }

    public GameObject GetInfoPanel()
    {
        return informationPanel;
    }
}
