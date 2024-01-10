using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private GameObject nameObject;
    [SerializeField] private GameObject infoObject;
    [SerializeField] private Image textureHolder;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] List<ItemStatisticPanel> statsPanelsList;
    [SerializeField] float scrollSpeed;
    [SerializeField] float coolDown;
    private TMP_Text nameText;
    private TMP_Text infoText;

    private Sprite emptyPanelSprite;
    bool enableScroll = true;
    private float am;

    float maxScrollValue;
    int enabledAmount;
    private float coolDownLeft;
    private bool scrollDown = true;
    private void Awake()
    {
        nameText = nameObject.GetComponent<TMP_Text>();
        infoText = infoObject.GetComponent<TMP_Text>();
        emptyPanelSprite = textureHolder.sprite;
        nameText.text = "";
        infoText.text = "";
        coolDownLeft = coolDown;

        foreach (var item in statsPanelsList)
        {
            item.SetEmpty();
            item.gameObject.SetActive(false);

        }

        am = statsPanelsList.Count - 6;
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
                        coolDownLeft = coolDown;
                    }
                }
                else
                {
                    scrollRect.verticalNormalizedPosition += (Time.deltaTime / 2);
                    if (scrollRect.verticalNormalizedPosition >= 1)
                    {
                        scrollDown = true;
                        coolDownLeft = coolDown;
                    }
                }
            }

            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
        }
    }

    public void SetInfoPanel(string nameContent, string infoContent,Sprite spriteContent, Dictionary<StatisticType, float> stats = null)
    {
        textureHolder.sprite = spriteContent;
        nameText.text = nameContent;
        infoText.text = infoContent;

        if (stats != null)
        {
            var notZero = stats.Where(a => a.Value != 0);
            int iterator = 0;
            foreach (KeyValuePair<StatisticType, float> pair in notZero)
            {

                ItemStatisticPanel panel = statsPanelsList[iterator];
                panel.gameObject.SetActive(true);
                panel.SetStatisticPanel(pair.Key, pair.Value);
                iterator++;
                
            }
            enabledAmount = iterator;

            if (enabledAmount > 6)
            {
                enableScroll = true;
                int tmp = enabledAmount - 6;
                maxScrollValue = 1 - tmp / am;
            }
            else
            {
                enableScroll = false;
            }
            scrollRect.verticalNormalizedPosition = 1;
            coolDownLeft = coolDown;
            scrollDown = true;
        }
    }

    public void SetEmpty()
    {
        nameText.text = "";
        infoText.text = "";
        textureHolder.sprite = emptyPanelSprite;

        foreach (var item in statsPanelsList)
        {
            item.SetEmpty();
            item.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
