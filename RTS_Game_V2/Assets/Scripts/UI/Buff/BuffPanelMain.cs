using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffPanelMain : MonoBehaviour
{
    [System.Serializable]
    public struct BuffsSettings
    {
        public StatisticType statisticType;
        public Sprite sprite;
    }

    [SerializeField] private GameObject panelPrefab;

    List<GameObject> leftPanels = new();

    [SerializeField] BuffsSettings[] buffsSettings;
    private Dictionary<StatisticType, Sprite> spriteDict;
    RectTransform mainPanelRect;
    private void OnEnable()
    {
        GameEvents.instance.OnBuffActivate += Buff;
        GameEvents.instance.OnBuffDeactivate += UpdateUI;
    }
    private void Awake()
    {
        //spriteDict = new();
        //foreach (var item in buffsSettings)
        //{
        //    spriteDict.Add(item.statisticType, item.sprite);
        //}
        mainPanelRect = GetComponent<RectTransform>();
    }
    

    private void Start()
    {
        spriteDict = new();
        foreach (var item in buffsSettings)
        {
            spriteDict.Add(item.statisticType, item.sprite);
        }
    }

    public void Buff(StatisticType statType, float statValue, float duration)
    {
        GameObject panel;
        if(CheckForPooledObject(out panel))
        { 
            BuffPanel buffPanel = panel.GetComponent<BuffPanel>();
            buffPanel.SetBuffPanel(statType, statValue, duration, spriteDict[statType]);
            panel.SetActive(true);
        }
        else
        {
            panel = Instantiate(panelPrefab);
            leftPanels.Add(panel);
            panel.transform.SetParent(this.transform);

            BuffPanel buffPanel = panel.GetComponent<BuffPanel>();
            buffPanel.SetBuffPanel(statType, statValue, duration, spriteDict[statType]);
            panel.SetActive(true);
        }

        UpdateUI(statType,statValue);
    }

    private void UpdateUI(StatisticType statType, float statValue)
    {
        int i = 0;
        foreach (var panel in leftPanels)
        {
            if (panel.activeSelf)
            {
                RectTransform rect = panel.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector3( mainPanelRect.sizeDelta.x /2 + (rect.sizeDelta.x + 10) * -i - rect.sizeDelta.x / 2, 0, 0);
                i++;
            }
        }
    }

    private bool CheckForPooledObject(out GameObject panel)
    {
        foreach (var item in leftPanels)
        {
            if (!item.activeInHierarchy)
            {
                panel = item;
                return true;
            }
        }
        panel = null;
        return false;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnBuffActivate -= Buff;
        GameEvents.instance.OnBuffDeactivate -= UpdateUI;
    }
}
