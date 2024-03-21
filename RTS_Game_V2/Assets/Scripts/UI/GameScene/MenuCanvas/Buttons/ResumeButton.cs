using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public void ResumeButtonClick()
    {
        GameEvents.instance.MenuPanel(false);
    }
}
