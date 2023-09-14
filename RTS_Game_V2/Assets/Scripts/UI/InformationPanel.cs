using UnityEngine;
using TMPro;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private GameObject nameObject;
    [SerializeField] private GameObject infoObject;

    private TMP_Text nameText;
    private TMP_Text infoText;


    private void Awake()
    {
        nameText = nameObject.GetComponent<TMP_Text>();
        infoText = infoObject.GetComponent<TMP_Text>();

        nameText.text = "";
        infoText.text = "";
    }

    public void SetInfoPanel(string nameContent, string infoContent)
    {
        nameText.text = nameContent;
        infoText.text = infoContent;
    }
}
