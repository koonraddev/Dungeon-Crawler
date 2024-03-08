using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class GameOverCanvasController : MonoBehaviour
{
    [SerializeField] GameObject background, dividerLeft, dividerRight, askPanel, restartButton, mainMenuButton;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private CanvasGroup buttonPanelCanvasGroup;
    [SerializeField] private Image backgroundImage, dividerLeftImage, dividerRightImage;
    Tween seq;
    private void OnEnable()
    {
        GameEvents.instance.OnGameOver += WaitAndFadeInFadeOut;
        GameEvents.instance.OnStartLevel += HideAllElements;
    }

    private void Awake()
    {
        seq = DOTween.Sequence()
            .Append(backgroundImage.DOFade(1, 0.3f))
            .Append(labelText.DOFade(1, 1f)).SetDelay(2f)
            .Join(dividerLeftImage.DOFade(1, 1f))
            .Join(dividerRightImage.DOFade(1, 1f)).SetDelay(2f)
            .Append(buttonPanelCanvasGroup.DOFade(1, 1f).SetDelay(1f));
    }
    private void WaitAndFadeInFadeOut()
    {
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
        background.SetActive(true);
        dividerLeft.SetActive(true);
        dividerRight.SetActive(true);
        buttonPanelCanvasGroup.gameObject.SetActive(true);
        labelText.gameObject.SetActive(true);
        seq.Restart();
    }

    private void HideAllElements()
    {
        buttonPanelCanvasGroup.alpha = 0f;
        labelText.alpha = 0f;

        backgroundImage.color = new(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f);
        dividerLeftImage.color = new(dividerLeftImage.color.r, dividerLeftImage.color.g, dividerLeftImage.color.b, 0f);
        dividerRightImage.color = new(dividerRightImage.color.r, dividerRightImage.color.g, dividerRightImage.color.b, 0f);

        background.SetActive(false);
        dividerLeft.SetActive(false);
        dividerRight.SetActive(false);
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
