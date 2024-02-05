using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class GameOverCanvasController : MonoBehaviour
{
    [SerializeField] GameObject background, askPanel, restartButton, mainMenuButton;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private CanvasGroup buttonPanelCanvasGroup;
    Tween seq;
    private void OnEnable()
    {
        GameEvents.instance.OnGameOver += WaitAndFadeInFadeOut;
        GameEvents.instance.OnStartLevel += HideAllElements;
    }

    private void Awake()
    {
        seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                restartButton.SetActive(true);
                mainMenuButton.SetActive(true);
                background.SetActive(true);
                buttonPanelCanvasGroup.gameObject.SetActive(true);
                labelText.gameObject.SetActive(true);
            }).SetDelay(4f)
            .Append(labelText.DOFade(1, 1f))
            .Append(buttonPanelCanvasGroup.DOFade(1, 1f).SetDelay(1f));
    }
    private void WaitAndFadeInFadeOut()
    {
        seq.Restart();
        seq.Play();
    }

    private void HideAllElements()
    {
        buttonPanelCanvasGroup.alpha = 0f;
        labelText.alpha = 0f;
        
        background.SetActive(false);
        buttonPanelCanvasGroup.gameObject.SetActive(false);
        labelText.gameObject.SetActive(false);
        askPanel.SetActive(false);
    }
    private void OnDisable()
    {
        GameEvents.instance.OnGameOver -= WaitAndFadeInFadeOut;
        GameEvents.instance.OnStartLevel -= HideAllElements;
    }
}
