using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomFader : MonoBehaviour
{
    private GameObject parentObject, destinationObject;
    private List<Transform> parentObjsTransforms,destObjsTransforms;

    Vector3 startPos;
    public GameObject ParentObject
    {
        set
        {
            parentObject = value;
            parentObjsTransforms = new();

            foreach (Transform child in parentObject.transform)
            {
                parentObjsTransforms.Add(child);
            }

            startPos = gameObject.transform.position;
            gameObject.transform.SetParent(null);
        }
    }
    public GameObject DestinationObject
    { 
        set 
        {
            destinationObject = value;
            destObjsTransforms = new();

            foreach (Transform child in destinationObject.transform)
            {
                destObjsTransforms.Add(child);
            }
            gameObject.transform.SetParent(null);
            FadeOut(destObjsTransforms, destinationObject);
        } 
    }

    private void Awake()
    {
        //startPos = gameObject.transform.position;
    }

    private void Update()
    {
        if(parentObject !=null && destinationObject != null)
        {
            if (parentObject.layer == 12 && destinationObject.layer == 12)
            {
                gameObject.transform.position = (startPos + new Vector3(0, +150f, 0f));
            }
            else
            {
                gameObject.transform.position = startPos;
            }
        }

    }

    public void Teleport()
    {
        if (parentObject.layer == 7)
        {
            FadeOut(parentObjsTransforms, parentObject);
            FadeIn(destObjsTransforms, destinationObject);
        }
        else
        {
            FadeOut(destObjsTransforms, destinationObject);
            FadeIn(parentObjsTransforms, parentObject);
        }
    }


    public void FadeOut(List<Transform> transformsList, GameObject mainObject)
    {
        mainObject.layer = 12;
        foreach (Transform child in transformsList)
        {
            child.gameObject.layer = 12;
        }
    }


    public void FadeIn(List<Transform> transformsList, GameObject mainObject)
    {
        mainObject.layer = 7;
        foreach (Transform child in transformsList)
        {
            child.gameObject.layer = 7;
        }
    }
}
