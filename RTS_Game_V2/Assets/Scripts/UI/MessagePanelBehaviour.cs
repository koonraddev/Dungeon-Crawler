using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[System.Serializable]
public class MessagePanelBehaviour : MonoBehaviour
{
    [HideInInspector] public bool fadeEffect;
    [HideInInspector] public float fadingTime;

    private List<TMP_Text> textHolderList = new();
    private Color panelColor;

    private Dictionary<TMP_Text, Color> originalColorsDict = new();
    private Dictionary<TMP_Text, Color> transparentColorsDict = new();

    private Color panelColorTransparent;
    private Image panel;

    private void Awake()
    {
        panel = GetComponent<Image>();
        GetTextHolders();
        SetOriginalColors();
        SetTransparentColors();
    }
    private void OnEnable()
    {
        FadeInInstant();
    }

    private void GetTextHolders()
    {
        TMP_Text[] textHolders = GetComponentsInChildren<TMP_Text>();
        
        foreach(TMP_Text textH in textHolders)
        {
            textHolderList.Add(textH);
        }
    }

    private void SetOriginalColors()
    {
        foreach (TMP_Text textH in textHolderList)
        {
            originalColorsDict.Add(textH, textH.color);
        }

        panelColor = panel.color;
    }

    private void SetTransparentColors()
    {
        foreach (TMP_Text textH in textHolderList)
        {
            Color transparentColor = textH.color;
            transparentColor.a = 0f;
            transparentColorsDict.Add(textH, transparentColor);
        }
        panelColorTransparent = panel.color;
        panelColorTransparent.a = 0f;
    }

    private void FadeInInstant()
    {
        panel.color = panelColor;
        foreach (TMP_Text textHolder in textHolderList)
        {
            textHolder.color = originalColorsDict[textHolder];
        }
    }
    public IEnumerator Deactivate()
    {
        Sequence fadeOutSeq = DOTween.Sequence();
        fadeOutSeq.Append(panel.DOColor(panelColorTransparent, fadingTime)).SetEase(Ease.Linear);
        foreach (TMP_Text textHolder in textHolderList)
        {
            fadeOutSeq.Join(textHolder.DOColor(transparentColorsDict[textHolder], fadingTime)).SetEase(Ease.Linear);
        }

        fadeOutSeq.WaitForCompletion();
        yield return fadeOutSeq.WaitForCompletion();
        fadeOutSeq.Kill();
        StopAllCoroutines();
    } 
}
