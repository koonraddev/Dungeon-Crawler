using UnityEngine;
using UnityEngine.UI;

public class MapPanelButton : MonoBehaviour
{
    [SerializeField] private Color panelOnColor, panelOFFColor;
    private bool mapFullSizeMode;
    [SerializeField] private Image statusIcon;
    private void OnEnable()
    {
        GameEvents.instance.OnMapPanel += FullMapSize;
    }

    public void MapButtonClick()
    {
        GameEvents.instance.MapPanel(!mapFullSizeMode);
    }

    private void FullMapSize(bool status)
    {
        mapFullSizeMode = status;
        statusIcon.color = mapFullSizeMode ? panelOnColor : panelOFFColor;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnMapPanel -= FullMapSize;
    }
}
