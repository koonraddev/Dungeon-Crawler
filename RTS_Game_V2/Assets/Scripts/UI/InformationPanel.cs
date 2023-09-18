using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private GameObject nameObject;
    [SerializeField] private GameObject infoObject;
    [SerializeField] private Image textureHolder;

    private TMP_Text nameText;
    private TMP_Text infoText;

    private Sprite infoPanelSprite;


    private void Awake()
    {
        nameText = nameObject.GetComponent<TMP_Text>();
        infoText = infoObject.GetComponent<TMP_Text>();
        infoPanelSprite = textureHolder.sprite;

        nameText.text = "";
        infoText.text = "";
    }

    public void SetInfoPanel(string nameContent, string infoContent)
    {
        textureHolder.sprite = infoPanelSprite;
        nameText.text = nameContent;
        infoText.text = infoContent;
    }

    public void SetEmpty()
    {
        nameText.text = "";
        infoText.text = "";
        textureHolder.sprite = null;
    }
}
