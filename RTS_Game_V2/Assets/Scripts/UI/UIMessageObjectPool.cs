using System.Collections.Generic;
using UnityEngine;


public class UIMessageObjectPool : MonoBehaviour
{
    public static UIMessageObjectPool instance;
    private List<GameObject> pooledObjects;
    [Tooltip("Message Prefab")]
    [SerializeField] private GameObject objectToPool;
    [Tooltip("Number of prefab to pool")]
    [SerializeField] private int amountToPool;
    [Tooltip("Specify whether the Object Pooler can create extra objects at runtime when there is a need for them")]
    [SerializeField] private bool canAddObjects;
    [Tooltip("Specify whether the Message Menu should follow mouse when on top of a object")]
    [SerializeField] private bool followMouse;

    public enum MessageType
    {
        POPUP,
        INFORMATION,
        TAKE,
        OPEN,
        DELETE,
        DROP
    }
    void Awake()
    {
        instance = this;
        pooledObjects = new List<GameObject>();
    }

    void Start()
    {
        CreateObjects(amountToPool);   
    }
    private bool CheckForEmptyObject(out GameObject pooledObject)
    {
        pooledObject = null;
        foreach (var item in pooledObjects)
        {
            if (!item.activeInHierarchy)
            {
                pooledObject = item;
                return true;
            }
        }
        return false;
    }

    public void DisplayMessage(IInteractionObject gameObjectReq, MessageType messageType )
    {
        if (!CheckForExistingMessage(gameObjectReq,messageType))
        {
            GameObject gm;
            if (CheckForEmptyObject(out GameObject messageObject))
            {
                gm = messageObject;
                MessageMenuController cc = gm.GetComponentInChildren<MessageMenuController>();
                cc.PrepareMessageMenu(gameObjectReq, messageType);
                gm.SetActive(true);
            }
            else
            {
                if (canAddObjects)
                {
                    CreateObjects(1);
                    DisplayMessage(gameObjectReq, messageType);
                }
            }
        }
    }

    private bool CheckForExistingMessage(IInteractionObject gameObjectReq, MessageType messageType)
    {
        foreach (var item in pooledObjects)
        {
            if (item.activeInHierarchy)
            {
                MessageMenuController messageMenu = item.GetComponentInChildren<MessageMenuController>();

                switch (messageType)
                {
                    case MessageType.POPUP:
                        if (gameObjectReq.GameObject.GetInstanceID() == messageMenu.GetId)
                        {
                            return true;
                        }
                        break;
                    default:
                        if (gameObjectReq.GameObject.GetInstanceID() == messageMenu.GetId && messageType == messageMenu.MessageType)
                        {
                            return true;
                        }
                        break;
                }
            }
        }
        return false;
    }

    public void DisplayMessage(ISpecialInventoryPanel specialInventoryPanel, MessageType messageType)
    {
        GameObject gm;
        if (CheckForEmptyObject(out GameObject messageObject))
        {
            gm = messageObject;
            MessageMenuController cc = gm.GetComponentInChildren<MessageMenuController>();
            cc.PrepareMessageMenu(specialInventoryPanel, messageType);
            gm.SetActive(true);
        }
        else
        {
            if (canAddObjects)
            {
                CreateObjects(1);
                DisplayMessage(specialInventoryPanel, messageType);
            }
        }
    }

    private void CreateObjects(int amountToCreate)
    {
        if (objectToPool.GetComponent<MessageMenuController>() != null)
        {
            GameObject tmp;
            if (amountToCreate > 0 && objectToPool != null)
            {
                for (int i = 0; i < amountToCreate; i++)
                {
                    tmp = Instantiate(objectToPool);
                    tmp.GetComponent<MessageMenuController>().FollowMouse = followMouse;
                    tmp.SetActive(false);
                    pooledObjects.Add(tmp);
                }
            }
        }
    }

}
