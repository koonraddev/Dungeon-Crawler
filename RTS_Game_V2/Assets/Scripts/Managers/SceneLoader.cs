using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool canSwitchScene = false;
    public static SceneLoader instance;
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            canSwitchScene = true;
        }
    }

    private void OnEnable()
    {
        GameEvents.instance.OnLoadGameScene += LoadDungeonScene;
        GameEvents.instance.OnSwitchScene += SwitchScene;
        GameEvents.instance.OnExitToMenu += LoadMenuScene;
    }


    public void LoadMenuScene()
    {
        StartCoroutine(LoadYourAsyncScene(0));
    }

    public void LoadDungeonScene()
    {
        StartCoroutine(LoadYourAsyncScene(1));
    }

    private void SwitchScene()
    {
        canSwitchScene = true;
    }


    IEnumerator LoadYourAsyncScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;
        canSwitchScene = false;
        while (!canSwitchScene && !asyncLoad.isDone)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        if (sceneIndex == 0)
        {
            GameEvents.instance.StartLevel();
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLoadGameScene -= LoadDungeonScene;
        GameEvents.instance.OnSwitchScene -= SwitchScene;
        GameEvents.instance.OnExitToMenu -= LoadMenuScene;
    }

}
