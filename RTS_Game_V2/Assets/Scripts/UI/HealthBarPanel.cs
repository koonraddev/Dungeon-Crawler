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

    private float currentHP;
    private float maxHP;

    private void OnEnable()
    {
        GameEvents.instance.OnUpdateCurrentHP += UpdateCurrentHP;
        GameEvents.instance.OnStatisticUpdate += UpdateMaxHP;
    }

    void Update()
    {
        slider.value = Mathf.Clamp(slider.value, 0, 1);
    }

    private void UpdateMaxHP(StatisticType statisticType,float value)
    {
        if(statisticType == StatisticType.MaxHealth)
        {
            maxHP = value;
            UpdateCurrentHP(currentHP);
            maxHPTextObject.text = Mathf.RoundToInt(maxHP).ToString();
            slider.value = currentHP / maxHP;
        }

    }

    private void UpdateCurrentHP(float value)
    {
        currentHP = value;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentHPTextObject.text = Mathf.RoundToInt(currentHP).ToString();
        slider.value = currentHP / maxHP;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnUpdateCurrentHP -= UpdateCurrentHP;
        GameEvents.instance.OnStatisticUpdate -= UpdateMaxHP;
    }
}
