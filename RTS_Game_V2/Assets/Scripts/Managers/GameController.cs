using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("UI Message")]
    [Tooltip("Message Prefab")]
    [SerializeField] private GameObject objectToPool;
    [Tooltip("Number of prefab to pool")]
    [SerializeField] private int amountToPool;
    [Tooltip("Specify whether the Object Pooler can create extra objects at runtime when there is a need for them")]
    [SerializeField] private bool canAddObjectsToPool;
    [Tooltip("Specify whether the Message Menu should follow mouse when on top of a object")]
    [SerializeField] private bool followMouse;

    [Header("Player Section")]
    [Tooltip("Player Object")]
    [SerializeField] private GameObject playerObject;

    [Space(15)]
    [SerializeField] public NavMeshSurface surface;
    [SerializeField] CameraMovingObject camMovObj;

    [Space(15)]
    [SerializeField] private RoomsGenerator roomsGenerator;
    [SerializeField] private RoomsSetSO roomsSet;
    [SerializeField] private GameObject startSpawnPoint;

    public int level;
    public enum GameStatus
    {
        START,
        RUN,
        PAUSE,
        END
    }

    void Awake()
    {
        if (!FindObjectOfType(typeof(UIMessageObjectPool)))
        {
            GameObject uiObjectPooler = new GameObject("UIObjectpooler", typeof(UIMessageObjectPool));
            UIMessageObjectPool.instance.AmountToPool = amountToPool;
            UIMessageObjectPool.instance.MessageObjectToPool = objectToPool;
            UIMessageObjectPool.instance.canAddObjects = canAddObjectsToPool;
            UIMessageObjectPool.instance.FollowMouse = followMouse;
        }

        roomsGenerator.SetRoomsGenerator(roomsSet);


    }

    private void OnEnable()
    {
        GameEvents.instance.OnLoadLevel += LoadLevel;
    }


    public void LoadLevel()
    {

    }

    private void Start()
    {
        GameEvents.instance.ChangeGameStatus(GameStatus.START);
        //if (!FindObjectOfType(typeof(Inventory)))
        //{
        //    GameObject invenotry = new GameObject("Inventory", typeof(Inventory));
        //    Inventory.Instance.InventorySO = inventorySO;
        //}

        //if (!FindObjectOfType(typeof(Equipment)))
        //{
        //    GameObject invenotry = new GameObject("Equipment", typeof(Equipment));
        //    Equipment.Instance.EquipmentSO = equipmentSO;
        //}

        GameEvents.instance.OnLastRoomReady += LastRoomReady;

        StartCoroutine(CreatStartSpawnPoint());
    }

    private void Update()
    {

    }

    public IEnumerator CreatStartSpawnPoint()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(startSpawnPoint);
    }

    private void UpdateNavMesh()
    {
        Debug.Log("UPDATE siatki");
        surface.BuildNavMesh();

    }

    private void SpawnPlayer()
    {
        GameObject gameObj = Instantiate(playerObject);
        if(camMovObj != null)
        {
            camMovObj.playerCharacter = gameObj;
        }
    }


    private void LastRoomReady()
    {
        //UpdateNavMesh();
        SpawnPlayer();
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLastRoomReady -= LastRoomReady;
        GameEvents.instance.OnLoadLevel -= LoadLevel;
    }

}
