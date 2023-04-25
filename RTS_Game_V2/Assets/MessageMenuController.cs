using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[RequireComponent(typeof(Canvas))]
public class MessageMenuController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject objectReq;
    private Vector3 objectReqPosition;
    private Vector3 offset;
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform panelRect;

    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private RectTransform buttonPanelRect;
    private Vector2 buttonPanelSize;
    [SerializeField] private GameObject buttonYES;
    [SerializeField] private GameObject buttonNO;
    [SerializeField] private GameObject buttonOK;

    private Button bt;

    [SerializeField] private TMP_Text nameHolder;
    [SerializeField] private TMP_Text descripotionHolder;
    [SerializeField] private TMP_Text messageHolder;
    
    [SerializeField] private RectTransform nameHolderRect;
    [SerializeField] private RectTransform descripotionHoldeRrect;
    [SerializeField] private RectTransform messageHolderRect;

    private Vector2 nameHolderSize;
    private Vector2 descrHolderSize;
    private Vector2 messHolderSize;


    [SerializeField] private Vector3 messageMenuPosition;
    private PopupPanel popupPanel;
    private bool doFadeOutAndUnactive;
    private bool messageMenuPosAsObjectPos;
    private Vector2 panelSize;

    void Start()
    {
        bt = buttonYES.GetComponent<Button>();
        buttonPanelSize = buttonPanelRect.sizeDelta;
        nameHolderSize = nameHolderRect.sizeDelta;
        descrHolderSize = descripotionHoldeRrect.sizeDelta;
        messHolderSize = messageHolderRect.sizeDelta;

        doFadeOutAndUnactive = true;
        mainCamera = Camera.main;
        popupPanel = panel.GetComponent<PopupPanel>();
        panelRect = panel.GetComponent<RectTransform>();
        panelSize = panelRect.sizeDelta;

        ResetAllElements();
    }


    void Update()
    {
        if (messageMenuPosAsObjectPos)
        {
            CheckPointing();
        }
        else
        {
            //Vector3 pos = offset;
            //if (panel.transform.position != pos)
            //{
            //    panel.transform.position = pos;
            //}
        }
    }

    private void OnEnable()
    {
        doFadeOutAndUnactive = true;
    }

    public void PrepareMessageMenu(IInteractionObjects intObject, MessageType messageType)
    {
        bt = buttonYES.GetComponent<Button>();//
        switch (messageType)
        {
            case MessageType.POPUP:
                messageMenuPosAsObjectPos = true;
                break;
            case MessageType.OPEN:
                messageMenuPosAsObjectPos = false;
                bt.onClick.AddListener(intObject.DoInteraction);
                buttonYES.SetActive(true);
                buttonNO.SetActive(true);
                messageHolder.text += "Open?";
                break;
            case MessageType.TAKE:
                messageMenuPosAsObjectPos = false;
                bt.onClick.AddListener(intObject.DoInteraction);
                buttonYES.SetActive(true);
                buttonNO.SetActive(true);
                messageHolder.text += "Take it?";
                break;
            case MessageType.INFORMATION:
                messageMenuPosAsObjectPos = false;
                buttonOK.SetActive(true);
                break;
            default:
                messageMenuPosAsObjectPos = false;
                buttonOK.SetActive(true);
                break;
        }

        objectReq = intObject.GetGameObject();
        objectReqPosition = objectReq.transform.position;
        SetTextHolders(intObject);
        CheckTextHolders();
        CheckButtons();
    }

    private void SetTextHolders(IInteractionObjects intObject)
    {
        Dictionary<string, string> lista = intObject.GetContentToDisplay();
        if (lista.Count > 0)
        {
            foreach (KeyValuePair<string, string> li in lista)
            {
                switch (li.Key)
                {
                    case "Name":
                        nameHolder.text += li.Value;
                        break;
                    case "Description":
                        descripotionHolder.text += li.Value;
                        break;
                    case "Message":
                        messageHolder.text += li.Value;
                        break;
                    default:
                        messageHolder.text = "Error";
                        break;
                }
            }
        }
    }

    private void CheckTextHolders()
    {
        if(nameHolder.text != "")
        {
            nameHolder.gameObject.SetActive(true);
            panelRect.sizeDelta += new Vector2(0f, nameHolderSize.y);

            nameHolderRect.localPosition -= new Vector3(0f, nameHolderSize.y, 0f);
            descripotionHoldeRrect.localPosition -= new Vector3(0f, nameHolderSize.y, 0f);
            messageHolderRect.localPosition -= new Vector3(0f, nameHolderSize.y, 0f);
            buttonPanelRect.localPosition -= new Vector3(0f, nameHolderSize.y, 0f);
            offset += new Vector3(0, 80, 0);
        }
        if(descripotionHolder.text != "")
        {
            descripotionHolder.gameObject.SetActive(true);
            panelRect.sizeDelta += new Vector2(0f, descrHolderSize.y);

            descripotionHoldeRrect.localPosition -= new Vector3(0f, descrHolderSize.y, 0f);
            messageHolderRect.localPosition -= new Vector3(0f, descrHolderSize.y, 0f);
            buttonPanelRect.localPosition -= new Vector3(0f, descrHolderSize.y, 0f);
            //offset += new Vector3(0, 50, 0);
        }
        if(messageHolder.text != "")
        {
            messageHolder.gameObject.SetActive(true);
            panelRect.sizeDelta += new Vector2(0f, messHolderSize.y);

            messageHolderRect.localPosition -= new Vector3(0f, messHolderSize.y, 0f);
            buttonPanelRect.localPosition -= new Vector3(0f, messHolderSize.y, 0f);
            //offset += new Vector3(0, 50, 0);
        }
    }

    private void CheckButtons()
    {
        if(buttonYES.activeSelf || buttonNO.activeSelf || buttonOK.activeSelf)
        {
            panelRect.sizeDelta += new Vector2(0f, buttonPanelSize.y);
            buttonPanelRect.localPosition -= new Vector3(0f, buttonPanelSize.y, 0f);
        }
    }
    private void CheckPointing()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Positioning();
            if ((hit.transform.gameObject == objectReq) && (objectReq != null))
            {
                doFadeOutAndUnactive = true;
            }
            else
            {
                if (doFadeOutAndUnactive)
                {
                    StartCoroutine(FadeOutAndUnactive());
                    doFadeOutAndUnactive = false;
                }
            }
        }
    }

    private void OnDisable()
    {
        Debug.Log("On disablee");
        ResetAllElements();
    }

    private void ResetAllElements()
    {
        nameHolder.text = "";
        descripotionHolder.text = "";
        messageHolder.text = "";

        nameHolder.gameObject.SetActive(false);
        descripotionHolder.gameObject.SetActive(false);
        messageHolder.gameObject.SetActive(false);

        buttonYES.SetActive(false);
        buttonNO.SetActive(false);
        buttonOK.SetActive(false);

        panelRect.sizeDelta = new Vector2(panelSize.x, 0);
        nameHolderRect.localPosition = Vector3.zero;
        descripotionHoldeRrect.localPosition = Vector3.zero;
        messageHolderRect.localPosition = Vector3.zero;
        buttonPanelRect.localPosition = Vector3.zero;

        offset = Vector3.zero;
        bt.onClick.RemoveAllListeners();
    }

    private void Positioning()
    {

        Vector3 pos = (mainCamera.WorldToScreenPoint(objectReqPosition) + offset);
        if (panel.transform.position != pos)
        {
            panel.transform.position = pos;
        }
    }

    private IEnumerator FadeOutAndUnactive()
    {
        if (popupPanel != null)
        {
            yield return StartCoroutine(popupPanel.FadeOut());
        }
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
