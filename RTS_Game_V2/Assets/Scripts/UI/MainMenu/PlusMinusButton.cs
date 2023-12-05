using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlusMinusButton : MonoBehaviour
{
    [SerializeField] private NewCharacterPanel newPlayerPanel;
    [SerializeField] private StatisticType statisticType;

    [SerializeField] private bool positiveButton;
    [SerializeField] private ButtonManager buttonManager;
    StatisticCreator statCreator;
    private void Awake()
    {

        statCreator = newPlayerPanel.StatsList.Where(x => x.StatisticType == statisticType).ToList()[0];
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
            if(newPlayerPanel.PointsLeft > 0 &&  newPlayerPanel.PointsLeft >= statCreator.RequirePoints)
            {
                buttonManager.ActivateButton();
            }
            else
            {
                buttonManager.DeactivateButton();
            }
        }
        else
        {
            if(statCreator.AddedValue != 0)
            {
                buttonManager.ActivateButton();
            }
            else
            {
                buttonManager.DeactivateButton();
            }
        }

    }
}
