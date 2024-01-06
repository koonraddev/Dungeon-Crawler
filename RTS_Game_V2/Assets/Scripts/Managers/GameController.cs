using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameStatus
{
    START,
    ON,
    PREPARING,
    PAUSED,
    END
}
public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject startSpawnPoint;
    public List<GameObject> spawnedRooms;
    public List<GameObject> spawnedPortals;
    private GameObject startSP;

    private static GameStatus gameStatus;
    public static GameStatus GameStatus { get => gameStatus; }

    public static GameController instance;
    

    private void Awake()
    {
        instance = this;
        spawnedRooms = new();
        spawnedPortals = new();
        gameStatus = GameStatus.PREPARING;
    }

    private void OnEnable()
    {
        GameEvents.instance.OnLevelSettingsSet += Respawn;
        GameEvents.instance.OnStartLevel += SetGameStatusON;
        GameEvents.instance.OnLoadNextLevel += SetGameStatusPreparing;
        GameEvents.instance.OnRestartFloor += SetGameStatusPreparing;
        GameEvents.instance.OnPauseGame += SetGameStatusPaused;
        GameEvents.instance.OnLastRoomReady += CheckRoomsAmount;
    }

    private void Start()
    {
        GameEvents.instance.ChangeGameStatus(GameStatus.START);
        GameEvents.instance.LoadLevel();
    }

    private void CheckRoomsAmount()
    {
        if(spawnedRooms.Count == RoomsGenerator.instance.RoomsToGenerate)
        {
            GameEvents.instance.GeneratingReady();
        }
        else
        {
            Respawn();
        }
    }

    private void SetGameStatusON()
    {
        gameStatus = GameStatus.ON;
    }

    private void SetGameStatusPreparing()
    {
        gameStatus = GameStatus.PREPARING;
    }

    private void SetGameStatusPaused()
    {
        gameStatus = GameStatus.PAUSED;
    }

    public void Respawn()
    {
        RoomsGenerator.instance.ResetRooms();
        MapManager.instance.ClearMap();
        DestroyOldPortals();
        DestoryOldRooms();
        StartCoroutine(CreatStartSpawnPoint());
    }


    public IEnumerator CreatStartSpawnPoint()
    {
        yield return new WaitForSeconds(1f);
        startSP = Instantiate(startSpawnPoint);
    }

    private void DestroyOldPortals()
    {
        foreach (var item in spawnedPortals)
        {
            Destroy(item);
        }
    }
    private void DestoryOldRooms()
    {
        Destroy(startSP);
        foreach (var item in spawnedRooms)
        {
            Destroy(item);
        }

        spawnedRooms.Clear();
    }

    
    private void OnDisable()
    {
        GameEvents.instance.OnLevelSettingsSet -= Respawn;
        GameEvents.instance.OnStartLevel -= SetGameStatusON;
        GameEvents.instance.OnLoadNextLevel -= SetGameStatusPreparing;
        GameEvents.instance.OnRestartFloor -= SetGameStatusPreparing;
        GameEvents.instance.OnPauseGame -= SetGameStatusPaused;
        GameEvents.instance.OnLastRoomReady -= CheckRoomsAmount;
    }
}
