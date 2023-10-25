using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject startSpawnPoint;
    public enum GameStatus
    {
        START,
        RUN,
        PAUSE,
        END
    }


    private void Start()
    {
        GameEvents.instance.ChangeGameStatus(GameStatus.START);

        StartCoroutine(CreatStartSpawnPoint());
    }

    public IEnumerator CreatStartSpawnPoint()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(startSpawnPoint);
    }

}
