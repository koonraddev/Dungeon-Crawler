using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
        GameEvents.instance.OnExitToMenu += SetLoadingMenuInfo;
    }

    private void SetLoadingMenuInfo()
    {
        tmpText.text = "Loading Menu";
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
            loadingComplete = false;
        }
    }
    private void OnDisable()
    {
        GameEvents.instance.OnGeneratingReady -= GameIsReady;
        GameEvents.instance.OnRestartFloor -= SetRestartingInfo;
        GameEvents.instance.OnLoadLevel -= SetLoadingInfo;
        GameEvents.instance.OnLoadGameScene -= SetLoadingInfo;
        GameEvents.instance.OnExitToMenu -= SetLoadingMenuInfo;
    }

}
