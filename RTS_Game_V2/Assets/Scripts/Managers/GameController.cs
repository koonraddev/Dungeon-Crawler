using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject startSpawnPoint;
    [SerializeField] public static List<GameObject> spawnedRooms;
    private GameObject startSP;
    public enum GameStatus
    {
        START,
        RUN,
        PAUSE,
        END
    }

    private void Awake()
    {
        spawnedRooms = new();
    }

    private void OnEnable()
    {
        GameEvents.instance.OnLoadLevel += Respawn;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLoadLevel -= Respawn;
    }

    public void Respawn()
    {
        DestoryOldRooms();
        StartCoroutine(CreatStartSpawnPoint());
    }


    private void Start()
    {
        GameEvents.instance.ChangeGameStatus(GameStatus.START);

        StartCoroutine(CreatStartSpawnPoint());
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

}
