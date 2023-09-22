using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticSlotPanel : MonoBehaviour
{
    [SerializeField] private StatisticType statisticType;
    [SerializeField] private TMP_Text textObject;

    private void OnEnable()
    {

    }

    private void Start()
    {
        GameEvents.instance.OnStatisticUpdate += UpdateStatistic;
    }

    public void UpdateStatistic(StatisticType statisticType, float value)
    {
        if(this.statisticType == statisticType)
        {
            textObject.text = value.ToString();
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnStatisticUpdate -= UpdateStatistic;
    }
}
