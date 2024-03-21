using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SceneCanvasManager : MonoBehaviour
{
    [SerializeField] private float fadingTime;
    [SerializeField] private CanvasGroup fadeGroup;
    Tween fadeOutTweener, fadeInTweener;
    private void Awake()
    {
        DontDestroyOnLoad(this);

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
        fadeGroup.blocksRaycasts = true;
        yield return FadeInInfoPanel().WaitForCompletion();
        GameEvents.instance.SwitchScene();
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
        fadeOutTweener.Restart();
        return fadeGroup.DOFade(0, fadingTime).Play();
    }

    private Tween FadeInInfoPanel()
    {
        fadeInTweener.Restart();
        return fadeGroup.DOFade(1, fadingTime).Play();
    }

    private void OnDisable()
    {
        GameEvents.instance.OnRestartFloor -= RestartingLevel;
        GameEvents.instance.OnStartLevel -= StartingLevel;
        GameEvents.instance.OnExitToMenu -= QuitingToMenu;
        GameEvents.instance.OnLoadGameScene -= LoadingDungeonScene;
    }

}
