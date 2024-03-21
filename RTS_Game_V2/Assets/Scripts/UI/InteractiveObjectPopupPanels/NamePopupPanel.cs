using TMPro;
using UnityEngine;

public class NamePopupPanel : PopupPanel
{
    [SerializeField] private TMP_Text nameHolder;
    public override PopupType MessageType { get => PopupType.NAME; }

    public override void PrepareMessageMenu(ObjectContent popup)
    {
        base.PrepareMessageMenu(popup);
        nameHolder.text = "";

        if (popup.Nametext == "") gameObject.SetActive(false);

        nameHolder.text = popup.Nametext;
    }
}
