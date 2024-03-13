using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class RoomTeleport : MonoBehaviour
{
    [SerializeField] private List<Renderer> portals;
    [SerializeField] private List<GameObject> particles;
    [SerializeField] private List<Light> lights;
    [SerializeField] private float activePortalValue, unactivePortalValue, activePortalLightIntensity, unactivePortalLightIntensity;
    [SerializeField] [ColorUsage(true,true)] private Color activePortalColor, unactivePortalColor;


    private void Start()
    {
        ActiveParticles(false);

    }
    public void ActiveTeleport(bool active)
    {
        //float newValue = active ? activePortalValue : unactivePortalValue;
        float newIntensity = active ? activePortalLightIntensity : unactivePortalLightIntensity;
        Color newColor = active ? activePortalColor : unactivePortalColor;

        Sequence seq = DOTween.Sequence();
        foreach (var item in portals)
        {
            seq.Join(item.material.DOColor(newColor, "_Color", 2f));

        }
        seq.Play();

        Sequence seq2 = DOTween.Sequence();
        foreach (var item in lights)
        {
            seq2.Join(item.DOIntensity(newIntensity, 2f));
        }
        seq2.Play();
    }

    public void ActiveParticles(bool active)
    {
        foreach (var item in particles)
        {
            item.SetActive(active);
        }
    }
}
