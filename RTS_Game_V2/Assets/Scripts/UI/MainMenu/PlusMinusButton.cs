using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlusMinusButton : MonoBehaviour
{
    [SerializeField] private NewPlayerPanel newPlayerPanel;
    [SerializeField] private StatisticType statisticType;
    private Image buttonImage;
    private Button button;
    [SerializeField] private bool positiveButton;
    private int requirePoints;
    private Color inactiveButtonColor, activeButtonColor;
    StatisticCreator ss;
    private float baseValue, pointsLeft;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        inactiveButtonColor = newPlayerPanel.InactiveButtonColor;
        activeButtonColor = newPlayerPanel.ActiveButtonColor;
        ss = newPlayerPanel.StatsList.Where(x => x.StatisticType == statisticType).ToList()[0];
        requirePoints = ss.RequirePoints;
        baseValue = ss.BaseStatisticValue;
    }


    public void PlusButton()
    {
        newPlayerPanel.AddPointToStatistic(statisticType);
    }
    
    public void MinusButton()
    {
        newPlayerPanel.DeletePointFromStatistic(statisticType);
    }

    private void Update()
    {
        if (positiveButton)
        {
            if(newPlayerPanel.PointsLeft > 0 &&  newPlayerPanel.PointsLeft >= requirePoints)
            {
                buttonImage.color = activeButtonColor;
                button.enabled = true;
            }
            else
            {

                buttonImage.color = inactiveButtonColor;
                button.enabled = false;
            }
        }
        else
        {
            if(ss.AddedValue != 0)
            {
                buttonImage.color = activeButtonColor;
                button.enabled = true;
            }
            else
            {
                buttonImage.color = inactiveButtonColor;
                button.enabled = false;
            }
        }

    }
}
