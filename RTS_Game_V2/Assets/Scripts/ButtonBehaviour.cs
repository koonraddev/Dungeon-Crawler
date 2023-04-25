using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    GameObject rootObject;
    private void Start()
    {
        rootObject = gameObject.transform.root.gameObject;
    }
    public void CloseMessageMenu()
    {
        rootObject.SetActive(false);
    }
}
