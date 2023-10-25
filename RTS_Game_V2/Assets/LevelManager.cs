using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private int level;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private void OnEnable()
    {
        GameEvents.instance.OnSavedPlayerData += LoadLevel;
    }

    private void LoadLevel()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        GameEvents.instance.OnSavedPlayerData -= LoadLevel;
    }
}
