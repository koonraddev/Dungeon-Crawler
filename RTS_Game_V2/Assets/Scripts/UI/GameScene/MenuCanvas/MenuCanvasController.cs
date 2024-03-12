using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject backgroundObject, mainPanel, optionsMenu, askMenu;

    private PlayerControls playerControls; //TO DO -> menu action
    private InputAction menuAction;
    private bool isMenuActivated = true, isMainPanelActivated, isOptionsPanelActivated, isAskPanelActivated;

    private void OnEnable()
    {
        GameEvents.instance.OnMenuPanel += MenuPanelStatus;
        GameEvents.instance.OnMainPanel += MainPanelStatus;
        GameEvents.instance.OnOptionsPanel += OptionsPanelStatus;
        GameEvents.instance.OnAskPanel += AskPanelStatus;
        GameEvents.instance.OnLoadLevel += CloseAllMenus;
    }

    private void CloseAllMenus()
    {
        GameEvents.instance.MenuPanel(false);
        GameEvents.instance.MainPanel(false);
        GameEvents.instance.AskPanel(false);
        GameEvents.instance.OptionsPanel(false);
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Start()
    {
        menuAction = playerControls.BasicMovement.Menu;
        GameEvents.instance.MenuPanel(false);
        OptionsPanelStatus(false);
        AskPanelStatus(false);
    }

    void Update()
    {
        if (menuAction.triggered)
        {
            GameEvents.instance.MenuPanel(!isMenuActivated);
        }
    }

    private void AskPanelStatus(bool active)
    {
        isAskPanelActivated = active;
        askMenu.SetActive(active);
    }

    private void OptionsPanelStatus(bool active)
    {
        isOptionsPanelActivated = active;
        optionsMenu.SetActive(false);
    }

    private void MainPanelStatus(bool active)
    {
        isMainPanelActivated = active;
        mainPanel.SetActive(isMainPanelActivated);
    }

    private void MenuPanelStatus(bool active)
    {
        isMenuActivated = active;

        backgroundObject.SetActive(isMenuActivated);
        MainPanelStatus(active);

        if (!active)
        {
            OptionsPanelStatus(active);
            AskPanelStatus(active);
            GameEvents.instance.ResumeGame();
        }
        else
        {
            GameEvents.instance.PasueGame();
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnMenuPanel -= MenuPanelStatus;
        GameEvents.instance.OnMainPanel -= MainPanelStatus;
        GameEvents.instance.OnOptionsPanel -= OptionsPanelStatus;
        GameEvents.instance.OnAskPanel -= AskPanelStatus;
        GameEvents.instance.OnLoadLevel -= CloseAllMenus;
    }
}
