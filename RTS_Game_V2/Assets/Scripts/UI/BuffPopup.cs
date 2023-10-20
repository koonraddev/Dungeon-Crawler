using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    float timeLeft;
    string popupInfo;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            Destroy(gameObject);
        }
        textObject.text = popupInfo + Mathf.Floor(timeLeft).ToString();
    }

    public void SetPopup(StatisticType statType, float statValue ,float timeLeft)
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

    }
}
