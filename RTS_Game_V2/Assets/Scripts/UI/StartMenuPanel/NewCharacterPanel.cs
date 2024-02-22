using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class StatisticCreator
{
    [SerializeField] private StatisticType statisticType;
    [SerializeField] private float baseStatValue, valueIncrease;
    [SerializeField] private int requirePoints;
    private float addedValue;

    public StatisticType StatisticType { get => statisticType; }
    public int RequirePoints { get => requirePoints; }
    public float BaseStatisticValue { get => baseStatValue; }
    public float ValueIncrease { get => valueIncrease; }
    public float AddedValue { get => addedValue; set => addedValue = value; }

    public void Reset()
    {
        addedValue = 0;
    }

    public void IncreaseValue()
    {
        addedValue += valueIncrease;
    }

    public void DecreaseValue()
    {
        addedValue -= valueIncrease;
    }
}

public class NewCharacterPanel : MonoBehaviour
{
    [SerializeField] private Color inactiveButtonColor, activeButtonColor;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private List<StatisticCreator> statsList;
    [SerializeField] private int startPoints;
    private int points;
    private int selectedSlot;

    [SerializeField] private InputField nameInputField;
    [SerializeField] private ButtonManager nextButton;

    public Color InactiveButtonColor { get => inactiveButtonColor; }
    public Color ActiveButtonColor { get => activeButtonColor; }
    public int PointsLeft { get => points; }
    public List<StatisticCreator> StatsList { get => statsList; }
    public string CharacterName { get => nameInputField.text; }
    public int SelectedSlot { get => selectedSlot; set =>selectedSlot = value; }

    private void Awake()
    {
        points = startPoints;

        foreach (var item in statsList)
        {
            item.Reset();
            GameEvents.instance.StatisticUpdate(item.StatisticType, item.BaseStatisticValue + item.AddedValue);
        }
    }
    void Update()
    {
        pointsText.text = points.ToString();

        if (nameInputField.text.Length > 0)
        {
            nextButton.ActivateButton();
        }
        else
        {
            nextButton.DeactivateButton();
        }

    }

    public void AddPointToStatistic(StatisticType statisticType)
    {
        StatisticCreator statCreator = statsList.Where(x => x.StatisticType == statisticType).ToList()[0];

        if(points >= statCreator.RequirePoints)
        {
            points -= statCreator.RequirePoints;
            statCreator.IncreaseValue();
            GameEvents.instance.StatisticUpdate(statCreator.StatisticType, statCreator.BaseStatisticValue + statCreator.AddedValue);
        }
    }

    public void DeletePointFromStatistic(StatisticType statisticType)
    {
        StatisticCreator statCreator = statsList.Where(x => x.StatisticType == statisticType).ToList()[0];

        if (statCreator.AddedValue + statCreator.BaseStatisticValue - statCreator.ValueIncrease >= statCreator.BaseStatisticValue)
        {
            points += statCreator.RequirePoints;
            statCreator.DecreaseValue();
            GameEvents.instance.StatisticUpdate(statCreator.StatisticType, statCreator.BaseStatisticValue + statCreator.AddedValue);
        }
    }

    private void OnDisable()
    {
        selectedSlot = 0;
        points = startPoints;

        foreach (var item in statsList)
        {
            item.Reset();
            GameEvents.instance.StatisticUpdate(item.StatisticType, item.BaseStatisticValue + item.AddedValue);
        }
    }

    public void Create()
    {
        PlayerBasicStatistics playerStats = new(StatsList);
        SaveManager.instance.ChosenSlotIndex = selectedSlot;
        SaveManager.instance.CreateSave(selectedSlot, CharacterName, playerStats);
        //SceneLoader.instance.LoadDungeonScene();
        GameEvents.instance.LoadGameScene();
        GameEvents.instance.SwitchScene();
    }
}
