using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullPopupPanel : PopupPanel
{
    [SerializeField] private TMP_Text nameHolder, descriptionHolder, messageHolder;
    [SerializeField] private Button yesButton;
    [SerializeField] private FullPopupType popupType;

    private enum FullPopupType
    {
        DROP = PopupType.DROP,
        TAKE = PopupType.TAKE,
        OPEN = PopupType.OPEN,
        USE = PopupType.USE
    }

    public override PopupType MessageType { get => (PopupType)popupType; }

    public override void PrepareMessageMenu(ObjectContent popup)
    {
        base.PrepareMessageMenu(popup);
        nameHolder.text = "";
        descriptionHolder.text = "";

        nameHolder.text = popup.Nametext;
        descriptionHolder.text = popup.Description;
        messageHolder.text = popup.Message;

        switch (popupType)
        {
            case FullPopupType.TAKE:
                messageHolder.text = "Take ?";
                break;
            case FullPopupType.OPEN:
                messageHolder.text = "Open ?";
                    break;
            case FullPopupType.DROP:
                messageHolder.text = "Drop ?";
                break;
            case FullPopupType.USE:
                messageHolder.text = "Use ?";
                break;
            default:
                gameObject.SetActive(false);
                break;
        }

        if(popup.YesButtonDelegate == null) //not working
        {
            gameObject.SetActive(false);
        }

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(()=> popup.YesButtonDelegate?.Invoke());
        yesButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
