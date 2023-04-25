using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class PopupPanel : MonoBehaviour
{
    [SerializeField] List<TMP_Text> textHolderList;
    [SerializeField] private Color panelColor;
    [SerializeField] private Color textColor;
    [SerializeField] private float fadingTime;

    private Color panelColorTransparent;
    private Color textColorTransparent;
    private Image panel;

    private void OnEnable()
    {
        panel = GetComponent<Image>();
        FadeInInstant();
    }
    private void Start()
    {
        panelColorTransparent = panelColor;
        panelColorTransparent.a = 0f;
        textColorTransparent = textColor;
        textColorTransparent.a = 0f;
        AddTextHolders(); 
    }

    private void AddTextHolders()
    {
        TMP_Text[] textHolders = GetComponentsInChildren<TMP_Text>();
        
        foreach(TMP_Text textH in textHolders)
        {
            textHolderList.Add(textH);
        }
    }

    private void FadeInInstant()
    {
        panel.color = panelColor;
        foreach (TMP_Text textHolder in textHolderList)
        {
            textHolder.color = textColor;
        }
    }
    public IEnumerator FadeOut()
    {
        Sequence fadeOutSeq = DOTween.Sequence();
        fadeOutSeq.Append(panel.DOColor(panelColorTransparent, fadingTime)).SetEase(Ease.Linear);
        foreach (TMP_Text textHolder in textHolderList)
        {
            fadeOutSeq.Join(textHolder.DOColor(textColorTransparent, fadingTime)).SetEase(Ease.Linear);
        }

        fadeOutSeq.WaitForCompletion();
        yield return fadeOutSeq.WaitForCompletion();
        fadeOutSeq.Kill();
        StopAllCoroutines();
    } 
}
