using TMPro;
using UnityEngine;

public class ItemStatisticPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text statisticLabel, statisticValue;

    public void SetStatisticPanel(StatisticType statisticType, float statisticValue)
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

        this.statisticValue.text = statisticValue.ToString();
    }

    public void SetEmpty()
    {
        statisticLabel.text = "";
        statisticValue.text = "";
    }  
}
