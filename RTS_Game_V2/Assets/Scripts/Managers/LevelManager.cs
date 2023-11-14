using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int level;

    private void Awake()
    {
        instance = this;
    }
    public int Level
    {
        get { return level; }
        set { level = value+1; }
    }

    private void OnEnable()
    {
        GameEvents.instance.OnSavedPlayerData += LoadLevel;
    }

    private void LoadLevel()
    {
        level += 1;
        RoomsGenerator.instance.RoomsToGenerate = level;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            GameEvents.instance.ActivateTeleport();
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnSavedPlayerData -= LoadLevel;
    }
}
