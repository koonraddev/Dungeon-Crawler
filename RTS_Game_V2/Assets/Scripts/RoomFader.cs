using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomFader : MonoBehaviour
{
    private GameObject parentObject, destinationObject;
    Vector3 startPos;
    private bool generatingDone = false;
    private bool firstRoom = false;

    public bool FirstRoom { get=> firstRoom; set => firstRoom = value; }

    public GameObject ParentObject
    {
        set
        {
            parentObject = value;
            startPos = gameObject.transform.position;
            gameObject.transform.SetParent(null);
        }
    }
    public GameObject DestinationObject
    { 
        set 
        {
            destinationObject = value;
            gameObject.transform.SetParent(null);
        } 
    }

    private void Awake()
    {
        //startPos = gameObject.transform.position;
    }

    private void OnEnable()
    {
        GameEvents.instance.OnGeneratingReady += Ready;
    }

    private void Update()
    {
        if (generatingDone)
        {
            if (parentObject != null && destinationObject != null)
            {
                //if (parentObject.transform.position.y == 100f && destinationObject.transform.position.y == 100f)
                //{
                //    gameObject.transform.position = (startPos + new Vector3(0, +150f, 0f));
                //}
                //else
                //{
                //    gameObject.transform.position = startPos;
                //    MapManager.instance.ShowPortal(gameObject);
                //}

                if (!parentObject.activeInHierarchy && !destinationObject.activeInHierarchy)
                {
                    gameObject.transform.position = (startPos + new Vector3(0, +150f, 0f));
                }
                else
                {
                    gameObject.transform.position = startPos;
                    MapManager.instance.ShowPortal(gameObject);
                }
            }
        }


    }

    private void Ready()
    {
        if(parentObject == null || destinationObject == null)
        {
            Debug.LogWarning("ZNISZCZONY PORTAL");
            Destroy(gameObject);
            return;
        }

        generatingDone = true;
        if (firstRoom)
        {
            //parentObject.transform.position = new Vector3(parentObject.transform.position.x, 0f, parentObject.transform.position.z);
            parentObject.SetActive(true);
            MapManager.instance.ActiveRoom(parentObject);
        }
        else
        {
            //parentObject.transform.position = new Vector3(parentObject.transform.position.x, 100f, parentObject.transform.position.z);
            parentObject.SetActive(false);
        }
        //destinationObject.transform.position = new Vector3(destinationObject.transform.position.x, 100f, destinationObject.transform.position.z);
        destinationObject.SetActive(false);
    }

    public void Teleport()
    {
        //if(parentObject.transform.position.y <= 10)
        //{
        //    parentObject.transform.position = new Vector3(parentObject.transform.position.x, 100f, parentObject.transform.position.z);
        //    destinationObject.transform.position = new Vector3(destinationObject.transform.position.x, 0f, destinationObject.transform.position.z);
        //    MapManager.instance.ActiveRoom(destinationObject);
        //    MapManager.instance.DeactiveRoom(parentObject);
        //}
        //else
        //{
        //    parentObject.transform.position = new Vector3(parentObject.transform.position.x, 0f, parentObject.transform.position.z);
        //    destinationObject.transform.position = new Vector3(destinationObject.transform.position.x, 100f, destinationObject.transform.position.z);
        //    MapManager.instance.ActiveRoom(parentObject);
        //    MapManager.instance.DeactiveRoom(destinationObject);
        //}

        if (parentObject.activeInHierarchy == true)
        {
            parentObject.SetActive(false);
            destinationObject.SetActive(true);
            MapManager.instance.ActiveRoom(destinationObject);
            MapManager.instance.DeactiveRoom(parentObject);
        }
        else
        {
            parentObject.SetActive(true);
            destinationObject.SetActive(false);
            MapManager.instance.ActiveRoom(parentObject);
            MapManager.instance.DeactiveRoom(destinationObject);
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnGeneratingReady -= Ready;
    }
}
