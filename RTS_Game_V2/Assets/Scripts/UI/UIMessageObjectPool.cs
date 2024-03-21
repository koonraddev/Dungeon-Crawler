using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMessageObjectPool : MonoBehaviour
{
    public static UIMessageObjectPool instance;
    private List<PopupPanel> pooledObjects;
    [SerializeField] GameObject poolPanel;
    [SerializeField] List<PopupPanel> popupPrefabsList;

    void Awake()
    {
        instance = this;
        pooledObjects = new List<PopupPanel>();
    }

    private bool CheckForEmptyObject(PopupType popupType ,out PopupPanel pooledObject)
    {
        pooledObject = null;
        foreach (var item in pooledObjects)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                if(item.MessageType == popupType)
                {
                    pooledObject = item;
                    return true;
                }
            }
        }
        return false;
    }

    public void DisplayMessage(ObjectContent content, PopupType popupType)
    {
        if (CheckForEmptyObject(popupType, out PopupPanel popupPanel))
        {
            popupPanel.PrepareMessageMenu(content);
            popupPanel.gameObject.SetActive(true);
        }
        else
        {
            if (CreateObject(popupType))
            {
                DisplayMessage(content, popupType);
            }
        }
    }

    private bool CreateObject(PopupType popupType)
    {
        if(popupPrefabsList.Any((x) => x.MessageType == popupType))
        {
            PopupPanel prefab = popupPrefabsList.First((x) => x.MessageType == popupType);

            if (prefab != null)
            {
                PopupPanel tmp = Instantiate(prefab);
                tmp.gameObject.SetActive(false);
                pooledObjects.Add(tmp);
                tmp.transform.SetParent(poolPanel.transform);
                return true;
            }
        }

        return false;
    }

}
