using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomTeleport : MonoBehaviour
{
    [SerializeField] private List<Renderer> portals;
    [SerializeField] private List<GameObject> particles;
    [SerializeField] private float activePortalValue, unactivePortalValue;
    [SerializeField] private Color activePortalColor, unactivePortalColor;


    private void Start()
    {
        ActiveParticles(false);

    }
    public void ActiveTeleport(bool active)
    {
        float newValue = active ? activePortalValue : unactivePortalValue;
        Color newColor = active ? activePortalColor : unactivePortalColor;
        foreach (var item in portals)
        {
            item.material.SetFloat("_Power", newValue);
            item.material.SetColor("_Color", newColor);
        }
    }

    public void ActiveParticles(bool active)
    {
        foreach (var item in particles)
        {
            item.SetActive(active);
        }
    }
}
