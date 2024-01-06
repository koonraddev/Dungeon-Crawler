using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool canSwitchScene = false;
    float time;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void OnEnable()
    {
        //GameEvents.instance.OnSavedPlayerData += LoadDungeonScene;
        GameEvents.instance.OnSwitchScene += SwitchScene;
        GameEvents.instance.OnExitToMenu += LoadMenuScene;
    }


    public void LoadMenuScene()
    {
        StartCoroutine(LoadYourAsyncScene(0));
    }

    public void LoadDungeonScene()
    {
        //StartCoroutine(LoadYourAsyncScene(2));
        StartCoroutine(LoadYourAsyncScene(1));
    }

    private void SwitchScene()
    {
        canSwitchScene = true;
    }


    IEnumerator LoadYourAsyncScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        //asyncLoad.allowSceneActivation = false;
        asyncLoad.allowSceneActivation = true;
        time = 0;
        while (!asyncLoad.isDone)
        {
            if(asyncLoad.progress <= 0.9f)
            {
                Debug.Log("PROGRESS: " + asyncLoad.progress + " |_______| " + time);
            }

            //if (canSwitchScene)
            //{
            //    asyncLoad.allowSceneActivation = true;
            //    canSwitchScene = false;
            //}
            yield return null;
        }
    }

    private void OnDisable()
    {
        //GameEvents.instance.OnSavedPlayerData -= LoadDungeonScene;
        GameEvents.instance.OnSwitchScene -= SwitchScene;
        GameEvents.instance.OnExitToMenu -= LoadMenuScene;
    }

}
