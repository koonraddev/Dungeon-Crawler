using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneCanvasManager : MonoBehaviour
{
    [SerializeField] private float fadingTime;
    [SerializeField] private CanvasGroup fadeGroup;
    Tween fadeOutTweener, fadeInTweener;
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
        //fadeGroup = fadePanel.GetComponent<CanvasGroup>();

        fadeOutTweener = fadeGroup.DOFade(0, fadingTime);
        fadeInTweener = fadeGroup.DOFade(1, fadingTime);    
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
        GameEvents.instance.OnGameOver += WaitAndFadeInFadeOut;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            FadeOutInfoPanel();
        }
    }
    private void WaitAndFadeInFadeOut()
    {
        Invoke(nameof(FadeInInfoPanel), 2f);
        Invoke(nameof(FadeOutInfoPanel), 4f);
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
        //Debug.LogWarning("FADING");
        fadeGroup.blocksRaycasts = true;
        yield return FadeInInfoPanel().WaitForCompletion();
        //fadeInTweener.Rewind();
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
        fadeOutTweener.Rewind();
        return fadeOutTweener.Play();
    }

    private Tween FadeInInfoPanel()
    {
        fadeInTweener.Rewind();
        return fadeInTweener.Play();
    }

    private void OnDisable()
    {
        Debug.LogWarning("DISABLED");
        GameEvents.instance.OnRestartFloor -= RestartingLevel;
        GameEvents.instance.OnStartLevel -= StartingLevel;
        GameEvents.instance.OnExitToMenu -= QuitingToMenu;
        GameEvents.instance.OnLoadGameScene -= LoadingDungeonScene;
        GameEvents.instance.OnGameOver += WaitAndFadeInFadeOut;
    }

}
