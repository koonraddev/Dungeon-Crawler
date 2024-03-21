using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] CameraMovingObject camMovObj;
    GameObject gameObj;
    private void OnEnable()
    {
        GameEvents.instance.OnGeneratingReady += SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        if(gameObj == null)
        {
            gameObj = Instantiate(playerObject);
            if (camMovObj != null)
            {
                camMovObj.PlayerCharacter = gameObj;
            }
            GameEvents.instance.PlayerSpawn();
        }
        else
        {
            gameObj.SetActive(false);
            gameObj.transform.position = Vector3.zero;
            gameObj.SetActive(true);

            camMovObj.SetPositionToTarget();
        }
    }


    private void OnDisable()
    {
        GameEvents.instance.OnGeneratingReady -= SpawnPlayer;
    }
}
