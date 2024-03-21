using System.Collections.Generic;
using UnityEngine;

public abstract class PopupPanel : MonoBehaviour
{
    public abstract PopupType MessageType { get; }
    private GameObject objectReq;
    public int GetId { get => objectReq.GetInstanceID(); }
    private bool relativePosition;
    private Vector3 objectReqPosition, relativeOffset;
    private RectTransform panelRect;

    private Vector3 diff;
    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();

        switch (MessageType)
        {
            case PopupType.NAME:
                relativePosition = true;
                break;
            default:
                relativePosition = false;
                break;
        }
    }
    public virtual void PrepareMessageMenu(ObjectContent popup)
    {
        objectReq = popup.RequestingGameObject;
        if (objectReq == null)
        {
            gameObject.SetActive(false);
        }


        if (MessageType == PopupType.NAME)
        {
            objectReqPosition = objectReq.transform.position;
            relativeOffset = new(0f, panelRect.sizeDelta.y, 0f);
            diff = Input.mousePosition - Camera.main.WorldToScreenPoint(objectReqPosition);
        }
    }

    public virtual void PrepareMessageMenu(Dictionary<string, string> contentDictionary, GameObject objectReq)
    {
        this.objectReq = objectReq;
        objectReqPosition = objectReq.transform.position;
        relativeOffset = new(0f,panelRect.sizeDelta.y,0f);
        diff = Input.mousePosition - Camera.main.WorldToScreenPoint(objectReqPosition);
    }

    private void OnEnable()
    {
        GameEvents.instance.OnCloseMessage += DeactivateMessageMenu;
    }


    private void Update()
    {
        if (objectReq == null || !objectReq.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }


        if (relativePosition)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(objectReqPosition) + diff + relativeOffset;
            if (gameObject.transform.position != pos)
            {
                gameObject.transform.position = pos;
            }

        }
        else
        {
            gameObject.transform.localPosition = Vector3.zero;
        }

    }

    private void DeactivateMessageMenu(int id)
    {
        if (id == objectReq.GetInstanceID() && MessageType == PopupType.NAME)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnCloseMessage -= DeactivateMessageMenu;
    }


}
