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
    private static List<GameObject> spawnedRooms;
    private static List<GameObject> spawnedPortals;
    private GameObject startSP;

    private static GameStatus gameStatus;
    public static GameStatus GameStatus { get => gameStatus; }

    public static List<GameObject> SpawnedRooms { get => spawnedRooms; }
    public static List<GameObject> SpawnedPortals { get => spawnedPortals; }

    private void Awake()
    {
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
        GameEvents.instance.OnResumeGame += SetGameStatusON;
    }

    public static void AddRoom(GameObject room)
    {
        spawnedRooms.Add(room);
    }

    public static void AddPortal(GameObject portal)
    {
        spawnedPortals.Add(portal);
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
        Time.timeScale = 1f;
    }

    private void SetGameStatusPreparing()
    {
        gameStatus = GameStatus.PREPARING;
        Time.timeScale = 1f;
    }

    private void SetGameStatusPaused()
    {
        gameStatus = GameStatus.PAUSED;
        Time.timeScale = 0f;
    }
    private void Respawn()
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
        GameEvents.instance.OnResumeGame -= SetGameStatusON;
    }
}
