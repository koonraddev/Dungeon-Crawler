using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] CameraMovingObject camMovObj;
    private void OnEnable()
    {
        GameEvents.instance.OnLastRoomReady += SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        GameObject gameObj = Instantiate(playerObject);
        if (camMovObj != null)
        {
            camMovObj.playerCharacter = gameObj;
        }
    }


    private void OnDisable()
    {
        GameEvents.instance.OnLastRoomReady -= SpawnPlayer;
    }
}
