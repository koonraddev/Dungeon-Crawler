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

    [Header("UI Inventory")]
    [Tooltip("Number of item slots")]
    [SerializeField] private int itemSlots;

    public enum GameStatus
    {
        START,
        RUN,
        PAUSE,
        END
    }

    void Awake()
    {
        if (!FindObjectOfType(typeof(Inventory)))
        {
            GameObject invenotry = new GameObject("Inventory",typeof(Inventory));
            Inventory.Instance.InventorySize = itemSlots;
        }

        if (!FindObjectOfType(typeof(UIMessageObjectPool)))
        {
            GameObject uiObjectPooler = new GameObject("UIObjectpooler", typeof(UIMessageObjectPool));
            UIMessageObjectPool.instance.AmountToPool = amountToPool;
            UIMessageObjectPool.instance.ObjectToPool = objectToPool;
            UIMessageObjectPool.instance.canAddObjects = canAddObjectsToPool;
            UIMessageObjectPool.instance.FollowMouse = followMouse;
        }

    }

    private void Start()
    {
        GameEvents.instance.ChangeGameStatus(GameStatus.START);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameEvents.instance.PrepareGame();
        }
    }
}
