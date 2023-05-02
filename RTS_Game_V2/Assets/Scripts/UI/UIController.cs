using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerInventoryUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            playerInventoryUI.SetActive(true);
        }
        else
        {
            playerInventoryUI.SetActive(false);
        }
    }
}
