using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;
using System.Reflection;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText, infoText, extraInfoText;
    [SerializeField] private Image textureHolder;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] List<ItemStatisticPanel> statsPanelsList;
    [SerializeField] float scrollSpeed, scrollCoolDown;

    private Sprite emptyPanelSprite;
    bool enableScroll, scrollDown;
    private float panelsAmount, maxScrollValue, coolDownLeft;

    private void Awake()
    {
        emptyPanelSprite = textureHolder.sprite;
        nameText.text = "";
        infoText.text = "";
        coolDownLeft = scrollCoolDown;

        foreach (var item in statsPanelsList)
        {
            item.SetEmpty();
            item.gameObject.SetActive(false);

        }

        panelsAmount = statsPanelsList.Count - 6;
    }

    private void Update()
    {

        if (enableScroll)
        {
            coolDownLeft -= Time.deltaTime;
            if (coolDownLeft <= 0)
            {
                if (scrollDown)
                {
                    scrollRect.verticalNormalizedPosition -= (Time.deltaTime / 2);
                    if (scrollRect.verticalNormalizedPosition <= maxScrollValue)
                    {
                        scrollDown = false;
                        coolDownLeft = scrollCoolDown;
                    }
                }
                else
                {
                    scrollRect.verticalNormalizedPosition += (Time.deltaTime / 2);
                    if (scrollRect.verticalNormalizedPosition >= 1)
                    {
                        scrollDown = true;
                        coolDownLeft = scrollCoolDown;
                    }
                }
            }

            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
        }
    }


   


    public void SetInfoPanel(Item item)
    {
        DisplayBasics(item.Name, item.Description, item.Sprite);
        extraInfoText.text = "";
        switch (item)
        {
            case EquipmentItem eqItem:
                DisplayStatistics(eqItem.Statistics);
                EquipmentSlotType eqSlotType = eqItem.ItemSlot;


                switch (eqSlotType)
                {
                    case EquipmentSlotType.HEAD:
                        extraInfoText.text = "HEAD";
                        break;
                    case EquipmentSlotType.CHEST:
                        extraInfoText.text = "CHEST";
                        break;
                    case EquipmentSlotType.LEGS:
                        extraInfoText.text = "LEGS";
                        break;
                    case EquipmentSlotType.FEETS:
                        extraInfoText.text = "FEETS";
                        break;
                    case EquipmentSlotType.LEFT_ARM:
                        extraInfoText.text = "LEFT ARM";
                        break;
                    case EquipmentSlotType.RIGHT_ARM:
                        extraInfoText.text = "RIGHT ARM";
                        break;
                    default:
                        break;
                }

                if (eqItem.IsWeapon)
                {
                    extraInfoText.text += ", WEAPON";
                }
                break;
            case UsableItem usItem:
                DisplayStatistics(usItem.Statistics);
                extraInfoText.text = "CONSUMABLE";
                break;
            case PassiveItem paItem:
                if (paItem.MultipleUse)
                {
                    extraInfoText.text = "PERMAMENT";
                }               
                break;
            default:
                break;
        }

    }



    public void DisplayBasics(string itemName, string itemDescription, Sprite itemSprite)
    {
        nameText.text = itemName;
        textureHolder.sprite = itemSprite;
        infoText.text = itemDescription;
    }

    //public void Exampe<T>(T comp) where T : Item
    //{
    //    PropertyInfo[] properties = typeof(T).GetProperties();
    //    foreach (PropertyInfo property in properties)
    //    {
    //        try
    //        {
    //            Debug.Log(property.GetValue(comp));
    //        }
    //        catch (Exception ex)
    //        {
    //            continue
    //        }
    //    }
    //}


    public void DisplayStatistics(Dictionary<StatisticType, float> statistics)
    {
        var notZero = statistics.Where(a => a.Value != 0);
        int iterator = 0;
        foreach (KeyValuePair<StatisticType, float> pair in notZero)
        {
            ItemStatisticPanel panel = statsPanelsList[iterator];
            panel.gameObject.SetActive(true);
            panel.SetStatisticPanel(pair.Key, pair.Value);
            iterator++;
        }
        int enabledAmount = iterator;
        if (enabledAmount > 6)
        {
            enableScroll = true;
            int tmp = enabledAmount - 6;
            maxScrollValue = 1 - tmp / panelsAmount;
        }
        else
        {
            enableScroll = false;
        }
        scrollRect.verticalNormalizedPosition = 1;
        coolDownLeft = scrollCoolDown;
        scrollDown = true;
    }



    public void SetEmpty()
    {
        nameText.text = "";
        infoText.text = "";
        textureHolder.sprite = emptyPanelSprite;
        extraInfoText.text = "";
        foreach (var item in statsPanelsList)
        {
            item.SetEmpty();
            item.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        SetEmpty();
    }
}
