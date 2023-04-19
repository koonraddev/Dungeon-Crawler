using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chest : MonoBehaviour, IInteractionObjects
{
    private bool opened;

    public void Start()
    {
        opened = false;
    }

    public void OnMouseEnterObject(Color highLightColor)
    {
        if (!opened)
        {
            Material[] objectMaterials;
            objectMaterials = gameObject.GetComponent<Renderer>().materials;
            if (objectMaterials != null)
            {
                foreach (Material objMaterial in objectMaterials)
                {
                    if (objMaterial.color != highLightColor)
                    {
                        objMaterial.DOColor(highLightColor, "_Color", 0.5f);
                    }
                }
            }
        }
    }
    public void OnMouseExitObject()
    {
        Material[] objectMaterials;
        objectMaterials = gameObject.GetComponent<Renderer>().materials;
        if (objectMaterials != null)
        {
            foreach (Material objMaterial in objectMaterials)
            {
                if (objMaterial.color != Color.white)
                {
                    objMaterial.DOColor(Color.white, "_Color", 0.5f);
                }
            }
        }
    }

    public void ObjectInteraction()
    {
        if (!opened)
        {
            gameObject.transform.DOLocalRotate(new Vector3(-140, 0, 0), 2f).SetEase(Ease.OutBounce);
            opened = true;
        }
    }
}
