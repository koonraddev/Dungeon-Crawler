using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticSlotPanel : MonoBehaviour
{
    [SerializeField] private StatisticType statisticType;
    [SerializeField] private TMP_Text statisticLabel, statisticValue;

    private void Awake()
    {
        switch (statisticType)
        {
            case StatisticType.MaxHealth:
                statisticLabel.text = "Max Health";
                break;
            case StatisticType.MovementSpeed:
                statisticLabel.text = "Movement Speed";
                break;
            case StatisticType.HealthPointsRegeneration:
                statisticLabel.text = "Health Points Regeneration";
                break;
            case StatisticType.HealthPercentageRegeneration:
                statisticLabel.text = "Health Percentage Regeneration";
                break;
            case StatisticType.Armor:
                statisticLabel.text = "Armor";
                break;
            case StatisticType.MagicResistance:
                statisticLabel.text = "Magic Resistance";
                break;
            case StatisticType.PhysicalDamage:
                statisticLabel.text = "Physical Damage";
                break;
            case StatisticType.MagicDamage:
                statisticLabel.text = "Magic Damage";
                break;
            case StatisticType.TrueDamage:
                statisticLabel.text = "True Damage";
                break;
            case StatisticType.AttackSpeed:
                statisticLabel.text = "Attack Speed";
                break;
            case StatisticType.AttackRange:
                statisticLabel.text = "Attack Range";
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStatistic;
    }

    public void UpdateStatistic(StatisticType statisticType, float value)
    {
        if(this.statisticType == statisticType)
        {
            statisticValue.text = value.ToString();
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticUpdate -= UpdateStatistic;
    }
}
