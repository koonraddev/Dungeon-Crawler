using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarPanel : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text currentHPTextObject;
    [SerializeField] private TMP_Text maxHPTextObject;
    float hpClamped;
    private float currentHP;
    private float maxHP;

    private void OnEnable()
    {
        GameEvents.instance.OnUpdateCurrentHP += UpdateCurrentHP;
        GameEvents.instance.OnStatisticUpdate += UpdateMaxHP;
    }


    private void UpdateMaxHP(StatisticType statisticType,float value)
    {
        if(statisticType == StatisticType.MaxHealth)
        {
            maxHP = value;
            maxHPTextObject.text = Mathf.RoundToInt(maxHP).ToString();
            slider.value = currentHP / maxHP;
        }

    }

    private void UpdateCurrentHP(float value)
    {
        currentHP = value;
        currentHPTextObject.text = Mathf.RoundToInt(currentHP).ToString();
        hpClamped = Mathf.Clamp(currentHP, 0, maxHP);
        slider.value = hpClamped / maxHP;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnUpdateCurrentHP -= UpdateCurrentHP;
        GameEvents.instance.OnStatisticUpdate -= UpdateMaxHP;
    }
}
