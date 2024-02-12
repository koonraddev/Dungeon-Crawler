using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class RoomTeleport : MonoBehaviour
{
    [SerializeField] private List<Renderer> portals;
    [SerializeField] private List<GameObject> particles;
    [SerializeField] private List<Light> lights;
    [SerializeField] private float activePortalValue, unactivePortalValue, activePortalLightIntensity, unactivePortalLightIntensity;
    [SerializeField] private Color activePortalColor, unactivePortalColor;


    private void Start()
    {
        ActiveParticles(false);

    }
    public void ActiveTeleport(bool active)
    {
        float newValue = active ? activePortalValue : unactivePortalValue;
        float newIntensity = active ? activePortalLightIntensity : unactivePortalLightIntensity;
        Color newColor = active ? activePortalColor : unactivePortalColor;
        foreach (var item in portals)
        {
            Sequence seq = DOTween.Sequence()
                .Append(item.material.DOFloat(newValue, "_Power", 2f))
                .Join(item.material.DOColor(newColor, "_Color", 2f));
            seq.Play();
        }

        foreach (var item in lights)
        {
            Sequence seq = DOTween.Sequence()
                .Append(item.DOColor(newColor, 2f))
                .Join(item.DOIntensity(newIntensity, 2f));

            seq.Play();
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
