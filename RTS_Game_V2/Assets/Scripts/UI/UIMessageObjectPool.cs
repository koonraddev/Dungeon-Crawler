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
    public GameObject GetPooledObject()
    {
        int objectsInList = pooledObjects.Count;
        for (int i = 0; i < objectsInList; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public void DisplayMessage(IInteractionObject gameObjectReq, MessageType messageType )
    {
        if (GetPooledObject() == null)
        {
            if (canAddObjects)
            {
                CreateObjects(1);
                GameObject gm = GetPooledObject();
                MessageMenuController cc = gm.GetComponentInChildren<MessageMenuController>();
                cc.PrepareMessageMenu(gameObjectReq, messageType);
                gm.SetActive(true);
            }
        }
        else
        {
            GameObject gm = GetPooledObject();
            MessageMenuController cc = gm.GetComponentInChildren<MessageMenuController>();
            cc.PrepareMessageMenu(gameObjectReq, messageType);
            gm.SetActive(true);
        }
    }

    public void DisplayMessage(ISpecialInventoryPanel specialInventoryPanel, MessageType messageType)
    {
        if (GetPooledObject() == null)
        {
            if (canAddObjects)
            {
                CreateObjects(1);
                GameObject gm = GetPooledObject();
                MessageMenuController cc = gm.GetComponentInChildren<MessageMenuController>();
                cc.PrepareMessageMenu(specialInventoryPanel, messageType);
                gm.SetActive(true);
            }
        }
        else
        {
            GameObject gm = GetPooledObject();
            MessageMenuController cc = gm.GetComponentInChildren<MessageMenuController>();
            cc.PrepareMessageMenu(specialInventoryPanel, messageType);
            gm.SetActive(true);
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
