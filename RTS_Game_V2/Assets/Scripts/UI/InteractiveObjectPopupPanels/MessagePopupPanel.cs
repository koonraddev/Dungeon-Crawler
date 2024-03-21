using TMPro;
using UnityEngine;

public class MessagePopupPanel : PopupPanel
{
    [SerializeField] private TMP_Text messageHolder;
    public override PopupType MessageType { get => PopupType.INFORMATION; }
    public override void PrepareMessageMenu(ObjectContent popup)
    {
        base.PrepareMessageMenu(popup);
        messageHolder.text = "";

        if (popup.Message == "") gameObject.SetActive(false);

        messageHolder.text = popup.Message;
    }
}
