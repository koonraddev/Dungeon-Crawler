using UnityEngine;
using UnityEngine.UI;

public class StatisticPanelButton : MonoBehaviour
{
    [SerializeField] private Color panelOnColor, panelOFFColor;
    private bool statisticPanelStatus;
    [SerializeField] private Image statusIcon;
    private void OnEnable()
    {
        GameEvents.instance.OnStatisticPanel += PanelStatus;
    }

    public void StatisticsButtonClick()
    {
        GameEvents.instance.StatisticPanel(!statisticPanelStatus);
    }

    private void PanelStatus(bool status)
    {
        statisticPanelStatus = status;
        statusIcon.color = statisticPanelStatus ? panelOnColor : panelOFFColor;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticPanel -= PanelStatus;
    }
}
