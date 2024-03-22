using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyInformationPanel : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text currentHPTextObject;
    [SerializeField] private TMP_Text maxHPTextObject;
    [SerializeField] private TMP_Text nameTextObject;
    [SerializeField] private Image imageObject;

    private float currentHP;
    private float maxHP;

    void Update()
    {
        if(enemy != null)
        {
            currentHP = enemy.Health;
            slider.value = currentHP / maxHP;
            currentHPTextObject.text = Mathf.RoundToInt(currentHP).ToString();
        }

        slider.value = Mathf.Clamp(slider.value, 0, 1);
    }

    public void SetPanel(Enemy enemyScript)
    {
        enemy = enemyScript;
        currentHP = enemy.Health;
        maxHP = enemy.MaxHealth;

        currentHPTextObject.text = Mathf.RoundToInt(currentHP).ToString();
        maxHPTextObject.text = Mathf.RoundToInt(maxHP).ToString();
        nameTextObject.text = enemy.Name;

        if(enemy.Sprite != null)
        {
            imageObject.sprite = enemy.Sprite;
            imageObject.DOFade(1, 0f).SetAutoKill(true).Play();
        }
        else
        {
            imageObject.DOFade(0, 0f).SetAutoKill(true).Play();
        }
    }
}
