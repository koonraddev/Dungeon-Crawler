using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;


public class ConsolePanel : MonoBehaviour
{
    public static ConsolePanel instance;

    [SerializeField] private Color transparentColor;
    [SerializeField] private TMP_Text consoleText;
    [SerializeField] private Scrollbar scrollbar;
    private Color originalPanelColor;
    private Color originalTextColor;
    private Image mainPanel;
    private bool consoleIsOn = true;

    private PlayerControls playerControls;
    private InputAction switchConsoleStatus;

    private Tween t1;
    private Tween t2;
    private void Awake()
    {
        playerControls = new PlayerControls();
        switchConsoleStatus = playerControls.BasicMovement.ConsoleLogs;
        instance = this;
        mainPanel = GetComponent<Image>();
        originalPanelColor = mainPanel.color;
        originalTextColor = consoleText.color;

        t1 = mainPanel.DOColor(transparentColor, 2f);
        t2 = consoleText.DOColor(transparentColor, 2f);
        t1.Play();
        t2.Play();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ConsoleStatus(bool activePanel)
    {
        consoleIsOn = activePanel;
    }

    void Update()
    {
        if (switchConsoleStatus.triggered)
        {
            ConsoleStatus(!consoleIsOn);
            StartCoroutine(FadeOut());
        }

        scrollbar.value = 0;
    }

    private bool CheckConsole()
    {
        if (consoleIsOn)
        {
            t1.Kill();
            t2.Kill();

            consoleText.color = originalTextColor;
            mainPanel.color = originalPanelColor;
            return true;
        }
        return false;
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("czekam");
        yield return new WaitForSeconds(5f);
        Debug.Log("zaczynam");
        t1 = mainPanel.DOColor(transparentColor, 2f);
        t2 = consoleText.DOColor(transparentColor, 2f);
        t1.Play();
        t2.Play();
    }

    public void AttackLog(string enemyName, float damage)
    {
        consoleText.text += "An " + enemyName + " <color=#5ef0f7>loses</color> " + damage + " hitpoints due to your attack\n";
        if (CheckConsole())
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }   
    }

    public void DamageLog(string enemyName, float damage)
    {
        consoleText.text += "You <color=#f55649>lose</color> " + damage + " hitpoints due to an attack by " + enemyName + "\n";
        if (CheckConsole())
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }


    public void HealLog(float healValue)
    {
        consoleText.text += "You <color=#96f75e>healed</color> " + healValue + " health points\n";
        if (CheckConsole())
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    public void UseLog(string objectName)
    {
        consoleText.text += "Using " + objectName + "\n";
        if (CheckConsole())
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    public void InfoLog(string info)
    {
        consoleText.text += info + "\n";
        if (CheckConsole())
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }
}
