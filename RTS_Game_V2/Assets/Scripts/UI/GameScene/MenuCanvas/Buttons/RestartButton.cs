using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private GameObject askPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private TMP_Text infoText;

    public void RestartButtonClick()
    {
        infoText.text = "Restart level?";
        askPanel.SetActive(true);
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        GameEvents.instance.RestartGame();
    }
}
