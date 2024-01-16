using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeScript : MonoBehaviour
{
    [SerializeField] private Volume volumeGrayEffect;
    private void OnEnable()
    {
        GameEvents.instance.OnGameOver += GrayFilterON;
        GameEvents.instance.OnStartLevel += GrayFilterOFF;
    }

    private void GrayFilterON()
    {
        volumeGrayEffect.enabled = true;
    }

    private void GrayFilterOFF()
    {
        volumeGrayEffect.enabled = false;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnGameOver -= GrayFilterON;
        GameEvents.instance.OnStartLevel -= GrayFilterOFF;
    }
}
