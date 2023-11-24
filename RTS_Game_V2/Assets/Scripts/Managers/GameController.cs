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
    [SerializeField] public static List<GameObject> spawnedRooms;
    private GameObject startSP;

    private static GameStatus gameStatus;
    public static GameStatus GameStatus { get => gameStatus; }

    private void Awake()
    {
        spawnedRooms = new();
        gameStatus = GameStatus.PREPARING;
    }

    private void OnEnable()
    {
        GameEvents.instance.OnLevelSettingsSet += Respawn;
        GameEvents.instance.OnStartLevel += SetGameStatusON;
        GameEvents.instance.OnLoadNextLevel += SetGameStatusPreparing;
        GameEvents.instance.OnPauseGame += SetGameStatusPaused;
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
        DestoryOldRooms();
        StartCoroutine(CreatStartSpawnPoint());
    }


    private void Start()
    {
        GameEvents.instance.ChangeGameStatus(GameStatus.START);
        GameEvents.instance.LoadLevel();
    }

    public IEnumerator CreatStartSpawnPoint()
    {
        yield return new WaitForSeconds(1f);
        startSP = Instantiate(startSpawnPoint);
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
        GameEvents.instance.OnLevelSettingsSet += Respawn;
        GameEvents.instance.OnStartLevel -= SetGameStatusON;
        GameEvents.instance.OnLoadNextLevel -= SetGameStatusPreparing;
        GameEvents.instance.OnPauseGame -= SetGameStatusPaused;
    }

}
