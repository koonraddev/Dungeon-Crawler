using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPanelMain : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab;

    List<GameObject> leftPanels = new();

    private void OnEnable()
    {
        GameEvents.instance.onBuffActivate += Buff;
        GameEvents.instance.onBuffDeactivate += UpdateUI;
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
            buffPanel.SetBuffPanel(statType, statValue, duration);
            panel.SetActive(true);
        }
        else
        {
            panel = GetPooledObject();
            BuffPanel buffPanel = panel.GetComponent<BuffPanel>();
            buffPanel.SetBuffPanel(statType, statValue, duration);
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
        GameEvents.instance.onBuffActivate -= Buff;
        GameEvents.instance.onBuffDeactivate -= UpdateUI;
    }
}
