using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticPanelButton : MonoBehaviour
{
    private bool statisticPanelStatus;

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
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticPanel -= PanelStatus;
    }
}
