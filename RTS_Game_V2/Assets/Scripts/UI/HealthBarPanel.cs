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
    private float sliderValue;
    private void OnEnable()
    {
        GameEvents.instance.OnUpdateCurrentHP += UpdateCurrentHP;
        GameEvents.instance.OnStatisticUpdate += UpdateMaxHP;
    }

    void Update()
    {
        slider.value = Mathf.Clamp01(slider.value);
    }

    private void UpdateMaxHP(StatisticType statisticType,float value)
    {
        if(statisticType == StatisticType.MaxHealth)
        {
            Debug.Log("UPDATE MAX");
            maxHP = value;
            //UpdateCurrentHP(currentHP);
            maxHPTextObject.text = Mathf.RoundToInt(maxHP).ToString();
            slider.value = currentHP / maxHP;
        }

    }

    private void UpdateCurrentHP(float value)
    {
        Debug.Log("UPDATE CURRENT");
        currentHP = value;
        //currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentHPTextObject.text = Mathf.RoundToInt(currentHP).ToString();
        hpClamped = Mathf.Clamp(currentHP, 0, maxHP);
        sliderValue = hpClamped / maxHP;
        slider.value = hpClamped / maxHP;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnUpdateCurrentHP -= UpdateCurrentHP;
        GameEvents.instance.OnStatisticUpdate -= UpdateMaxHP;
    }
}
