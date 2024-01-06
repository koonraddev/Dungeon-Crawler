using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneCanvasManager : MonoBehaviour
{

    [SerializeField] private GameObject fadePanel;
    [SerializeField] private float fadingTime;
    private CanvasGroup fadeGroup;

    private void Awake()
    {
        fadeGroup = fadePanel.GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        GameEvents.instance.OnRestartFloor += RestartingLevel;
        GameEvents.instance.OnStartLevel += StartingLevel;
        GameEvents.instance.OnExitToMenu += QuitingToMenu;
    }

    private void QuitingToMenu()
    {
        throw new System.NotImplementedException();
    }

    private void RestartingLevel()
    {
        StartCoroutine(WaitForFadeInAndLoadLevel());
    }

    private void StartingLevel()
    {
        StartCoroutine(WaitForFadeOutAndStartGame());
    }

    IEnumerator WaitForFadeOutAndStartGame()
    {
        yield return FadeOutInfoPanel().WaitForCompletion();
        fadeGroup.blocksRaycasts = false;
    }

    IEnumerator WaitForFadeInAndLoadLevel()
    {
        fadeGroup.blocksRaycasts = true;
        yield return FadeInInfoPanel().WaitForCompletion();
        GameEvents.instance.LoadLevel();
    }

    private Tween FadeOutInfoPanel()
    {
        return fadeGroup.DOFade(0, fadingTime);
    }

    private Tween FadeInInfoPanel()
    {
        return fadeGroup.DOFade(1, fadingTime);
    }

    private void OnDisable()
    {
        GameEvents.instance.OnRestartFloor -= RestartingLevel;
        GameEvents.instance.OnStartLevel -= StartingLevel;
        GameEvents.instance.OnExitToMenu -= QuitingToMenu;
    }

}
