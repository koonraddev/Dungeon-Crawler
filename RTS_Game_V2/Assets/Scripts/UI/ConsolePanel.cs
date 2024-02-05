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
    [SerializeField] private float fadingDelay;
    private Color originalPanelColor;
    private Color originalTextColor;
    private Image mainPanel;
    private bool consoleIsOn = true;

    private PlayerControls playerControls;
    private InputAction switchConsoleStatus;

    private Tween t1, t2;
    private void Awake()
    {
        playerControls = new PlayerControls();
        switchConsoleStatus = playerControls.BasicMovement.ConsoleLogs;
        instance = this;
        mainPanel = GetComponent<Image>();
        originalPanelColor = mainPanel.color;
        originalTextColor = consoleText.color;

        t1 = mainPanel.DOColor(transparentColor, 2f).SetDelay(fadingDelay);
        t2 = consoleText.DOColor(transparentColor, 2f).SetDelay(fadingDelay);
        CheckConsole();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        GameEvents.instance.OnConsolePanel += ConsoleStatus;
    }

    private void ConsoleStatus(bool activePanel)
    {
        consoleIsOn = activePanel;
    }

    void Update()
    {
        if (switchConsoleStatus.triggered)
        {
            GameEvents.instance.ConsolePanel(!consoleIsOn);
            CheckConsole();
        }

        scrollbar.value = 0;
    }

    private void CheckConsole()
    {
        if (consoleIsOn)
        {
            if (t1.IsPlaying())
            {
                t1.Rewind();
                t2.Rewind();
            }

            consoleText.color = originalTextColor;
            mainPanel.color = originalPanelColor;
            t1.Play();
            t2.Play();
        }
    }

    public void EnemyTakeDamage(string enemyName, float damage)
    {
        consoleText.text += "An " + enemyName + " <color=#5ef0f7>loses</color> " + damage + " hitpoints due to your attack\n";
        CheckConsole();  
    }

    public void PlayerTakeDamage(string enemyName, float damage)
    {
        consoleText.text += "You <color=#f55649>lose</color> " + damage + " hitpoints due to an attack by " + enemyName + "\n";
        CheckConsole();
    }


    public void HealLog(float healValue)
    {
        consoleText.text += "You <color=#96f75e>healed</color> " + healValue + " health points\n";
        CheckConsole();
    }

    public void UseLog(string objectName)
    {
        consoleText.text += "Using " + objectName + "\n";
        CheckConsole();
    }

    public void InfoLog(string info)
    {
        consoleText.text += info + "\n";
        CheckConsole();
    }
    private void OnDisable()
    {
        GameEvents.instance.OnConsolePanel -= ConsoleStatus;
    }
}
