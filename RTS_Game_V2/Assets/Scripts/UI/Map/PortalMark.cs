using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PortalMark : MonoBehaviour
{
    [SerializeField] private Color activePortalColor, unactivePortalColor;
    [SerializeField] private Image foregroundMarkImage;

    public void SetPortal(bool locked)
    {
        if (locked)
        {
            DeactivatePortal();
        }
        else
        {
            ActivatePortal();
        }
        gameObject.SetActive(false);
    }

    public void ShowPortal()
    {
        gameObject.SetActive(true);
    }

    public void ActivatePortal()
    {
        foregroundMarkImage.color = activePortalColor;
    }

    public void DeactivatePortal()
    {
        foregroundMarkImage.color = unactivePortalColor;
    }
}
