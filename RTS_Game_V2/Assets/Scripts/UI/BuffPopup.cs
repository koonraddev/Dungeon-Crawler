using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    float timeLeft;
    string popupInfo;

    RectTransform popupRect, parentRect, buffRect;
    Vector2 parentPos, newPos;
    float minX, maxX;
    float newX, newY;
    float sizeX;

    bool setted = false;
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            Destroy(gameObject);
        }
        textObject.text = popupInfo + Mathf.Floor(timeLeft).ToString();

        if (setted)
        {
            parentPos = buffRect.anchoredPosition;
            newX = Mathf.Clamp(parentPos.x, minX, maxX);
            newY = -parentRect.sizeDelta.y * 2;
            newPos = new(newX, newY);
            popupRect.anchoredPosition = newPos;
        }
    }

    public void SetPopup(StatisticType statType, float statValue ,float timeLeft, GameObject buffPanel, GameObject parentObject)
    {
        popupInfo = "";
        this.timeLeft = timeLeft;

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

        if (statValue > 0)
        {
            popupInfo += " increased: ";
        }
        else
        {
            popupInfo += " decreased: ";
        }

        popupInfo += statValue.ToString();
        popupInfo += "\nTime left: ";

        gameObject.transform.SetParent(parentObject.transform);
        parentRect = parentObject.GetComponent<RectTransform>();
        popupRect = GetComponent<RectTransform>();
        buffRect = buffPanel.GetComponent<RectTransform>();

        sizeX = parentRect.sizeDelta.x;

        minX = -sizeX / 2 + popupRect.sizeDelta.x/2;
        maxX = sizeX/2 - popupRect.sizeDelta.x/2;

        parentPos = buffRect.anchoredPosition;
        newX = Mathf.Clamp(parentPos.x, minX, maxX);
        newY = -parentRect.sizeDelta.y * 2;
        newPos = new(newX, newY);
        popupRect.anchoredPosition = newPos;

        setted = true;
    }
}
