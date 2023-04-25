using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIObjectPool : MonoBehaviour
{
    public static UIObjectPool instance;
    private List<GameObject> pooledObjects;
    public GameObject ObjectToPool { get; set; }
    public int AmountToPool { get; set; }
    public bool canAddObjects { get; set; }
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

    public void DisplayMessage(IInteractionObjects gameObjectReq, MessageType messageType )
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

    private void CreateObjects(int amountToCreate)
    {
        if (ObjectToPool.GetComponent<MessageMenuController>() != null)
        {
            GameObject tmp;
            if (amountToCreate > 0 && ObjectToPool != null)
            {
                for (int i = 0; i < amountToCreate; i++)
                {
                    tmp = Instantiate(ObjectToPool);
                    tmp.SetActive(false);
                    pooledObjects.Add(tmp);
                }
            }
        }

    }
}
