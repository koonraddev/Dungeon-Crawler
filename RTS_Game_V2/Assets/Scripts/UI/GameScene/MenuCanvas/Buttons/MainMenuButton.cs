using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject askPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private TMP_Text infoText;

    public void MainMenuButtonClick()
    {
        infoText.text = "Return to main menu?";
        askPanel.SetActive(true);
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        GameEvents.instance.ExitToMenu();
    }
}
