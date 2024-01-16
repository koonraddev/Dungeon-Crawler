using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;

    private bool loadingComplete;
    private void OnEnable()
    {
        GameEvents.instance.OnGeneratingReady += GameIsReady;
        GameEvents.instance.OnRestartFloor += SetRestartingInfo;
        GameEvents.instance.OnLoadLevel += SetLoadingInfo;
        GameEvents.instance.OnLoadGameScene += SetLoadingInfo;
        GameEvents.instance.OnGameOver += SetGameOverInfo;
    }

    private void SetGameOverInfo()
    {
        tmpText.text = "";
        loadingComplete = false;
    }

    private void SetRestartingInfo()
    {
        tmpText.text = "Restarting Floor";
        loadingComplete = false;
    }


    private void SetLoadingInfo()
    {
        tmpText.text = "Loading level";
        loadingComplete = false;
    }
    private void GameIsReady()
    {
        tmpText.text = "Loading Complete.\n Press Space to continue.";
        loadingComplete = true;
    }

    void Update()
    {
        if(loadingComplete & Input.GetKeyDown(KeyCode.Space))
        {
            GameEvents.instance.StartLevel();
        }
    }
    private void OnDisable()
    {
        GameEvents.instance.OnGeneratingReady -= GameIsReady;
        GameEvents.instance.OnRestartFloor -= SetRestartingInfo;
        GameEvents.instance.OnLoadLevel -= SetLoadingInfo;
        GameEvents.instance.OnLoadGameScene -= SetLoadingInfo;
    }

}
