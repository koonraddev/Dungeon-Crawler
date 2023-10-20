using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;



public class BuffPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image buffImage;
    private RectTransform rectTrans;
    private float timeLeft;
    private StatisticType statType;
    private float statValue;

    [SerializeField] private GameObject infoObject;
    private TMP_Text textObject;
    private GameObject tmpObj;
    private void Awake()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();

    }

    private void OnEnable()
    {
        GameEvents.instance.OnBuffDeactivate += Deactivate;
    }

    private void Deactivate(StatisticType statType, float statValue)
    {
        if((this.statType == statType) && (this.statValue == statValue))
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(infoObject != null)
        {
            tmpObj = Instantiate(infoObject);
            RectTransform rect = tmpObj.GetComponent<RectTransform>();
            //CanvasGroup canvGroup = tmpObj.GetComponent<CanvasGroup>();
            //canvGroup.alpha = 0.8f;
            //canvGroup.blocksRaycasts = false;

            tmpObj.transform.SetParent(GameObject.Find("BuffPanelMain").transform);
            rect.transform.localPosition = rectTrans.localPosition - new Vector3(0,rectTrans.rect.height,0);
            BuffPopup popup = tmpObj.GetComponent<BuffPopup>();
            popup.SetPopup(statType,statValue, timeLeft);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(tmpObj);
    }

    public void SetBuffPanel(StatisticType statType, float statValue, float duration, Sprite buffSprite)
    {
        timeLeft = duration;
        this.statType = statType;
        this.statValue = statValue;

        buffImage.sprite = buffSprite;

        if (statValue > 0)
        {
            buffImage.color = Color.green;

        }
        else
        {
            buffImage.color = Color.red;
        }

    }

    private void OnDisable()
    {
        GameEvents.instance.OnBuffDeactivate -= Deactivate;
    }
}
