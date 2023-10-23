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
        if(GetPooledObject() == null)
        {
            panel = Instantiate(panelPrefab);

            leftPanels.Add(panel);
            BuffPanel buffPanel = panel.GetComponent<BuffPanel>();
            panel.transform.SetParent(this.transform);
            buffPanel.SetBuffPanel(statType, statValue, duration, spriteDict[statType]);
            panel.SetActive(true);
        }
        else
        {
            panel = GetPooledObject();
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
            if (panel.activeInHierarchy)
            {
                RectTransform rect = panel.GetComponent<RectTransform>();
                rect.transform.localPosition = new Vector3((rect.rect.width + 10) * -i , 0, 0);
                i++;
            }
        }
    }

    private GameObject GetPooledObject()
    {
        foreach (var item in leftPanels)
        {
            if (!item.activeInHierarchy)
            {
                return item;
            }
        }
        return null;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnBuffActivate -= Buff;
        GameEvents.instance.OnBuffDeactivate -= UpdateUI;
    }
}
