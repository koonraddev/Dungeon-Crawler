using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanelButton : MonoBehaviour
{
    private bool mapFullSizeMode;
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
    }

    private void OnDisable()
    {
        GameEvents.instance.OnMapPanel -= FullMapSize;
    }
}
