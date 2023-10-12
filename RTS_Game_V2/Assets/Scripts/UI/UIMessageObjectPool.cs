using System.Collections.Generic;
using UnityEngine;
public class UIMessageObjectPool : MonoBehaviour
{
    public static UIMessageObjectPool instance;
    private List<GameObject> pooledObjects;
    public GameObject MessageObjectToPool { get; set; }
    public int AmountToPool { get; set; }
    public bool canAddObjects { get; set; }
    public bool FollowMouse { get; set; }
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
        CreateObjects(AmountToPool);   
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
        if (MessageObjectToPool.GetComponent<MessageMenuController>() != null)
        {
            GameObject tmp;
            if (amountToCreate > 0 && MessageObjectToPool != null)
            {
                for (int i = 0; i < amountToCreate; i++)
                {
                    tmp = Instantiate(MessageObjectToPool);
                    tmp.GetComponent<MessageMenuController>().FollowMouse = FollowMouse;
                    tmp.SetActive(false);
                    pooledObjects.Add(tmp);
                }
            }
        }
    }

}
