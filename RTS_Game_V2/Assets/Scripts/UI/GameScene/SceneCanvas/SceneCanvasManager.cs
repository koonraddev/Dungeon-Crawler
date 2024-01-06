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
        DontDestroyOnLoad(this);
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this);
        //    fadeGroup = fadePanel.GetComponent<CanvasGroup>();
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        fadeGroup = fadePanel.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        FadeOutInfoPanel();
        fadeGroup.blocksRaycasts = false;
    }
    private void OnEnable()
    {
        GameEvents.instance.OnRestartFloor += RestartingLevel;
        GameEvents.instance.OnStartLevel += StartingLevel;
        GameEvents.instance.OnExitToMenu += QuitingToMenu;
        GameEvents.instance.OnLoadGameScene += LoadingDungeonScene;
    }

    private void LoadingDungeonScene()
    {
        StartCoroutine(WaitForFadeInAndSwitchScene());
    }

    private void QuitingToMenu()
    {
        StartCoroutine(WaitForFadeInAndSwitchScene());
    }

    private void RestartingLevel()
    {
        StartCoroutine(WaitForFadeInAndLoadLevel());
    }

    private void StartingLevel()
    {
        StartCoroutine(WaitForFadeOutAndStartGame());
    }

    IEnumerator WaitForFadeInAndSwitchScene()
    {
        Debug.LogWarning("FADING");
        fadeGroup.blocksRaycasts = true;
        yield return FadeInInfoPanel().WaitForCompletion();
        GameEvents.instance.SwitchScene();
    }

    IEnumerator WaitForFadeOutAndStartGame()
    {
        yield return FadeOutInfoPanel().WaitForCompletion();
        fadeGroup.blocksRaycasts = false;
        //GameEvents.instance.StartLevel();
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
        Debug.LogWarning("DISABLED");
        GameEvents.instance.OnRestartFloor -= RestartingLevel;
        GameEvents.instance.OnStartLevel -= StartingLevel;
        GameEvents.instance.OnExitToMenu -= QuitingToMenu;
        GameEvents.instance.OnLoadGameScene -= LoadingDungeonScene;
    }

}
