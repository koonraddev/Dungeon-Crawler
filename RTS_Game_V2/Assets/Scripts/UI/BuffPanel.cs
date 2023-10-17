using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct BuffsSettings
{
    public StatisticType statisticType;
    public Sprite sprite;
}

public class BuffPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image buffImage;
    private RectTransform rectTrans;
    private float timeLeft;
    private StatisticType statType;
    private float statValue;

    [SerializeField] private GameObject infoObject;
    private TMP_Text textObject;
    private GameObject tmpObj;

    [SerializeField] BuffsSettings[] buffsSettings;
    private Dictionary<StatisticType, Sprite> spriteDict;


    private string popupInfo;
    private void Awake()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();

        spriteDict = new();
        foreach (var item in buffsSettings)
        {
            spriteDict.Add(item.statisticType, item.sprite);
        }
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if(timeLeft <= 0)
        {
            BuffManager.instance.Debuff(statType, statValue);
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(infoObject != null)
        {
            tmpObj = Instantiate(infoObject);
            RectTransform rect = tmpObj.GetComponent<RectTransform>();
            CanvasGroup canvGroup = tmpObj.GetComponent<CanvasGroup>();
            canvGroup.alpha = 0.8f;
            canvGroup.blocksRaycasts = false;

            tmpObj.transform.SetParent(GameObject.Find("BuffPanelMain").transform);
            rect.transform.localPosition = rectTrans.localPosition - new Vector3(0,rectTrans.rect.height,0);
            textObject = tmpObj.GetComponentInChildren<TMP_Text>();
            textObject.text = popupInfo;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(tmpObj);
    }

    public void SetBuffPanel(StatisticType statType, float statValue, float duration)
    {
        popupInfo = "";
        timeLeft = duration;
        this.statType = statType;
        this.statValue = statValue;
        switch (statType)
        {
            case StatisticType.MaxHealth:
                popupInfo = "Max Health";
                break;
            case StatisticType.MovementSpeed:
                popupInfo = "Movement Speed";
                break;
            case StatisticType.HealthPointsRegeneration:
                popupInfo = "Health Points Regeneration";
                break;
            case StatisticType.HealthPercentageRegeneration:
                popupInfo = "Health Percents Regeneration";
                break;
            case StatisticType.Armor:
                popupInfo = "Armor";
                break;
            case StatisticType.MagicResistance:
                popupInfo = "Magic Resistance";
                break;
            case StatisticType.PhysicalDamage:
                popupInfo = "Physical Damage";
                break;
            case StatisticType.MagicDamage:
                popupInfo = "Magic Damage";
                break;
            case StatisticType.TrueDamage:
                popupInfo = "True Damage";
                break;
            case StatisticType.AttackSpeed:
                popupInfo = "Attack Speed";
                break;
            case StatisticType.AttackRange:
                popupInfo = "Attack Range";
                break;
            default:
                break;
        }

        buffImage.sprite = spriteDict[statType];

        if (statValue > 0)
        {
            buffImage.color = Color.green;
            popupInfo += " increased: ";
        }
        else
        {
            buffImage.color = Color.red;
            popupInfo += " decreased: ";
        }

        popupInfo += statValue.ToString();
    }
}
