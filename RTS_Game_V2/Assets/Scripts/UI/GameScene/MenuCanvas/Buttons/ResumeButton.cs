using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public void ResumeButtonClick()
    {
        GameEvents.instance.MenuPanel(false);
    }
}
