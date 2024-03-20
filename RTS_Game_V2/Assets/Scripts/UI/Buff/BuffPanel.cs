using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BuffPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image buffImage, backgroundImage;
    [SerializeField] private Color buffColor, debuffColor;
    private float timeLeft;
    private StatisticType statType;
    private float statValue;

    [SerializeField] private GameObject infoObject;
    private BuffPopup buffPopup;

    private void OnEnable()
    {
        GameEvents.instance.OnBuffDeactivate += Deactivate;
    }

    private void Deactivate(StatisticType statType, float statValue)
    {
        if((this.statType == statType) && (this.statValue == statValue))
        {
            if(buffPopup != null)
            {
                buffPopup.gameObject.SetActive(false);
                Destroy(buffPopup.gameObject);
            }
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
            if(buffPopup == null)
            {
                GameObject popup = Instantiate(infoObject);
                buffPopup = popup.GetComponent<BuffPopup>();
            }
            else
            {
                buffPopup.gameObject.SetActive(true);
            }
            buffPopup.SetPopup(statType, statValue, timeLeft, gameObject, transform.parent.gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(buffPopup != null)
        {
            buffPopup.gameObject.SetActive(false);
        }

    }

    public void SetBuffPanel(StatisticType statType, float statValue, float duration, Sprite buffSprite)
    {
        timeLeft = duration;
        this.statType = statType;
        this.statValue = statValue;

        buffImage.sprite = buffSprite;

        if (statValue > 0)
        {
            backgroundImage.color = buffColor;

        }
        else
        {
            backgroundImage.color = debuffColor;
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnBuffDeactivate -= Deactivate;
    }
}
