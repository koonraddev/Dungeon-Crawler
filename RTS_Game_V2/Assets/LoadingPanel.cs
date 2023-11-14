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
        GameEvents.instance.OnLastRoomReady += GameIsReady;
        loadingComplete = false;
        SetText("Loading level...");
    }
    private void GameIsReady()
    {
        SetText("Loading Complete.\n Press Space to continue.");
        loadingComplete = true;
    }

    public void SetText(string text)
    {
        tmpText.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        if(loadingComplete & Input.GetKeyDown(KeyCode.Space))
        {
            GameEvents.instance.StartLevel();
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        GameEvents.instance.OnLastRoomReady -= GameIsReady;
    }

}
