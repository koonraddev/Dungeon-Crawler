using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private GameObject nameObject;
    [SerializeField] private GameObject infoObject;
    [SerializeField] private Image textureHolder;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] List<GameObject> statsObjectList;
    [SerializeField] float scrollSpeed;
    [SerializeField] float coolDown;
    private TMP_Text nameText;
    private TMP_Text infoText;

    private Sprite infoPanelSprite;
    bool enableScroll = true;
    private float am;

    float maxScrollValue;
    int enabledAmount;
    private float coolDownLeft;
    private bool scrollDown = true;
    private void Awake()
    {
        nameText = nameObject.GetComponent<TMP_Text>();
        infoText = infoObject.GetComponent<TMP_Text>();
        infoPanelSprite = textureHolder.sprite;
        nameText.text = "";
        infoText.text = "";
        coolDownLeft = coolDown;

        foreach (var item in statsObjectList)
        {
            item.SetActive(false);
            TMP_Text label = item.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            TMP_Text value = item.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

            label.text = "";
            value.text = "";
        }

        am = statsObjectList.Count - 6;
    }

    private void Update()
    {

        if (enableScroll)
        {
            coolDownLeft -= Time.deltaTime;
            if (coolDownLeft <= 0)
            {
                if (scrollDown)
                {
                    scrollRect.verticalNormalizedPosition -= (Time.deltaTime / 2);
                    if (scrollRect.verticalNormalizedPosition <= maxScrollValue)
                    {
                        scrollDown = false;
                        coolDownLeft = coolDown;
                    }
                }
                else
                {
                    scrollRect.verticalNormalizedPosition += (Time.deltaTime / 2);
                    if (scrollRect.verticalNormalizedPosition >= 1)
                    {
                        scrollDown = true;
                        coolDownLeft = coolDown;
                    }
                }
            }

            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
        }
    }

    public void SetInfoPanel(string nameContent, string infoContent,Sprite spriteContent, Dictionary<StatisticType, float> stats = null)
    {
        textureHolder.sprite = spriteContent;
        nameText.text = nameContent;
        infoText.text = infoContent;

        if (stats != null)
        {
            int iterator = 0;
            foreach (KeyValuePair<StatisticType, float> pair in stats)
            {
                if (pair.Value > 0)
                {
                    GameObject oneStatObj = statsObjectList[iterator];
                    TMP_Text label = oneStatObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
                    TMP_Text value = oneStatObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

                    switch (pair.Key)
                    {
                        case StatisticType.MaxHealth:
                            label.text = "Max Health";
                            break;
                        case StatisticType.MovementSpeed:
                            label.text = "Movement Speed";
                            break;
                        case StatisticType.HealthPointsRegeneration:
                            label.text = "Health Points Regeneration";
                            break;
                        case StatisticType.HealthPercentageRegeneration:
                            label.text = "Health Percentage Regeneration";
                            break;
                        case StatisticType.Armor:
                            label.text = "Armor";
                            break;
                        case StatisticType.MagicResistance:
                            label.text = "Magic Resistance";
                            break;
                        case StatisticType.PhysicalDamage:
                            label.text = "Physical Damage";
                            break;
                        case StatisticType.MagicDamage:
                            label.text = "Magic Damage";
                            break;
                        case StatisticType.TrueDamage:
                            label.text = "True Damage";
                            break;
                        case StatisticType.AttackSpeed:
                            label.text = "Attack Speed";
                            break;
                        case StatisticType.AttackRange:
                            label.text = "Attack Range";
                            break;
                        default:
                            break;
                    }

                    value.text = pair.Value.ToString();
                    oneStatObj.SetActive(true);

                    iterator++;
                }
            }
            enabledAmount = iterator;

            if (enabledAmount > 6)
            {
                enableScroll = true;
                int tmp = enabledAmount - 6;
                maxScrollValue = 1 - tmp / am;
            }
            else
            {
                enableScroll = false;
            }
            scrollRect.verticalNormalizedPosition = 1;
            coolDownLeft = coolDown;
            scrollDown = true;
        }
    }

    public void SetEmpty()
    {
        nameText.text = "";
        infoText.text = "";
        textureHolder.sprite = infoPanelSprite;

        foreach (var item in statsObjectList)
        {
            item.SetActive(false);
            TMP_Text label = item.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            TMP_Text value = item.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

            label.text = "";
            value.text = "";
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
