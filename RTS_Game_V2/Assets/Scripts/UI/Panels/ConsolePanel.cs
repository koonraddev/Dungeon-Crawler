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

    [SerializeField] private TMP_Text consoleText;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private float fadingDelay, fadingTime;
    [SerializeField] private List<Graphic> objectsToFade;
    private bool consoleIsOn = true;

    private PlayerControls playerControls;
    private InputAction switchConsoleStatus;

    private Sequence seq;
    Vector3 startPostion;
    private void Awake()
    {
        playerControls = new PlayerControls();
        switchConsoleStatus = playerControls.BasicMovement.ConsoleLogs;
        instance = this;
        startPostion = gameObject.transform.position;
        seq = DOTween.Sequence();

       

        for (int i = 0; i < objectsToFade.Count; i++)
        { 
            seq.Join(objectsToFade[i].DOFade(0f, fadingTime));

            if (i == objectsToFade.Count - 1)
            {
                seq.Append(objectsToFade[i].DOFade(0f, fadingTime)).SetDelay(fadingDelay);
            }
            
        }

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
            //consoleText.DOFade(1f,0f);
            //mainPanel.DOFade(1f, 0f);
            gameObject.transform.position = startPostion;
            seq.Restart();
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
